using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    //protected GameObject[] neighbourHex = new GameObject[6];
    //int num_x = -1;
    //int num_z = -1;
    //float axis_x;
    //float axis_z;
    Scene.hexNum hexNum = new Scene.hexNum{
        num_x = -1,
        num_z = -1
    };
    Scene.hexAxis hexA;

    Scene.hexNum[] neighbour = new Scene.hexNum[6]; // start from left up hexagon, clockwise

    void Initialize()
    {
        //for (int i=0; i<6; i++)
            //neighbourHex[i] = null;

        hexA = NumToAxis(hexNum.num_x, hexNum.num_z);
        neighbour = GetNeighbourHex(hexNum);
    }

    public static int NearestAngleForCamera(Scene.hexAxis start, Scene.hexAxis end)
    {
        Vector3 v = new Vector3(end.axis_x - start.axis_x, 0, end.axis_z - start.axis_z);
        float angle = Vector3.Angle(Vector3.forward, v);
        int angleInt = Mathf.RoundToInt((angle-30)/60) * 60 + 30;
        if (Vector3.Cross(v, Vector3.forward).y>0)
            angleInt = -angleInt;
        return angleInt;
    }

    public static Vector3 CenterVectorBetweenTwoHex(Scene.hexNum start, Scene.hexNum end)
    {
        Scene.hexAxis startAxis = NumToAxis(start.num_x, start.num_z);
        Scene.hexAxis endAxis = NumToAxis(end.num_x, end.num_z);
        return new Vector3(endAxis.axis_x - startAxis.axis_x, 0, endAxis.axis_z - startAxis.axis_z);
    }

    public static Scene.hexAxis NumToAxis(int num_x, int num_z)
    {
        Scene.hexAxis hexA;
        hexA.axis_x = Scene.xDistanceHex * num_x;
        if (num_z%2==1)
            hexA.axis_x += Scene.xDistanceHex/2;
        hexA.axis_z = Scene.zDistanceHex * num_z;
        return hexA;
    }

    public static Scene.hexNum AxisToNum(float axis_x, float axis_z)
    {
        Scene.hexNum hexN;
        hexN.num_z = Mathf.RoundToInt(axis_z/Scene.zDistanceHex);
        if (hexN.num_z%2==1)
            axis_x -= Scene.xDistanceHex/2;
        hexN.num_x = Mathf.RoundToInt(axis_x/Scene.xDistanceHex);
        return hexN;
    }

    public static Scene.hexAxis[] NumToAxisArray(Scene.hexNum[] array)
    {
        Scene.hexAxis[] axisArray = new Scene.hexAxis[array.Length];
        for (int i=0; i<array.Length; i++)
        {
            axisArray[i] = NumToAxis(array[i].num_x, array[i].num_z);
        }
        return axisArray;
    }

    public static Scene.hexNum[] GetNoisyEdgesBetweenPointsOffset(Scene.hexNum start, Scene.hexNum end, int level, float range)
    {
        // number of segments = pow(2, level)
        if (level == 0) return GetLineBetweenPointsOffset(start, end);

        int numPoints = (int)Mathf.Pow(2f, level)+1; // include start & end
        Scene.hexNumFloat[] points = new Scene.hexNumFloat[numPoints]; // use Scene.hexAxis to store paired float values, but the syntax is close to Scene.hexNum
        int k1 = (int)Mathf.Pow(2f, level-1);
        int k2 = (int)Mathf.Pow(2f, level-2);
        int k3 = (int)Mathf.Pow(2f, level-3);

        start = OffsetToCubeCoord(start);
        end = OffsetToCubeCoord(end);

        Scene.hexNumFloat start_f = Scene.HexNumToFloat(start);
        Scene.hexNumFloat end_f = Scene.HexNumToFloat(end);
        points[0] = start_f;
        points[points.Length-1] = end_f;
        
        while(true)
        {
            // level = 1
            Scene.hexNumFloat hexNF_1 = new Scene.hexNumFloat{
                num_x_float = start.num_x,
                num_z_float = end.num_z,
            };
            Scene.hexNumFloat hexNF_2 = new Scene.hexNumFloat{
                num_x_float = end.num_x,
                num_z_float = start.num_z,
            };
            points[k1] = Scene.RandomLerp(hexNF_1, hexNF_2, range);

            if(level == 1) break;


            // level = 2
            Scene.hexNumFloat hexNF_3 = Scene.HalfLerp(start_f, hexNF_1);
            Scene.hexNumFloat hexNF_4 = Scene.HalfLerp(start_f, hexNF_2);
            points[k2] = Scene.RandomLerp(hexNF_3, hexNF_4, range);

            Scene.hexNumFloat hexNF_5 = Scene.HalfLerp(end_f, hexNF_2);
            Scene.hexNumFloat hexNF_6 = Scene.HalfLerp(end_f, hexNF_1);
            points[k1+k2] = Scene.RandomLerp(hexNF_5, hexNF_6, range);

            if(level == 2) break;


            // level = 3
            Scene.hexNumFloat hexNF_7 = Scene.HalfLerp(start_f, hexNF_3);
            Scene.hexNumFloat hexNF_8 = Scene.HalfLerp(start_f, hexNF_4);
            points[k3] = Scene.RandomLerp(hexNF_7, hexNF_8, range);

            hexNF_7 = Scene.HalfLerp(hexNF_3, points[k1]);
            hexNF_8 = Scene.HalfLerp(hexNF_4, points[k1]);
            points[k2+k3] = Scene.RandomLerp(hexNF_7, hexNF_8, range);

            hexNF_7 = Scene.HalfLerp(hexNF_5, points[k1]);
            hexNF_8 = Scene.HalfLerp(hexNF_6, points[k1]);
            points[k1+k3] = Scene.RandomLerp(hexNF_7, hexNF_8, range);

            hexNF_7 = Scene.HalfLerp(end_f, hexNF_5);
            hexNF_8 = Scene.HalfLerp(end_f, hexNF_6);
            points[k1+k2+k3] = Scene.RandomLerp(hexNF_7, hexNF_8, range);
            break;
        }

        Scene.hexNum[] noisyEdge = new Scene.hexNum[0];
        Scene.hexNum[] line;

        Scene.hexNum[] pointsRounded = new Scene.hexNum[points.Length];
        for (int i=0; i<points.Length; i++)
        {
            pointsRounded[i] = CubeRound(points[i].num_x_float, points[i].num_z_float);
        }

        for (int i=0; i<points.Length-1; i++)
        {
            line = GetLineBetweenPointsCube(pointsRounded[i], pointsRounded[i+1]);
            Utils.Add(ref noisyEdge, pointsRounded[i]);
            if (line != null)
                Utils.Add(ref noisyEdge, line);
        }
        Utils.Add(ref noisyEdge, pointsRounded[points.Length-1]);

        for(int i=0; i<noisyEdge.Length; i++)
        {
            noisyEdge[i] = CubeToOffsetCoord(noisyEdge[i]);
            if (noisyEdge[i].num_x<0) noisyEdge[i].num_x=0;
            if (noisyEdge[i].num_x>=Scene.areaHeight) noisyEdge[i].num_x=Scene.areaHeight-1;
            if (noisyEdge[i].num_z<0) noisyEdge[i].num_z=0;
            if (noisyEdge[i].num_z>=Scene.areaWidth) noisyEdge[i].num_z=Scene.areaWidth-1;
        }
        return noisyEdge;
    }

    public static Scene.hexNum[] GetTwistedEdgesBetweenPointsOffset(Scene.hexNum start, Scene.hexNum end, float range)
    {
        // twist twice
        Scene.hexNumFloat[] points = new Scene.hexNumFloat[4];
        start = OffsetToCubeCoord(start);
        end = OffsetToCubeCoord(end);

        Scene.hexNumFloat start_f = Scene.HexNumToFloat(start);
        Scene.hexNumFloat end_f = Scene.HexNumToFloat(end);
        points[0] = start_f;
        points[points.Length-1] = end_f;
        
        Scene.hexNumFloat hexNF_1 = new Scene.hexNumFloat{
            num_x_float = start.num_x,
            num_z_float = end.num_z,
        };
        Scene.hexNumFloat hexNF_2 = new Scene.hexNumFloat{
            num_x_float = end.num_x,
            num_z_float = start.num_z,
        };

        Scene.hexNumFloat hexNF_3 = Scene.Lerp(start_f, hexNF_1, 0.33f);
        Scene.hexNumFloat hexNF_4 = Scene.Lerp(start_f, hexNF_2, 0.33f);
        points[1] = Scene.RandomLerp(hexNF_3, hexNF_4, range);
        hexNF_3 = Scene.Lerp(end_f, hexNF_1, 0.33f);
        hexNF_4 = Scene.Lerp(end_f, hexNF_2, 0.33f);
        points[2] = Scene.RandomLerp(hexNF_3, hexNF_4, range);

        Scene.hexNum[] edge = new Scene.hexNum[0];
        Scene.hexNum[] line;

        Scene.hexNum[] pointsRounded = new Scene.hexNum[points.Length];
        for (int i=0; i<points.Length; i++)
        {
            pointsRounded[i] = CubeRound(points[i].num_x_float, points[i].num_z_float);
        }

        for (int i=0; i<points.Length-1; i++)
        {
            line = GetLineBetweenPointsCube(pointsRounded[i], pointsRounded[i+1]);
            Utils.Add(ref edge, pointsRounded[i]);
            if (line != null)
                Utils.Add(ref edge, line);
        }
        Utils.Add(ref edge, pointsRounded[points.Length-1]);

        for(int i=0; i<edge.Length; i++)
        {
            edge[i] = CubeToOffsetCoord(edge[i]);
            if (edge[i].num_x<0) edge[i].num_x=0;
            if (edge[i].num_x>=Scene.areaHeight) edge[i].num_x=Scene.areaHeight-1;
            if (edge[i].num_z<0) edge[i].num_z=0;
            if (edge[i].num_z>=Scene.areaWidth) edge[i].num_z=Scene.areaWidth-1;
        }
        return edge;
    }

    public static Scene.hexNum[] GetLineBetweenPointsOffset(Scene.hexNum start, Scene.hexNum end)
    {
        start = OffsetToCubeCoord(start);
        end = OffsetToCubeCoord(end);
        Scene.hexNum[] line = GetLineBetweenPointsCube(start, end);
        if (line == null)   return null;
        for(int i=0; i<line.Length; i++)
        {
            line[i] = CubeToOffsetCoord(line[i]);
        }
        return line;
    }

    public static Scene.hexNum[] GetLineBetweenPointsCube(Scene.hexNum start, Scene.hexNum end)
    {
        int distance = DistanceBetweenPointsCube(start, end);
        if (distance == 0 || distance == 1) return null;

        Scene.hexNum[] line = new Scene.hexNum[distance-1]; // do not include start & end

        float x = start.num_x;
        float z = start.num_z;
        //float y = -start.num_x-start.num_z;
        float delta_x = (end.num_x - start.num_x)/(float)distance;
        float delta_z = (end.num_z - start.num_z)/(float)distance;
        //float delta_y = (-(end.num_x+end.num_z) + (start.num_x+start.num_z))/(float)distance;

        for(int i=1; i<distance; i++)
        {

            x += delta_x;
            //y += delta_y;
            z += delta_z;
            line[i-1] = CubeRound(x, z);
        }
        return line;
    }

    public static Scene.hexNum CubeRound(float x, float z)
    {
        // round float to int under cube coordinate (find the cude the float point is on)
        float y = -x-z;
        int num_x = Mathf.RoundToInt(x);
        int num_z = Mathf.RoundToInt(z);
        int num_y = Mathf.RoundToInt(y);

        float x_diff = Mathf.Abs(num_x-x);
        float y_diff = Mathf.Abs(num_y-y);
        float z_diff = Mathf.Abs(num_z-z);

        if (x_diff>y_diff && x_diff>z_diff)
            num_x = -num_z-num_y;
        else if (y_diff>z_diff)
            num_y = -num_x-num_z;
        else num_z = -num_x-num_y;
        return new Scene.hexNum {
            num_x = num_x,
            num_z = num_z
        };
    }
    
    public static Scene.hexNum OffsetToCubeCoord(Scene.hexNum hexN)
    {
        Scene.hexNum cube;
        cube.num_z = hexN.num_z;
        cube.num_x = hexN.num_x - hexN.num_z/2;
        // y = -x-z
        return cube;
    }

    public static Scene.hexNum CubeToOffsetCoord(Scene.hexNum hexN)
    {
        Scene.hexNum offset;
        offset.num_z = hexN.num_z;
        offset.num_x = hexN.num_x + hexN.num_z/2;
        // y = -x-z
        return offset;
    }

    public static int DistanceBetweenPointsOffset(Scene.hexNum start, Scene.hexNum end)
    {
        start = OffsetToCubeCoord(start);
        end = OffsetToCubeCoord(end);
        return DistanceBetweenPointsCube(start, end);
    }

    public static int DistanceBetweenPointsCube(Scene.hexNum start, Scene.hexNum end)
    {
        // calculate under cude coordinate
        int distance_x = Mathf.Abs(start.num_x-end.num_x);
        int distance_z = Mathf.Abs(start.num_z-end.num_z);
        int distance_y = Mathf.Abs(-(start.num_x+start.num_z)+(end.num_x+end.num_z));
        return (distance_x+distance_y+distance_z)/2;
    }

    public static Scene.hexNum[] GetRandomArea(Scene.hexNum hexN, int height, int width)
    {
        Scene.hexNum[] area = new Scene.hexNum[0];
        Scene.hexNum[] row;
        Scene.hexNum bottomRowCenter, upRowCenter;
        int rowLength, randomStart, randomHeight=height;
        
        bottomRowCenter = new Scene.hexNum {
            num_x = hexN.num_x + (height-1)/2,
            num_z = hexN.num_z
        };

        upRowCenter = new Scene.hexNum { // temporary value
            num_x = -1,
            num_z = -1
        };

        for (int i=1; i<=width; i++)
        {
            row = GetRow(hexN, randomHeight);
            rowLength = row.Length;
            Utils.Add(ref area, row);

            if (i==width)
            {
                upRowCenter = new Scene.hexNum {
                    num_x = hexN.num_x + (randomHeight-1)/2,
                    num_z = hexN.num_z
                };
            }

            randomStart = UnityEngine.Random.Range(-1+hexN.num_z%2, 1+hexN.num_z%2); // -1, 0, 1
            hexN.num_x += randomStart;
            hexN.num_z += 1;
            if (hexN.num_z>=Scene.areaWidth) break;
            if (hexN.num_x<0) {
                hexN.num_x=0;
                randomStart=0;
            }

            randomHeight = rowLength + UnityEngine.Random.Range(-1-randomStart, 3);
            if(randomHeight<=0) randomHeight = 1;
            if(height>=3 && randomHeight>=1.5f*height) randomHeight = Mathf.RoundToInt(1.5f*height);
        }
        Utils.Add(ref area, bottomRowCenter);
        Utils.Add(ref area, upRowCenter);
        return area;
    }

    public static Scene.hexNum[] GetRow(Scene.hexNum hexN, int height)
    {
        if (hexN.num_x+height >= Scene.areaHeight)
            height = Scene.areaHeight-hexN.num_x-1;
        Scene.hexNum[] row = new Scene.hexNum[height];
        Scene.hexNum tmp = hexN;
        for (int i=0; i<height; i++)
        {
            row[i] = tmp;
            tmp = GetRightHex(tmp);
        }
        return row;
    }
    public static Scene.hexNum[] GetSpiralRing(Scene.hexNum hexN, int radius)
    {
        // radius=1: 1 hex
        Scene.hexNum[] ring = new Scene.hexNum[3*radius*(radius-1)+1];
        Scene.hexNum[] ring_tmp;

        int count = 0;
        for (int i=radius; i>=2; i--)
        {
            ring_tmp = GetRing(hexN, i-1);
            if (ring_tmp == null) return null;
            Array.Copy(ring_tmp, 0, ring, count, 6*(i-1));
            count += 6*(i-1);
        }
        ring[count++] = hexN;
        return ring;
        // 6 vertices: ring[radius-2, 2*radius-3, 3*radius-4, 4*radius-5, 5*radius-6, 6*radius-7]
        // left down, right down, right, right up, left up, left
    }

    public static Scene.hexNum[] GetRing(Scene.hexNum hexN, int distance)
    {
        Scene.hexNum[] ring = new Scene.hexNum[distance*6];
        Scene.hexNum hexN_1 = hexN;
        for (int i=1; i<=distance; i++)
        {
            hexN_1 = GetLeftHex(hexN_1);
            if (hexN_1.num_x == -1) return null;
        }
        
        int count = 0;
        for (int i=1; i<=distance; i++)
        {
            hexN_1 = GetRightDownHex(hexN_1);
            if (hexN_1.num_x == -1) return null;
            ring[count++] = hexN_1;
        }

        for (int i=1; i<=distance; i++)
        {
            hexN_1 = GetRightHex(hexN_1);
            if (hexN_1.num_x == -1) return null;
            ring[count++] = hexN_1;
        }

        for (int i=1; i<=distance; i++)
        {
            hexN_1 = GetRightUpHex(hexN_1);
            if (hexN_1.num_x == -1) return null;
            ring[count++] = hexN_1;
        }

        for (int i=1; i<=distance; i++)
        {
            hexN_1 = GetLeftUpHex(hexN_1);
            if (hexN_1.num_x == -1) return null;
            ring[count++] = hexN_1;
        }

        for (int i=1; i<=distance; i++)
        {
            hexN_1 = GetLeftHex(hexN_1);
            if (hexN_1.num_x == -1) return null;
            ring[count++] = hexN_1;
        }

        for(int i=1; i<=distance; i++)
        {
            hexN_1 = GetLeftDownHex(hexN_1);
            if (hexN_1.num_x == -1) return null;
            ring[count++] = hexN_1;
        }

        return ring;
    }

    // Get 6 neighbour hexagons
    public static Scene.hexNum[] GetNeighbourHex(Scene.hexNum hexN, bool OutOfBound=true)
    {
        Scene.hexNum[] neighbour = new Scene.hexNum[6];
        // some values can possibly be null
        neighbour[0] = GetLeftUpHex(hexN, OutOfBound);
        neighbour[1] = GetRightUpHex(hexN, OutOfBound);
        neighbour[2] = GetRightHex(hexN, OutOfBound);
        neighbour[3] = GetRightDownHex(hexN, OutOfBound);
        neighbour[4] = GetLeftDownHex(hexN, OutOfBound);
        neighbour[5] = GetLeftHex(hexN, OutOfBound);
        return neighbour;
    }

    public static Scene.hexNum GetHexByAngle(Scene.hexNum hexN, int angle, bool OutOfBound=true)
    {
        Scene.hexNum result = new Scene.hexNum {num_x=-1, num_z=-1};
        if (angle > 150) angle -= 360;
        if (angle < -150) angle += 360;
        switch (angle)
        {
            case 30:
                result = GetRightUpHex(hexN, OutOfBound); break;
            case 90:
                result = GetRightHex(hexN, OutOfBound); break;
            case 150:
                result = GetRightDownHex(hexN, OutOfBound); break;
            case -150:
                result = GetLeftDownHex(hexN, OutOfBound); break;
            case -90:
                result = GetLeftHex(hexN, OutOfBound); break;
            case -30:
                result = GetLeftUpHex(hexN, OutOfBound); break;
            default:
                break;
        }
        return result;
    }
    
    public static Scene.hexNum GetLeftUpHex(Scene.hexNum hexN, bool OutOfBound=true)
    {
        Scene.hexNum tmp;
        if(hexN.num_z % 2 == 0)
        {
            tmp.num_x = hexN.num_x-1;
            tmp.num_z = hexN.num_z+1;
        }
        else {
            tmp.num_x = hexN.num_x;
            tmp.num_z = hexN.num_z+1;
        }
        if (OutOfBound && (tmp.num_x<0 || tmp.num_z>=Scene.areaWidth)) return new Scene.hexNum {num_x=-1, num_z=-1};; // outside boundary
        return tmp;
    }

    public static Scene.hexNum GetRightUpHex(Scene.hexNum hexN, bool OutOfBound=true)
    {
        Scene.hexNum tmp;
        if(hexN.num_z % 2 == 0)
        {
            tmp.num_x = hexN.num_x;
            tmp.num_z = hexN.num_z+1;
        }
        else {
            tmp.num_x = hexN.num_x+1;
            tmp.num_z = hexN.num_z+1;
        }
        if (OutOfBound && (tmp.num_x>=Scene.areaHeight || tmp.num_z>=Scene.areaWidth)) return new Scene.hexNum {num_x=-1, num_z=-1}; // outside boundary
        return tmp;
    }

    public static Scene.hexNum GetRightHex(Scene.hexNum hexN, bool OutOfBound=true)
    {
        Scene.hexNum tmp;
        tmp.num_x = hexN.num_x+1;
        tmp.num_z = hexN.num_z;
        if (OutOfBound && tmp.num_x>=Scene.areaHeight) return new Scene.hexNum {num_x=-1, num_z=-1};; // outside boundary
        return tmp;
    }

    public static Scene.hexNum GetRightDownHex(Scene.hexNum hexN, bool OutOfBound=true)
    {
        Scene.hexNum tmp;
        if(hexN.num_z % 2 == 0)
        {
            tmp.num_x = hexN.num_x;
            tmp.num_z = hexN.num_z-1;
        }
        else {
            tmp.num_x = hexN.num_x+1;
            tmp.num_z = hexN.num_z-1;
        }
        if (OutOfBound && (tmp.num_x>=Scene.areaHeight || tmp.num_z<0)) return new Scene.hexNum {num_x=-1, num_z=-1};;
        return tmp;
    }

    public static Scene.hexNum GetLeftDownHex(Scene.hexNum hexN, bool OutOfBound=true)
    {
        Scene.hexNum tmp;
        if(hexN.num_z % 2 == 0)
        {
            tmp.num_x = hexN.num_x-1;
            tmp.num_z = hexN.num_z-1;
        }
        else {
            tmp.num_x = hexN.num_x;
            tmp.num_z = hexN.num_z-1;
        }
        if (OutOfBound && (tmp.num_x<0 || tmp.num_z<0)) return new Scene.hexNum {num_x=-1, num_z=-1};;
        return tmp;
    }

    public static Scene.hexNum GetLeftHex(Scene.hexNum hexN, bool OutOfBound=true)
    {
        Scene.hexNum tmp;
        tmp.num_x = hexN.num_x-1;
        tmp.num_z = hexN.num_z;
        if (OutOfBound && tmp.num_x<0) return new Scene.hexNum {num_x=-1, num_z=-1};;
        return tmp;
    }
}
