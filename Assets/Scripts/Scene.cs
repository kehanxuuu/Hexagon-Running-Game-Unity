using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    public static bool gameStopped = false;
    public static bool coinRestart = false;
    public const float xDistanceHex = 1.1f;
    public const float zDistanceHex = 1.73205080757f / 2 * 1.1f; //Mathf.Sqrt(3) = 1.73205080757
    public const int areaHeight = 50;
    public const int areaWidth = 300;
    public static GameObject[] map;
    public static GameObject[] coin;
    public static bool[] hexOccupied;
    public static bool[] sceneOccupied;
    public static bool[] widthOccupied;
    public static bool[] coinOccupied;
    public static bool longArea = true;
    public static bool recoverBomb = false;
    public struct hexNum
    {
        public int num_x;
        public int num_z;

        public static bool operator< (hexNum a, hexNum b)
        {
            if(a.num_z < b.num_z) return true;
            if(a.num_z == b.num_z && a.num_x < b.num_x) return true;
            return false;
        }

        public static bool operator> (hexNum a, hexNum b)
        {
            if(a.num_z > b.num_z) return true;
            if(a.num_z == b.num_z && a.num_x > b.num_x) return true;
            return false;
        }

        public static bool operator== (hexNum a, hexNum b)
        {
            if(a.num_x == b.num_x && a.num_z == b.num_z)
                return true;
            return false;
        }

        public static bool operator!= (hexNum a, hexNum b)
        {
            if(a.num_x != b.num_x || a.num_z != b.num_z)
                return true;
            return false;
        }

        public static hexNum operator+ (hexNum a, hexNum b)
        {
            return new hexNum {
                num_x = a.num_x + b.num_x,
                num_z = a.num_z + b.num_z,
            };
        }
    }

    public hexNum defaultValue = new hexNum{num_x=-1, num_z=-1};

    public struct hexNumFloat
    {
        public float num_x_float;
        public float num_z_float;
    }

    public struct hexAxis
    {
        public float axis_x;
        public float axis_z;

        public static bool operator< (hexAxis a, hexAxis b)
        {
            if(a.axis_x < b.axis_x) return true;
            if(a.axis_x == b.axis_x && a.axis_z < b.axis_z) return true;
            return false;
        }

        public static bool operator> (hexAxis a, hexAxis b)
        {
            if(a.axis_x > b.axis_x) return true;
            if(a.axis_x == b.axis_x && a.axis_z > b.axis_z) return true;
            return false;
        }

    }

    public struct areaStartEnd
    {
        public hexNum start;
        public hexNum end;
        public bool same;

        public static bool operator< (areaStartEnd a, areaStartEnd b)
        {
            int a_x_sum = a.start.num_x+a.end.num_x;
            int a_z_sum = a.start.num_z+a.end.num_z;
            int b_x_sum = b.start.num_x+b.end.num_x;
            int b_z_sum = b.start.num_z+b.end.num_z;
            if(a_z_sum < b_z_sum) return true;
            if(a_z_sum == b_z_sum && a_x_sum < b_x_sum) return true;
            return false;
        }

        public static bool operator> (areaStartEnd a, areaStartEnd b)
        {
            int a_x_sum = a.start.num_x+a.end.num_x;
            int a_z_sum = a.start.num_z+a.end.num_z;
            int b_x_sum = b.start.num_x+b.end.num_x;
            int b_z_sum = b.start.num_z+b.end.num_z;
            if(a_z_sum > b_z_sum) return true;
            if(a_z_sum == b_z_sum && a_x_sum > b_x_sum) return true;
            return false;
        }
    }

    public static areaStartEnd[][] hexCenter = new areaStartEnd[9][];
    public static hexNum[] checkpointPos;
    public static hexAxis[] checkpointPosAxis;

    void Awake()
    {
        Initialize();
    }

    public static void Initialize()
    {
        map = new GameObject[areaHeight*areaWidth];
        coin = new GameObject[areaHeight*areaWidth];
        hexOccupied = new bool[areaHeight*areaWidth];
        sceneOccupied = new bool[areaHeight*areaWidth];
        coinOccupied = new bool[areaHeight*areaWidth];
        widthOccupied = new bool[areaWidth];
        for (int i=0; i<areaHeight*areaWidth; i++)
        {
            hexOccupied[i] = false;
            sceneOccupied[i] = false;
            coinOccupied[i] = false;
        }
        for (int i=0; i<areaWidth; i++)
            widthOccupied[i] = false;
        for (int i=0; i<9; i++)
            hexCenter[i] = new areaStartEnd[0];
    }

    public static void GetCheckpointPosValue()
    {
        if (longArea)
        {
            //if (checkpointPos == null)
            checkpointPos = new hexNum[] {
                new hexNum {
                    num_x = UnityEngine.Random.Range(5, areaHeight-5),
                    num_z = areaWidth*1/5 + UnityEngine.Random.Range(-2, 3)
                },
                new hexNum {
                    num_x = UnityEngine.Random.Range(5, areaHeight-5),
                    num_z = areaWidth*2/5 + UnityEngine.Random.Range(-2, 3)
                },
                new hexNum {
                    num_x = UnityEngine.Random.Range(5, areaHeight-5),
                    num_z = areaWidth*3/5 + UnityEngine.Random.Range(-2, 3)
                },
                new hexNum {
                    num_x = UnityEngine.Random.Range(5, areaHeight-5),
                    num_z = areaWidth*4/5 + UnityEngine.Random.Range(-2, 3)
                },
            };
        }
        else {
            //if (checkpointPos == null)
            checkpointPos = new hexNum[] {
                new hexNum {
                    num_x = UnityEngine.Random.Range(3, 7),
                    num_z = areaWidth*2/3 + UnityEngine.Random.Range(-2, 3)
                },
                new hexNum {
                    num_x = areaHeight*2/3 - UnityEngine.Random.Range(1, 4),
                    num_z = areaWidth - UnityEngine.Random.Range(3, 7)
                },
                new hexNum {
                    num_x = areaHeight/3 + UnityEngine.Random.Range(1, 4),
                    num_z = UnityEngine.Random.Range(3, 7)
                },
                new hexNum {
                    num_x = areaHeight - UnityEngine.Random.Range(3, 7),
                    num_z = areaWidth/3 + UnityEngine.Random.Range(-2, 3)
                },
            };
        }
        //if (checkpointPosAxis == null)
        checkpointPosAxis = Hexagon.NumToAxisArray(checkpointPos);
    }

    public static void SortHexagonCenter()
    {
        if (longArea)
        {
            Utils.Sort(ref hexCenter[0]);
            //Debug.Log(hexCenter[0].Length);
        }
        else {
            for (int i=0; i<9; i++)
                Utils.Sort(ref hexCenter[i]);
            
            // reverse the order in the middle column
            for (int i=3; i<6; i++)
            {
                hexCenter[i] = Utils.Reverse(hexCenter[i]);
            }
        }
    }

    public static areaStartEnd GetNextHexagonCenter(int area, int num)
    {
        // area: 0-8
        if (num+1<hexCenter[area].Length)
            return hexCenter[area][num+1];
        // search next subarea
        while(area<8)
        {
            if (hexCenter[++area].Length > 0)
                return hexCenter[area][0];
        }
        return GetAreaStartEndDefaultValue(); // the last one, no next element
    }

    public static void AddHexagonCenter(hexNum hexN, hexNum hexN_new, bool same=true)
    {
        if (longArea)
        {
            Utils.Add(ref hexCenter[0], CreateAreaStartEnd(hexN, hexN_new, same));
        }
        else {
            if (hexN.num_x>=0 && hexN.num_x<areaHeight/3)
            {
                if (hexN.num_z>=0 && hexN.num_z<areaWidth/3)
                    Utils.Add(ref hexCenter[0], CreateAreaStartEnd(hexN, hexN_new, same));
                else if (hexN.num_z>=areaWidth/3 && hexN.num_z<areaWidth*2/3)
                    Utils.Add(ref hexCenter[1], CreateAreaStartEnd(hexN, hexN_new, same));
                else //hexN.num_z>=areaWidth*2/3 && hexN.num_z<areaWidth
                    Utils.Add(ref hexCenter[2], CreateAreaStartEnd(hexN, hexN_new, same));
            }

            else if (hexN.num_x>=areaHeight/3 && hexN.num_x<areaHeight*2/3)
            {
                if (hexN.num_z>=0 && hexN.num_z<areaWidth/3)
                    Utils.Add(ref hexCenter[5], CreateAreaStartEnd(hexN, hexN_new, same));
                else if (hexN.num_z>=areaWidth/3 && hexN.num_z<areaWidth*2/3)
                    Utils.Add(ref hexCenter[4], CreateAreaStartEnd(hexN, hexN_new, same));
                else //hexN.num_z>=areaWidth*2/3 && hexN.num_z<areaWidth
                    Utils.Add(ref hexCenter[3], CreateAreaStartEnd(hexN, hexN_new, same));
            }

            else //hexN.num_x>=areaHeight*2/3 && hexN.num_x<areaHeight
            {
                if (hexN.num_z>=0 && hexN.num_z<areaWidth/3)
                    Utils.Add(ref hexCenter[6], CreateAreaStartEnd(hexN, hexN_new, same));
                else if (hexN.num_z>=33 && hexN.num_z<areaWidth*2/3)
                    Utils.Add(ref hexCenter[7], CreateAreaStartEnd(hexN, hexN_new, same));
                else //hexN.num_z>=areaWidth*2/3 && hexN.num_z<areaWidth
                    Utils.Add(ref hexCenter[8], CreateAreaStartEnd(hexN, hexN_new, same));
            }
        }
    }

    public static areaStartEnd CreateAreaStartEnd(hexNum hexN, hexNum hexN_new, bool same=true)
    {
        areaStartEnd center;
        center.start = hexN;
        center.end = hexN_new;
        center.same = same;
        return center;
    }

    public static areaStartEnd GetAreaStartEndDefaultValue()
    {
        areaStartEnd center;
        center.start = new hexNum{num_x=-1, num_z=-1};
        center.end = new hexNum{num_x=-1, num_z=-1};
        center.same = true;
        return center;
    }

    /*public hexNum GetStartInAreaStartEnd(areaStartEnd center)
    {
        return areaStartEnd.start;
    }

    public hexNum GetEndInAreaStartEnd(areaStartEnd center)
    {
        if (center.same)
            return areaStartEnd.start;
        else return areaStartEnd.end;
    }*/

    public static bool JudgeWidthOccupancy(int z, int width)
    {
        if (z<0) return false;
        if (z+width >= areaWidth) return false;
        for (int i=0; i<width; i++)
        {
            if (widthOccupied[i+z]) return false;
        }
        return true;
    }

    public static void SetWidthOccupancy(int z, int width)
    {
        for (int i=0; i<width; i++)
        {
            Debug.Assert(!widthOccupied[i+z], "Width already occupied in SetWidthOccupancy");
            widthOccupied[i+z] = true;
        }
    }

    public static bool JudgeRectAreaOccupancy(int start_x, int start_z, int height, int width)
    {
        if (start_x+height >= areaHeight) return false;
        if (start_z+width >= areaWidth) return false;
        for (int i=0; i<height; i++)
            for (int j=0; j<width; j++)
            {
                int num = GetOneDimensionVal(start_x+i, start_z+j);
                try {
                    if (sceneOccupied[num] || hexOccupied[num]) return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        return true;
    }

    public static void SetRectAreaOccupancy(int start_x, int start_z, int height, int width, bool sceneOccupancy)
    {
        for (int i=0; i<height; i++)
            for (int j=0; j<width; j++)
            {
                int num = GetOneDimensionVal(start_x+i, start_z+j);
                if (sceneOccupancy)
                {
                    //Debug.Assert(!sceneOccupied[num], "Scene block already occupied in SetRectAreaOccupancy");
                    sceneOccupied[num] = true;
                }
                else {
                    //Debug.Assert(!hexOccupied[num], "Hex block already occupied in SetRectAreaOccupancy");
                    hexOccupied[num] = true;
                }
            }
    }

    public static bool JudgeAreaOccupancy(hexNum[] area)
    {
        for (int i=0; i<area.Length; i++)
        {
            int num = GetOneDimensionVal(area[i].num_x, area[i].num_z);
            if (hexOccupied[num] || sceneOccupied[num]) return false;
        }
        return true;
    }

    public static void SetAreaOccupancy(hexNum[] area, bool sceneOccupancy)
    {
        for (int i=0; i<area.Length; i++)
        {
            Debug.Assert(area[i].num_x >= 0 && area[i].num_x < areaHeight && area[i].num_z >= 0 && area[i].num_z < areaWidth, "Area out of range in SetAreaOccupancy");
            int num = GetOneDimensionVal(area[i].num_x, area[i].num_z);
            if (sceneOccupancy)
            {
                 Debug.Assert(!sceneOccupied[num], "Scene block already occupied in SetAreaOccupancy");
                sceneOccupied[num] = true;
            }
            else{
                Debug.Assert(!hexOccupied[num], "Hex block already occupied in SetAreaOccupancy");
                hexOccupied[num] = true;
            }
        }
    }

    public static bool JudgeRectInRing(int rect_start_x, int rect_start_z, int height, int width, hexNum[] ring)
    {
        // detect 4 angles
        bool rectInRing = SearchHexNum(ring, rect_start_x, rect_start_z) &&
            SearchHexNum(ring, rect_start_x+height-1, rect_start_z) &&
            SearchHexNum(ring, rect_start_x, rect_start_z+width-1) &&
            SearchHexNum(ring, rect_start_x+height-1, rect_start_z+width-1);
        return rectInRing;
    }

    public static hexAxis GetRectAreaCenter(int start_x, int start_z, int height, int width)
    {
        //return Hexagon.NumToAxis(start_x, start_z);
        hexAxis hexA_1, hexA_2, hexA_3, hexA_4, hexA_5, hexA_6, hexA_7;
        if( width == 1)
        {
            if (height % 2 == 1)
                return Hexagon.NumToAxis(start_x+height/2, start_z);
            // height % 2 == 0
            hexA_1 = Hexagon.NumToAxis(start_x+height/2-1, start_z);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2, start_z);
            hexA_3 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x)/2,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z)/2
            };
            return hexA_3;
        }
        //height == 1 && width > 1 -> no such area

        if (width % 2 == 0 && height % 2 ==1)
        {
            hexA_1 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2-1);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2);
            hexA_3 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x)/2,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z)/2
            };
            return hexA_3;
        }
        else if (width % 2 == 0 && height % 2 ==0)
        {
            hexA_1 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2-1);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2-1);
            hexA_3 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2);
            hexA_4 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2);
            hexA_5 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x + hexA_3.axis_x + hexA_4.axis_x)/4,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z + hexA_3.axis_z + hexA_4.axis_z)/4
            };
            return hexA_5;
        }
        else if (width % 2 ==1 && height % 2 ==1)
        {
            hexA_1 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2-1);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2);
            hexA_3 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2+1);
            hexA_4 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x + hexA_3.axis_x)/3,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z + hexA_3.axis_z)/3
            };
            return hexA_4;
        }
        else if (width % 2 ==1 && height % 2 ==0)
        {
            hexA_1 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2-1);
            hexA_2 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2);
            hexA_3 = Hexagon.NumToAxis(start_x+height/2-1, start_z+width/2+1);
            hexA_4 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2-1);
            hexA_5 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2);
            hexA_6 = Hexagon.NumToAxis(start_x+height/2, start_z+width/2+1);
            hexA_7 = new hexAxis {
                axis_x = (hexA_1.axis_x + hexA_2.axis_x + hexA_3.axis_x + hexA_4.axis_x + hexA_5.axis_x + hexA_6.axis_x)/6,
                axis_z = (hexA_1.axis_z + hexA_2.axis_z + hexA_3.axis_z + hexA_4.axis_z + hexA_5.axis_z + hexA_6.axis_z)/6
            };
            return hexA_7;
        }
        // won't reach here
        hexA_1 = new hexAxis {axis_x=-1, axis_z=-1};
        return hexA_1;
    }

    public static int GetOneDimensionVal(int x, int z)
    {
        return x*areaWidth + z;
    }

    public static bool SearchHexNum(hexNum[] array, int x, int z)
    {
        for (int i=0; i<array.Length; i++)
        {
            if (array[i].num_x == x && array[i].num_z == z) return true;
        }
        return false;
    }

    public static hexNumFloat HexNumToFloat(hexNum point)
    {
        return new hexNumFloat {
            num_x_float = point.num_x,
            num_z_float = point.num_z
        };
    }

    public static hexNumFloat Lerp(hexNumFloat start, hexNumFloat end, float ratio)
    {
        Debug.Assert(ratio>=0 && ratio<=1, "ratio out of [0, 1] in Lerp");
        hexNumFloat tmp;
        tmp.num_x_float = ratio*start.num_x_float + (1-ratio)*end.num_x_float;
        tmp.num_z_float = ratio*start.num_z_float + (1-ratio)*end.num_z_float;
        return tmp;
    }

    public static hexNumFloat RandomLerp(hexNumFloat start, hexNumFloat end, float range)
    {
        float division = UnityEngine.Random.Range(0.5f-range/2, 0.5f+range/2);
        //Debug.Log("division: "+division);
        hexNumFloat tmp = Lerp(start, end, division);
        return tmp;
    }

    public static hexNumFloat HalfLerp(hexNumFloat start, hexNumFloat end)
    {
        return Lerp(start, end, 0.5f);
    }

    public static hexNum Lerp(hexNum start, hexNum end, float ratio)
    {
        Debug.Assert(ratio>=0 && ratio<=1, "ratio out of [0, 1] in Lerp");
        hexNum tmp;
        tmp.num_x = Mathf.RoundToInt(ratio*start.num_x + (1-ratio)*end.num_x);
        tmp.num_z = Mathf.RoundToInt(ratio*start.num_z + (1-ratio)*end.num_z);
        return tmp;
    }

    public static hexNum HalfLerp(hexNum start, hexNum end)
    {
        return Lerp(start, end, 0.5f);
    }

}
