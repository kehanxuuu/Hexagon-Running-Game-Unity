using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

//[ExecuteInEditMode]
public class GenerateScene : MonoBehaviour
{
    public int randomSeed;
    public int checkpointNum;
    GameObject[] objects;
    GameObject[] hex;
    GameObject finalCheckpoint, coin, diamond, AICharacter, Portal;
    GameObject terrain;
    public static string[] hex_name = {
        "grass",
        "sand",
        "stone",
        "water",
        "speedUp",
        "speedDown",
        "jump",
        "checkpoint",
        "start",
        "bomb",
    };

    string[] pattern_object = {
        "RockCliff-03", //6*4
        "RockCliff-01", //4*4
        "Tree-04", //3*4
        //"Tree-02", //2*2
        "Tree-05", //2*3
        "TreeBirch-01", //2*2
        //"TreeBirch-03", //2*2
        "TreeBirch-05", //2*3
        //"TreeBirch-06", //2*2
        "TreeDead-04", //2*2
        "RoadSign-01", //2*2
        "ShrubTall-01", //2*2
        "PondReed-02", //1*1
        "Tree_Sml-01", //1*1
        //"Tree_Sml-02", //1*1
        "Tree_Sml-03", //1*1
        //"Tree_Sml-04", //1*1
    };
    int[] pattern_object_num = {
        1, //"RockCliff-03", 6*4
        1, //"RockCliff-01", 4*4
        1, //"Tree-04", 3*4
        //1, //"Tree-02", 2*2
        1, //"Tree-05", 2*3
        1, //"TreeBirch-01", 2*2
        //1, //"TreeBirch-03", 2*2
        1, //"TreeBirch-05", 2*3
        //1, //"TreeBirch-06", 2*2
        1, //"TreeDead-04", 2*2
        1, //"RoadSign-01", 2*2
        1, //"ShrubTall-01", 2*2
        1, //"PondReed-02", 1*1
        1, //"Tree_Sml-01", 1*1
        //1, //"Tree_Sml-02", 1*1
        1, //"Tree_Sml-03", 1*1
        //1, //"Tree_Sml-04", 1*1
    };

    string[] small_object = {
        "Tree-01", //4*5
        "Tree-03", //5*5
        "Tree-04", //3*4
        "Tree-05", //2*3
        "RoadFenceWood_Damaged-01", //3*3
        "RoadFenceWood_Lrg-01", //2*4
        "RoadFenceWood_Sml-03", //2*1
        "RockMed-02", //2*1
        "ShrubTall-01", //2*2
        "ShrubTall-02", //1*1
        "TreeLog_B-02", //2*1
        "TreeLog_B-03", //2*2
        "TreeStump_A-02", //2*2
        "TreeStump_B-03", //2*2
        "RoadSign-01", //2*2
        "RoadLamp-01", //1*1
        "PondReed-02", //1*1
        /*"RockSml-02", //1*1
        "ShrubShort-02", //1*1
        "Grass-01", //1*1
        "Grass-02", //1*1 */
    };

    int[] small_object_num = {
        20, //"Tree-01", 4*5
        20, //"Tree-03", 5*5
        20, //"Tree-04", 3*4
        20, //"Tree-05", 2*3
        10, //"RoadFenceWood_Damaged-01", 3*3
        10, //"RoadFenceWood_Lrg-01", 2*4
        10, //"RoadFenceWood_Sml-03", 2*1
        30, //"RockMed-02", 2*1
        25,//"ShrubTall-01", 2*2
        30, //"ShrubTall-02", 1*1
        30, //"TreeLog_B-02", 2*1
        30, //"TreeLog_B-03", 2*2
        15, //"TreeStump_A-02", 2*2
        15, //"TreeStump_B-03", 2*2
        12, //"RoadSign-01", 2*2
        15, //"RoadLamp-01", 1*1
        15, //"PondReed-02", 1*1
        /*
        50, //"RockSml-02", 1*1
        50, //"ShrubShort-02", 1*1
        50, //"Grass-01", 1*1
        50, //"Grass-02", 1*1 */
    };

    string[] small_object_fill = {
        "RockSml-02", //1*1
        //"ShrubShort-02", //1*1
        "Grass-01", //1*1
        "Grass-02", //1*1
    };

    string[] large_rock = {
        //"RockCliff-0203", //30*18
        "RockCliff-03", //26*15
        "RockCliff-03-1", //17*22
        "RockCliff-02", //17*17
        "RockCliff-01" //13*12
    };

    int[] large_rock_num = {
        //1, //"RockCliff-0203", 30*18
        1, //"RockCliff-03", 26*15
        1, //"RockCliff-03-1", 17*22
        1, //"RockCliff-02", 17*17
        1, //"RockCliff-01", 13*12
    };

    string[] large_object = {
        "Oak-02", //12*10
        //"Pond_Boardwalk-02", //8*5
        "TreeBirch-05", //8*8
        "TreeBirch-02", //7*8
        "Tree-05", //10*12
        "Tree-03", //9*11
        "Tree-02", //7*10
        "Tree-04", //7*10
        "Tree-01", //7*9
        "TreeBirch-01", //4*4
        "TreeDead-01", //5*6
        "TreeDead-02", //4*3
        "Birch-03", //8*8
    };

    int[] large_object_num = {
        5, //"Oak-02", 12*10
        //2, //"Pond_Boardwalk-02", 8*5
        5, //"TreeBirch-05", 8*8
        5, //"TreeBirch-02", 7*8
        5, //"Tree-05", 10*12
        5, //"Tree-03", 9*11
        5, //"Tree-02", 7*10
        5, //"Tree-04", 7*10
        5, //"Tree-01", 7*9
        5, //"TreeBirch-01", 4*4
        5, //"TreeDead-01", 5*6
        5, //"TreeDead-02", 4*3
        5 //"Birch-03", 8*8
    };

    int[] idleAreaInPattern;

    public void genScene()
    {
        UnityEngine.Random.InitState(randomSeed);

        LoadHexagonObjects();
        Scene.Initialize();

        PlaceStartHexagons();
        PlaceEndHexagons();

        // place checkpoints
        Scene.GetCheckpointPosValue();
        PlaceCheckpointHexagonPatterns(Scene.checkpointPos, 10);
        GenerateCheckpointRandomArea(Scene.checkpointPos, 4, 6);

        idleAreaInPattern = InsertPattern();

        PlaceAllHexagonPatternsInArray(pattern_object, pattern_object_num, "Prefabs/Scene/Hex/", 20);

        // Random Area
        GenerateRandomArea(2, 5, 7, 10, 5);

        ReleaseIdleAreaInPattern(idleAreaInPattern);

        ConnectAllHexagons();
        
        PlaceAllSceneInArray(large_rock, large_rock_num, "Prefabs/Scene/Large/", 20);
        PlaceAllSceneInArray(large_object, large_object_num, "Prefabs/Scene/Large/", 10);
        PlaceAllSceneInArray(small_object, small_object_num, "Prefabs/Scene/Small/", 10);
        
        
        /*for(int i=0; i<50; i++)
            for(int j=0; j<50; j++)
            {
                PlaceHexagon(i, j, sand);
            }*/
        FillRestArea(small_object_fill, "Prefabs/Scene/Small/", 3);

        terrain = GameObject.Find("/Env/Terrain");
        terrain.GetComponent<TerrainGenerate>().Generate();
    }

    void ReleaseIdleAreaInPattern(int[] idleArea)
    {
        for (int i=0; i<idleArea.Length; i++)
        {
            Debug.Assert(Scene.hexOccupied[idleArea[i]], "Idle area not occupied in ReleaseIdleAreaInPattern");
            Scene.hexOccupied[idleArea[i]] = false;
        }
    }

    int[] InsertPattern()
    {
        Pattern.Initialize();
        int sample_count=0, start_x, start_z, num_local, num_global;
        int total_count = 20;
        Scene.hexNum bias;
        int[] idleArea = new int[0];

        while(true) {
            sample_count += 1;
            if(sample_count >= total_count) break;

            start_x = UnityEngine.Random.Range(2, Scene.areaHeight-Pattern.height-2);
            start_z = UnityEngine.Random.Range(2, Scene.areaWidth-Pattern.width-2);
            if (Pattern.start_even && start_z%2==1) start_z += 1;

            if (!Scene.JudgeWidthOccupancy(start_z-1, Pattern.width+1)) continue;

            if (Scene.JudgeRectAreaOccupancy(start_x, start_z-1, Pattern.height, Pattern.width+1))
            {
                //Debug.Log("area value: "+Pattern.area[Pattern.GetOneDimensionVal(0, 1)]);
                for (int i=0; i<Pattern.height; i++)
                    for (int j=0; j<Pattern.width; j++)
                    {
                        num_local = Pattern.GetOneDimensionVal(i, j);
                        if (Pattern.area[num_local] >= 0)
                        {
                            PlaceHexagon(start_x+i, start_z+j, hex[Pattern.area[num_local]], add_angle:true, follow:true);
                        }
                        else{
                            num_global = Scene.GetOneDimensionVal(start_x+i, start_z+j);
                            Utils.Add(ref idleArea, num_global);
                        }
                    }

                Scene.SetRectAreaOccupancy(start_x, start_z, Pattern.height, Pattern.width, sceneOccupancy: false);
                bias = new Scene.hexNum {num_x = start_x, num_z = start_z};
                Scene.AddHexagonCenter(Pattern.start+bias, Pattern.end+bias, same: false);
                Scene.SetWidthOccupancy(start_z-1, Pattern.width+1);

                break;
            }
        }
        if (sample_count == total_count) Debug.Log("Pattern placement not successful");
        return idleArea;
    }

    void LoadHexagonObjects()
    {
        string dir = "Prefabs/Hex/";
        hex = new GameObject[hex_name.Length];
        for (int i=0; i<hex_name.Length; i++)
        {
            hex[i] = (GameObject)Resources.Load(dir+hex_name[i]);
        }

        finalCheckpoint = (GameObject)Resources.Load("Prefabs/finalCheckpoint");
        coin = (GameObject)Resources.Load("Prefabs/coin");
        diamond = (GameObject)Resources.Load("Prefabs/diamond");
        AICharacter = (GameObject)Resources.Load("Prefabs/AI");
        Portal = (GameObject)Resources.Load("Prefabs/Portal");
    }

    void PlaceStartHexagons()
    {
        GenerateRandomArea(3, 6, 5, 8, 1, use_xz:true, static_x:0, static_z:0);
    }

    void PlaceEndHexagons()
    {
        PlaceHexagonPattern(3, 3, finalCheckpoint, 100, 1, 3, 5, 3, true, Scene.areaHeight-8, Scene.areaWidth-8);
    }

    void FillRestArea(string[] name, string dir, int ratio)
    {
        GameObject[] objects = new GameObject[name.Length];

        for(int i=0; i<name.Length; i++)
            objects[i] = (GameObject)Resources.Load(dir+name[i]);

        for(int i=0; i<Scene.areaHeight; i++)
            for(int j=0; j<Scene.areaWidth; j++)
            {
                int val = UnityEngine.Random.Range(0, name.Length*ratio);
                if (val < name.Length)
                    PlaceHexagon(i, j, objects[val], place_coin: false, show_minimap: false, isHex: false);
            }
    }

    void GenerateRandomArea(int lower_bound_height, int upper_bound_height, int lower_bound_width, int upper_bound_width, int times, bool use_xz=false, int static_x=-1, int static_z=-1, bool considerOccupancy=true)
    {
        int sample_count, x, z, height, width;
        for (int i=0; i<times; i++)
        {
            height = UnityEngine.Random.Range(lower_bound_height, upper_bound_height+1);
            width = UnityEngine.Random.Range(lower_bound_width, upper_bound_width+1);
            sample_count = 0;
            while(true) {
                sample_count += 1;
                if(sample_count >= 10) break;
                if (use_xz && static_x != -1 && static_z != -1) {
                    x = static_x;
                    z = static_z;
                }
                else {
                    x = UnityEngine.Random.Range(0, Scene.areaHeight-height);
                    z = UnityEngine.Random.Range(0, Scene.areaWidth-width);
                }

                if (considerOccupancy && !Scene.JudgeWidthOccupancy(z, width+1)) continue;

                Scene.hexNum hexN = new Scene.hexNum {num_x=x, num_z=z};
                Scene.hexNum[] area = Hexagon.GetRandomArea(hexN, height, width);
                Scene.hexNum[] area_clean = area.ToList().GetRange(0, area.Length-2).ToArray();

                if(Scene.JudgeAreaOccupancy(area_clean) || !considerOccupancy)
                {
                    //Scene.SetAreaOccupancy(area); -> Occupancy set in PlaceHexagon
                    PlaceHexagon(area_clean, hex[0], add_angle:true, follow:true); //grass
                    if (!considerOccupancy) return;

                    //Scene.hexNum center = Scene.HalfLerp(area[area.Length-2], area[area.Length-1]);
                    //Scene.AddHexagonCenter(center);
                    Scene.AddHexagonCenter(area[area.Length-2], area[area.Length-1], same: false);
                    //Debug.Log("area[area.Length-2]: "+area[area.Length-2].num_x+" "+area[area.Length-2].num_z);
                    //Debug.Log("area[area.Length-1]: "+area[area.Length-1].num_x+" "+area[area.Length-1].num_z);
                    Scene.SetWidthOccupancy(z, width+1);

                    InsertSpeedDownModule(area_clean);
                    InsertBombModule(area_clean);
                    break;
                }
            }
        }
    }

    void GenerateCheckpointRandomArea(Scene.hexNum[] pos, int lower_bound_width, int upper_bound_width)
    {
        int width;
        Scene.hexNum hexN;
        GameObject obj = (GameObject)Resources.Load("Prefabs/Hex/checkpoint");
        GameObject obj_new;

        for (int i=1; i<pos.Length; i+=2)
        {
            while(true) {
                width = UnityEngine.Random.Range(lower_bound_width, upper_bound_width+1);
                if (!Scene.JudgeWidthOccupancy(pos[i].num_z-width-1, width*2+1)) continue;

                hexN = new Scene.hexNum {num_x=UnityEngine.Random.Range(5, Scene.areaHeight-5), num_z=pos[i].num_z-width};
                Scene.hexNum[] area = Hexagon.GetRandomArea(hexN, 5, width*2);
                Scene.hexNum[] area_clean = area.ToList().GetRange(0, area.Length-2).ToArray();
                if(Scene.JudgeAreaOccupancy(area_clean))
                {
                    Scene.AddHexagonCenter(area[area.Length-2], area[area.Length-1], same:false);
                    Scene.hexNum center = Scene.HalfLerp(area[area.Length-2], area[area.Length-1]);

                    obj_new = PlaceSceneObject(center.num_x, center.num_z, 1, 1, obj);
                    obj_new.GetComponent<Checkpoint>().num = i;
                    obj_new.GetComponent<Checkpoint>().total = pos.Length;

                    PlaceHexagon(area_clean, hex[0], add_angle:true, follow:true);
                    Scene.SetWidthOccupancy(pos[i].num_z-width-1, width*2+1);
                    break;
                }
            }
        }
    }

    /*void GenerateCheckpointRandomArea(Scene.hexNum[] pos, int lower_bound_width, int upper_bound_width)
    {
        int width;
        Scene.hexNum hexN;
        GameObject obj = (GameObject)Resources.Load("Prefabs/Hex/checkpoint");
        for (int i=0; i<pos.Length; i++)
        {
            while(true) {
                width = UnityEngine.Random.Range(lower_bound_width, upper_bound_width+1);
                hexN = new Scene.hexNum {num_x=pos[i].num_x-1, num_z=pos[i].num_z+1};
                Scene.hexNum[] area = Hexagon.GetRandomArea(hexN, 3, width);
                Scene.hexNum[] area_clean = area.ToList().GetRange(0, area.Length-2).ToArray();
                if(Scene.JudgeAreaOccupancy(area_clean))
                {
                    PlaceHexagon(pos[i].num_x, pos[i].num_z, obj, place_coin:false, add_angle: true, follow:true);
                    PlaceHexagon(area_clean, hex[0], add_angle:true, follow:true);
                    Scene.SetWidthOccupancy(pos[i].num_z, width+1);
                    Scene.AddHexagonCenter(pos[i]);
                    Scene.AddHexagonCenter(area[area.Length-1]);
                    break;
                }
            }
        }
    }*/

    void ConnectAllHexagons()
    {
        Scene.SortHexagonCenter();

        Scene.hexNum[] line;
        Scene.hexNum[][] startOfLine;
        Scene.hexNum start, end;
        Scene.hexAxis startAxis, endAxis;
        int angle;

        //bool searchPortalStart = true, searchPortalEnd = false;
        //GameObject doorIn=null, doorOut=null;
        //Portal doorInScript=null, doorOutScript=null;
        int pairedDoorNum = 3;
        Portal[] doorInScripts = new Portal[pairedDoorNum];
        Portal[] doorOutScripts = new Portal[pairedDoorNum];
        int doorInCount = 0, doorOutCount = 0;
        bool haveDoor = false;

        //Get the first element in hexCenter
        /*start = new Scene.hexNum{
            num_x = 0,
            num_z = 0
        };
        end = Scene.GetNextHexagonCenter(0, -1);

        startAxis = Hexagon.NumToAxis(start.num_x, start.num_z);
        endAxis = Hexagon.NumToAxis(end.num_x, end.num_z);
        angle = Hexagon.NearestAngleForCamera(startAxis, endAxis);

        Debug.Log("start_x: "+start.num_x);
        Debug.Log("start_z: "+start.num_z);
        Debug.Log("end_x: "+end.num_x);
        Debug.Log("end_z: "+end.num_z);
        line = Hexagon.GetLineBetweenPointsOffset(start, end);
        Utils.Add(ref line, start);
        PlaceHexagon(line, hex[1], add_angle:true, angle:angle); //sand

        line = Hexagon.GetNoisyEdgesBetweenPointsOffset(start, end, 1, 0.4f);
        PlaceHexagon(line, hex[2], add_angle:true, angle:angle); //stone
        
        line = Hexagon.GetTwistedEdgesBetweenPointsOffset(start, end, 0.6f);
        PlaceHexagon(line, hex[3], add_angle:true, angle:angle); //water */

        int total = Scene.longArea? 1:9;

        for(int k=0; k<total; k++)
        {
            for (int i=0; i<Scene.hexCenter[k].Length;i++)
            {
                start = Scene.hexCenter[k][i].end;
                end = Scene.GetNextHexagonCenter(k, i).start;

                startAxis = Hexagon.NumToAxis(start.num_x, start.num_z);
                endAxis = Hexagon.NumToAxis(end.num_x, end.num_z);
                angle = Hexagon.NearestAngleForCamera(startAxis, endAxis);

                if (end.num_x == -1) break; // reach end of center -> change to goal later

                int distance = Hexagon.DistanceBetweenPointsOffset(start, end);
                startOfLine = ConnectAllLines(start, end, distance, angle);
                
                if (startOfLine[0]!=null && startOfLine[0].Length!=0)
                {
                    int j = 0;
                    while(j < startOfLine[0].Length)
                    {
                        bool placeSuccess = PlaceRoadSign(startOfLine[0][j++], angle);
                        //Debug.Assert(placeSuccess, "Not successful when placing road sign: "+i);
                        if (placeSuccess) break;
                    }

                    if (startOfLine[1]!= null)
                        Utils.Add(ref startOfLine, startOfLine[1]);
                    if (startOfLine[2]!= null)
                        Utils.Add(ref startOfLine, startOfLine[2]);
                    
                    InsertSpeedDownModule(startOfLine[0]);
                    InsertBombModule(startOfLine[0]);
                    /*if (i%2 == 1) // Insert SpeedUp Module
                        InsertSpeedUpJumpModule(startOfLine, distance, angle, SpeedUp: true);
                    else    // Insert Jump Module
                        InsertSpeedUpJumpModule(startOfLine, distance, angle, SpeedUp: false);*/
                    if (i%2 == 1)   //Insert Jump Module
                        InsertSpeedUpJumpModule(startOfLine[0], distance, angle, SpeedUp: false);
                    InsertSpeedUpJumpModule(startOfLine[0], distance, angle, SpeedUp: true);
                }

                // Build subpaths and place door
                if (!haveDoor && doorInCount < pairedDoorNum && doorInCount == doorOutCount)
                {
                    if (UnityEngine.Random.Range(0, 1f)<0.7f)
                    {
                        doorInScripts[doorInCount] = PlaceDoorIn(angle, start, distance);
                        if (doorInScripts[doorInCount] != null)
                        {
                            //searchPortalStart = false;
                            //searchPortalEnd = true;
                            //Debug.Log("Place In");
                            Debug.Log("place In at pattern i: "+i);
                            doorInCount ++;
                            haveDoor = true;
                            continue;
                        }
                    }
                }

                if (!haveDoor && doorOutCount < pairedDoorNum && doorOutCount == (doorInCount-1))
                {
                    if (UnityEngine.Random.Range(0, 1f)<0.7f)
                    {
                        doorOutScripts[doorOutCount] = PlaceDoorOut(angle, end, distance);
                        if (doorOutScripts[doorOutCount] != null)
                        {
                            //searchPortalEnd = false;
                            //Debug.Log("Place Out");
                            Debug.Log("place Out at pattern i: "+i);
                            doorOutCount ++;
                            haveDoor = true;
                            continue;
                        }
                    }
                }

                if (haveDoor) haveDoor = false;
            }
        }

        //Debug.Assert(doorInCount==doorOutCount, "In & Out doors number don't match");
        int[] numArray = Utils.GetRandomNum(doorOutCount);
        for (int k=0; k<doorOutCount; k++)
            SetDoorPair(doorInScripts[k], doorOutScripts[numArray[k]]);
        if (doorOutCount == doorInCount-1)
            // fill the last doorIn
            doorInScripts[doorInCount-1] = doorOutScripts[UnityEngine.Random.Range(0, doorOutCount)];
    }

    Portal PlaceDoorIn(int angle, Scene.hexNum start, int distance)
    {
        int PortalAngle = UnityEngine.Random.Range(30, 90);
        int subAngle = PortalAngle>60?90:30;
        int maxLength, length, num_x, num_z;
        float angleTmp = 2f * Mathf.PI * PortalAngle / 360;
        Scene.hexNum PortalPos;
        Scene.hexNum[][] startLine;
        Scene.hexAxis startAxis, endAxis;
        GameObject doorIn;
        Portal doorInScript;
        //if (angle>0) PortalAngle = -PortalAngle;
        
        if (angle < 0)
        {
            // protalAngle > 0
            maxLength = (int)Mathf.Min(
                (Scene.areaHeight-start.num_x)/Mathf.Sin(angleTmp),
                (Scene.areaWidth-start.num_z)/Mathf.Cos(angleTmp)
                );
            maxLength = Mathf.Min(maxLength, distance+distance/2);
            length = UnityEngine.Random.Range(distance, distance+distance/2);
            num_x = start.num_x + (int)(length*Mathf.Sin(angleTmp));
            num_z = start.num_z + (int)(length*Mathf.Cos(angleTmp));
        }
        else {
            maxLength = (int)Mathf.Min(
                start.num_x/Mathf.Sin(angleTmp),
                (Scene.areaWidth-start.num_z)/Mathf.Cos(angleTmp)
                );
            maxLength = Mathf.Min(maxLength, distance+distance/2);
            length = UnityEngine.Random.Range(distance, distance+distance/2);
            num_x = start.num_x - (int)(length*Mathf.Sin(angleTmp));
            num_z = start.num_z + (int)(length*Mathf.Cos(angleTmp));
            PortalAngle = -PortalAngle;
            subAngle = -subAngle;
        }
        
        if (!Scene.JudgeRectAreaOccupancy(num_x-1, num_z, 3, 1)) return null;
        doorIn = PlaceSceneObject(num_x, num_z, 1, 1, Portal, angle: PortalAngle);
        doorInScript = doorIn.GetComponent<Portal>();
        doorInScript.inDoor = true;

        PortalPos = new Scene.hexNum {
            num_x = num_x,
            num_z = num_z
        };

        startAxis = Hexagon.NumToAxis(start.num_x, start.num_z);
        endAxis = Hexagon.NumToAxis(PortalPos.num_x, PortalPos.num_z);
        int lineAngle = Hexagon.NearestAngleForCamera(startAxis, endAxis);

        int subDistance = Hexagon.DistanceBetweenPointsOffset(start, PortalPos);
        startLine = ConnectAllLines(start, PortalPos, subDistance, subAngle, subPath:true);
        doorInScript.nearestHex = startLine[0][startLine[0].Length-1];
        try{
            doorInScript.forwardVector = Hexagon.CenterVectorBetweenTwoHex(startLine[0][startLine[0].Length-2], startLine[0][startLine[0].Length-1]).normalized;
        }
        catch (Exception e)
        {
            //Hexagon.CenterVectorBetweenTwoHex(start, startLine[0][startLine[0].Length-1]).normalized;
            doorInScript.forwardVector = new Vector3(Mathf.Sin(2*Mathf.PI*lineAngle/360f), 0, Mathf.Cos(2*Mathf.PI*lineAngle/360f));
        }

        if (startLine[0]!=null && startLine[0].Length!=0)
        {
            int j = 0;
            while(j < startLine[0].Length)
            {
                bool placeSuccess = PlaceRoadSign(startLine[0][j++], lineAngle);
                //Debug.Assert(placeSuccess, "Not successful when placing road sign: "+i);
                if (placeSuccess) break;
            }
        }
        return doorInScript;
    }

    Portal PlaceDoorOut(int angle, Scene.hexNum end, int distance)
    {
        int PortalAngle = UnityEngine.Random.Range(30, 90);
        int subAngle = PortalAngle>60?90:30;
        int maxLength, length, num_x, num_z;
        float angleTmp = 2f * Mathf.PI * PortalAngle / 360;
        Scene.hexNum PortalPos;
        Scene.hexNum[][] startLine;
        Scene.hexAxis startAxis, endAxis;
        GameObject doorOut;
        Portal doorOutScript;

        if (angle < 0)
        {
            maxLength = (int)Mathf.Min(
                end.num_x/Mathf.Sin(angleTmp),
                end.num_z/Mathf.Cos(angleTmp)
                );
            maxLength = Mathf.Min(maxLength, distance+distance/2);
            length = UnityEngine.Random.Range(distance, distance+distance/2);
            num_x = end.num_x - (int)(length*Mathf.Sin(angleTmp));
            num_z = end.num_z - (int)(length*Mathf.Cos(angleTmp));
        }
        else {
            maxLength = (int)Mathf.Min(
                (Scene.areaHeight-end.num_x)/Mathf.Sin(angleTmp),
                end.num_z/Mathf.Cos(angleTmp)
                );
            maxLength = Mathf.Min(maxLength, distance+distance/2);
            length = UnityEngine.Random.Range(distance, distance+distance/2);
            num_x = end.num_x + (int)(length*Mathf.Sin(angleTmp));
            num_z = end.num_z - (int)(length*Mathf.Cos(angleTmp));
            subAngle = -subAngle;
        }

        if (!Scene.JudgeRectAreaOccupancy(num_x-1, num_z, 3, 1)) return null;
        doorOut = PlaceSceneObject(num_x, num_z, 1, 1, Portal, angle: subAngle);
        doorOutScript = doorOut.GetComponent<Portal>();
        doorOutScript.inDoor = false;
        //doorOutScript.pairedDoor = doorInScript;
        //doorInScript.pairedDoor = doorOutScript;

        PortalPos = new Scene.hexNum {
            num_x = num_x,
            num_z = num_z
        };

        startAxis = Hexagon.NumToAxis(PortalPos.num_x, PortalPos.num_z);
        endAxis = Hexagon.NumToAxis(end.num_x, end.num_z);
        int lineAngle = Hexagon.NearestAngleForCamera(startAxis, endAxis);

        int subDistance = Hexagon.DistanceBetweenPointsOffset(PortalPos, end);
        startLine = ConnectAllLines(PortalPos, end, subDistance, subAngle, subPath:true);
        doorOutScript.nearestHex = startLine[0][0];
        try {
            doorOutScript.forwardVector = Hexagon.CenterVectorBetweenTwoHex(startLine[0][0], startLine[0][1]).normalized;
        }
        catch (Exception e)
        {
            //Hexagon.CenterVectorBetweenTwoHex(startLine[0][0], end);
            doorOutScript.forwardVector = new Vector3(Mathf.Sin(2*Mathf.PI*lineAngle/360f), 0, Mathf.Cos(2*Mathf.PI*lineAngle/360f));
        }
        //ReplaceObject(startLine[0][1], hex[5]);

        // fill in remaining area
        //int x_val=-1, z_val=-1;
        Scene.hexNum hexN = startLine[0][1];
        int dirAngle = Utils.getVectorAngle(Vector3.forward, doorOutScript.forwardVector);

        for (int t=2; t<=5; t++)
        {
            hexN = Hexagon.GetHexByAngle(hexN, dirAngle);
            //x_val = startLine[0][1].num_x*t - startLine[0][0].num_x*(t-1);
            //z_val = startLine[0][1].num_z*t - startLine[0][0].num_z*(t-1);
            //Utils.DebugDrawPoint(hexN, 0, Color.red);
            PlaceHexagon(hexN.num_x, hexN.num_z, hex[0], place_coin:true);
        }

        if (lineAngle>0)
            GenerateRandomArea(5, 5, 3, 4, 1, use_xz:true, static_x: hexN.num_x, static_z:hexN.num_z, considerOccupancy:false);
        else
            GenerateRandomArea(5, 5, 3, 4, 1, use_xz:true, static_x: hexN.num_x-5, static_z:hexN.num_z, considerOccupancy:false);
        return doorOutScript;
    }

    void SetDoorPair(Portal doorInScript, Portal doorOutScript)
    {
        doorOutScript.pairedDoor = doorInScript;
        doorInScript.pairedDoor = doorOutScript;
    }

    Scene.hexNum[][] ConnectAllLines(Scene.hexNum start, Scene.hexNum end, int distance, int angle, bool subPath=false)
    {
        Scene.hexNum[] line;
        Scene.hexNum[][] startOfLine = new Scene.hexNum[3][];
        for (int i=0; i<startOfLine.Length; i++)
            startOfLine[i] = null;
        line = Hexagon.GetLineBetweenPointsOffset(start, end);
        if (line!=null)
            startOfLine[0] = PlaceHexagon(line, hex[1], add_angle:true, angle:angle); //sand

        if (subPath) return startOfLine;
        
        int level = 1;
        if (distance >= 16) level = 3;
        if (distance >= 8) level = 2;
        else if (distance >= 4) level = 1;

        line = Hexagon.GetNoisyEdgesBetweenPointsOffset(start, end, level, 0.4f);
        if (line!=null)
            startOfLine[1] = PlaceHexagon(line, hex[2], add_angle:true, angle:angle); //stone
        
        line = Hexagon.GetTwistedEdgesBetweenPointsOffset(start, end, 0.6f);
        if (line!=null)
            startOfLine[2] = PlaceHexagon(line, hex[3], add_angle:true, angle:angle); //water

        return startOfLine;
    }

    void InsertSpeedUpJumpModule(Scene.hexNum[] line, int distance, int angle, bool SpeedUp)
    {
        int sample_count, order, choose, num;
        int angle_reverse;
        int len = line.Length;
        int countUp, countDown;
        Scene.hexNum tmp, tmpStart;
        string objectName = (SpeedUp?"speedUp":"jump")+"(Clone)";

        for (int i=1; i<=distance/8; i++)
        {
            sample_count = 0;
            while(true) {
                sample_count += 1;
                if(sample_count >= 8) break;

                countUp = 0;
                countDown = 0;

                order = UnityEngine.Random.Range(0, len);
                tmp = line[order];

                //Utils.DebugDrawPoint(tmp, angle, Color.blue);

                while (true)
                {
                    tmp = Hexagon.GetHexByAngle(tmp, angle);
                    if (tmp.num_x==-1 || tmp.num_z==-1) break;
                    num = Scene.GetOneDimensionVal(tmp.num_x, tmp.num_z);
                    if (!Scene.hexOccupied[num]) break;
                    if (Scene.map[num].name == objectName) break;
                    if (Scene.map[num].name == "bomb(Clone)") break;
                    if (Scene.map[num].GetComponent<HexagonParam>().scanned == true) break;

                    //Utils.DebugDrawPoint(tmp, angle, Color.red);
                    Scene.map[num].GetComponent<HexagonParam>().scanned = true;
                    countUp++;
                }

                tmp = line[order];
                tmpStart = tmp;
                while (true)
                {
                    angle_reverse = -(180-angle);
                    if (angle_reverse < -150) angle_reverse += 360;
                    tmp = Hexagon.GetHexByAngle(tmp, angle_reverse);
                    if (tmp.num_x==-1 || tmp.num_z==-1) break;
                    num = Scene.GetOneDimensionVal(tmp.num_x, tmp.num_z);
                    if (!Scene.hexOccupied[num]) break;
                    if (Scene.map[num].name == objectName) break;
                    if (Scene.map[num].name == "bomb(Clone)") break;
                    if (Scene.map[num].GetComponent<HexagonParam>().scanned == true) break;
                    
                    tmpStart = tmp;

                    //Utils.DebugDrawPoint(tmp, angle, Color.green);
                    Scene.map[num].GetComponent<HexagonParam>().scanned = true;
                    countDown++;
                }

                //tmpStart = Hexagon.GetHexByAngle(tmp, angle);
                //Utils.DebugDrawPoint(tmpStart, angle, Color.yellow);

                int totalLen = countUp+countDown+1;
                if (totalLen>=9)
                {
                    choose = UnityEngine.Random.Range(2, totalLen-6);
                    //Debug.Log("choose: "+choose);
                    for (int j=1; j<=choose; j++)
                        tmpStart = Hexagon.GetHexByAngle(tmpStart, angle);

                    if (SpeedUp)
                    {
                        ReplaceObject(tmpStart, hex[4]);

                        for (int j=1; j<=5; j++)
                        {
                            tmpStart = Hexagon.GetHexByAngle(tmpStart, angle);
                            num = Scene.GetOneDimensionVal(tmpStart.num_x, tmpStart.num_z);
                            //Debug.Log("tmpStart: "+tmpStart.num_x+" "+tmpStart.num_z);
                            if (Scene.map[num].name == "speedDown(Clone)")
                            {
                                ReplaceObject(tmpStart, hex[1]);
                            }
                        }
                        break;
                    }
                    else {
                        ReplaceObject(tmpStart, hex[6]);
                        for (int j=1; j<=2; j++)
                        {
                            tmpStart = Hexagon.GetHexByAngle(tmpStart, angle);
                            num = Scene.GetOneDimensionVal(tmpStart.num_x, tmpStart.num_z);
                            if (UnityEngine.Random.Range(0, 1f) < 0.5)
                                Destroy(Scene.map[num]);
                        }
                    }
                }
            }
        }
    }

    void InsertSpeedDownModule(Scene.hexNum[] line)
    {
        int sample_count, order, num;
        bool pair;
        Scene.hexNum tmp;
        bool[] result = new bool[8];
        Scene.hexNum[] tmpNeighbour;
        for (int i=1; i<=line.Length/15; i++)
        {
            sample_count = 0;
            pair = true;
            while(true) {
                sample_count += 1;
                if(sample_count >= 5) break;

                order = UnityEngine.Random.Range(0, line.Length);
                tmp = line[order];
                if (!GetHexExistence(tmp))  continue;

                tmpNeighbour = Hexagon.GetNeighbourHex(tmp);
                Utils.Add(ref tmpNeighbour, Hexagon.GetLeftUpHex(tmpNeighbour[0]));
                Utils.Add(ref tmpNeighbour, Hexagon.GetRightUpHex(tmpNeighbour[1]));

                for (int j=0; j<tmpNeighbour.Length; j++)
                {
                    result[j] = GetHexExistence(tmpNeighbour[j]);
                }
                
                pair &= (result[0]&&result[3]&&result[6]) || (!result[3]);
                pair &= (result[1]&&result[4]&&result[7]) || (!result[4]);
                //pair &= (result[2]&&result[5]) || (!result[2]&&!result[5]);

                if (pair && (result[3]||result[4]))
                {
                    ReplaceObject(tmp, hex[5]);
                    for (int j=0; j<tmpNeighbour.Length; j++)
                    {
                        if (result[j])
                        {
                            num = Scene.GetOneDimensionVal(tmpNeighbour[j].num_x, tmpNeighbour[j].num_z);
                            Scene.map[num].GetComponent<HexagonParam>().scanned = true;
                        }
                    }

                    break;
                }
            }
        }
    }

    void InsertBombModule(Scene.hexNum[] line)
    {
        int sample_count, order, num;
        bool[] result = new bool[6];
        Scene.hexNum tmp;
        Scene.hexNum[] tmpNeighbour;
        for (int i=1; i<=line.Length/35; i++)
        {
            sample_count = 0;
            while(true) {
                sample_count += 1;
                if(sample_count >= 5) break;

                order = UnityEngine.Random.Range(0, line.Length);
                tmp = line[order];
                if (!GetHexExistence(tmp))  continue;

                tmpNeighbour = Hexagon.GetNeighbourHex(tmp);
                for (int j=0; j<tmpNeighbour.Length; j++)
                {
                    result[j] = GetHexExistence(tmpNeighbour[j]);
                }

                if ((result[0]&&result[4]&&result[5])||(result[1]&&result[2]&&result[3]))
                {
                    ReplaceObject(tmp, hex[9]);
                    for (int j=0; j<tmpNeighbour.Length; j++)
                    {
                        if (result[j])
                        {
                            num = Scene.GetOneDimensionVal(tmpNeighbour[j].num_x, tmpNeighbour[j].num_z);
                            Scene.map[num].GetComponent<HexagonParam>().scanned = true;
                        }
                    }

                    break;
                }
            }
        }
    }

    bool GetHexExistence(Scene.hexNum hexN)
    {
        if (hexN.num_x == -1 || hexN.num_z == -1)  return false;
        int num = Scene.GetOneDimensionVal(hexN.num_x, hexN.num_z);
        if (!Scene.hexOccupied[num]) return false;
        try {
            if (Scene.map[num].name == "speedDown(Clone)") return false;
        }
        catch (Exception e)
        {
            Debug.Log("hexN.num_x: "+hexN.num_x);
            Debug.Log("hexN.num_z: "+hexN.num_z);
            return false;
        }
        return true;
    }

    void ReplaceObject(Scene.hexNum hexN, GameObject obj)
    {
        int num = Scene.GetOneDimensionVal(hexN.num_x, hexN.num_z);
        int angle = Scene.map[num].GetComponent<HexagonParam>().angle;
        bool follow = Scene.map[num].GetComponent<HexagonParam>().follow;
        Destroy(Scene.map[num]);

        if (Scene.coinOccupied[num])
            Destroy(Scene.coin[num]);

        Vector3 pos = GetPosFromHexNum(hexN.num_x, hexN.num_z);
        Scene.map[num] = Instantiate(obj, pos, Quaternion.identity, transform);
        Scene.map[num].layer = 9;
        Scene.map[num].GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        Scene.map[num].AddComponent<HexagonParam>();
        Scene.map[num].GetComponent<HexagonParam>().angle = angle;
        Scene.map[num].GetComponent<HexagonParam>().follow = follow;
        Scene.map[num].GetComponent<HexagonParam>().scanned = true;
    }

    Vector3 GetPosFromHexNum(int x, int z)
    {
        Scene.hexAxis hexA;
        Vector3 pos;
        hexA = Hexagon.NumToAxis(x, z);
        pos.x = hexA.axis_x;
        pos.z = hexA.axis_z;
        pos.y = 0;
        return pos;
    }

    bool PlaceRoadSign(Scene.hexNum start, int angle)
    {
        GameObject obj = (GameObject)Resources.Load("Prefabs/sign");
        Dictionary<int, int[]> dict = new Dictionary<int, int[]>() {
            {30, new int[]{3, 4, 5, 0, 1, 2}},
            {90, new int[]{4, 5, 0, 1, 2, 3}},
            {150, new int[]{5, 0, 1, 2, 3, 4}},
            {-150, new int[]{0, 1, 2, 3, 4, 5}},
            {-90, new int[]{1, 2, 3, 4, 5, 0}},
            {-30, new int[]{2, 3, 4, 5, 0, 1}},
        };
        Scene.hexNum[] neighbour = Hexagon.GetNeighbourHex(start);
        int[] order = dict[angle];

        int num;
        Vector3 pos;
        Scene.hexNum hexN;
        Scene.hexAxis hexA;

        for (int i=0; i<=5; i++)
        {
            hexN = neighbour[order[i]];
            if (hexN.num_x==-1 && hexN.num_z==-1) continue;
            num = Scene.GetOneDimensionVal(hexN.num_x, hexN.num_z);
            if (!Scene.hexOccupied[num] && !Scene.sceneOccupied[num])
            {
                pos = GetPosFromHexNum(hexN.num_x, hexN.num_z);
                Scene.map[num] = Instantiate(obj, pos, Quaternion.Euler(0, 90+angle, 0), transform);
                Scene.sceneOccupied[num] = true;
                return true;
            }
        }
        return false;
    }

    Scene.hexNum[] PlaceHexagon(Scene.hexNum[] array, GameObject obj, bool add_angle=false, int angle=0, bool follow=false)
    {
        bool placeResult;
        Scene.hexNum[] val = new Scene.hexNum[0];
        for (int j=0; j<array.Length; j++)
        {
            placeResult = PlaceHexagon(array[j].num_x, array[j].num_z, obj, add_angle:add_angle, angle:angle, follow:follow);
            if (placeResult)
            {
                Utils.Add(ref val, array[j]);
            }
        }
        return val; // the first placed hexagon in the array
    }

    bool PlaceHexagon(int x, int z, GameObject obj, bool place_coin=true, bool add_angle=false, int angle=0, bool follow=false, bool show_minimap=true, bool isHex=true)
    {
        int num;
        Scene.hexAxis hexA;
        Vector3 pos;

        num = Scene.GetOneDimensionVal(x, z);
        if (!Scene.hexOccupied[num] && !Scene.sceneOccupied[num])
        {
            pos = GetPosFromHexNum(x, z);

            if (pos.x==0&&pos.z==0)
            {
                obj = hex[8];
            }

            /*else if (place_coin && UnityEngine.Random.Range(0f, 1f) < 0.05 && !(pos.x==0&&pos.z==0))
            {
                //obj = hex[4+UnityEngine.Random.Range(0, 3)];
                //obj = hex[5];
                obj = hex[9];
            }*/

            Scene.map[num] = Instantiate(obj, pos, Quaternion.identity, transform);
            if (show_minimap)
            {
                Scene.map[num].layer = 9;
                Scene.map[num].GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            }

            if (isHex)
                Scene.hexOccupied[num] = true;
            else Scene.sceneOccupied[num] = true;

            if (add_angle)
            {
                Scene.map[num].AddComponent<HexagonParam>();
                if (follow) Scene.map[num].GetComponent<HexagonParam>().follow = true;
                else {
                    Scene.map[num].GetComponent<HexagonParam>().follow = false;
                    Scene.map[num].GetComponent<HexagonParam>().angle = angle;
                }
            }

            if (pos.x==0&&pos.z==0)
            {
                Scene.map[num].AddComponent<HexagonParam>();
                Scene.map[num].GetComponent<HexagonParam>().follow = false;
                Scene.map[num].GetComponent<HexagonParam>().angle = 30;
            }

            // place coin
            if (place_coin && !(pos.x==0&&pos.z==0))
                {
                    float randVal = UnityEngine.Random.Range(0f, 1f);
                    if (randVal < 0.1)
                    {
                        pos.y = 0.71f;
                        Scene.coin[num] = Instantiate(coin, pos, Quaternion.identity, transform);
                        Scene.coinOccupied[num] = true;
                    }
                    else if (randVal < 0.11)
                    {
                        pos.y = 1f;
                        Scene.coin[num] = Instantiate(diamond, pos, Quaternion.identity, transform);
                        Scene.coinOccupied[num] = true;
                    }
                }
            
            return true;
        }
        return false;
    }

    GameObject PlaceSceneObject(int x, int z, int height, int width, GameObject obj, int angle=0)
    {
        int num;
        Scene.hexAxis hexA;
        Vector3 pos;

        Scene.SetRectAreaOccupancy(x, z, height, width, sceneOccupancy: true);
        hexA = Scene.GetRectAreaCenter(x, z, height, width);
        pos.x = hexA.axis_x - obj.GetComponent<Space>().bias_x;
        pos.z = hexA.axis_z - obj.GetComponent<Space>().bias_z;
        pos.y = obj.GetComponent<Space>().bias_y;
        num = Scene.GetOneDimensionVal(x, z);
        Scene.map[num] = Instantiate(obj, pos, Quaternion.Euler(0, angle, 0), transform);
        return Scene.map[num];
    }

    void PlaceAllSceneInArray(string[] name, int[] num, string dir, int count)
    {
        GameObject[] objects = new GameObject[name.Length];
        int height, width, sample_count, place_count, x, z;

        int[] order = Utils.GetRandomNum(name.Length);
        for (int i=0; i<name.Length; i++)
        {
            int k = order[i];
            objects[k] = (GameObject)Resources.Load(dir+name[k]);
            height = objects[k].GetComponent<Space>().height;
            width = objects[k].GetComponent<Space>().width;
            sample_count = 0;
            place_count = 0;

            while(true) {
                sample_count += 1;
                if(sample_count >= count) break;

                x = UnityEngine.Random.Range(0, Scene.areaHeight-height);
                z = UnityEngine.Random.Range(0, Scene.areaWidth-width);
                //x = UnityEngine.Random.Range(0, 50-height);
                //z = UnityEngine.Random.Range(0, 50-width);

                if(Scene.JudgeRectAreaOccupancy(x, z, height, width))
                {
                    PlaceSceneObject(x, z, height, width, objects[k]);
                    place_count += 1;
                    if (place_count == num[k]) break;
                }
            }
        }
    }

    void PlaceHexagonPattern(int height, int width, GameObject obj, int count, int place_num, int lower_bound, int upper_bound, int edge_distance, bool use_xz=false, int static_x=-1, int static_z=-1, bool checkpoint=false, int num=-1, int total=-1, bool place_AI=false)
    {
        GameObject obj_new;
        // use_xz = true: for placing checkpoint
        int sample_count = 0, place_count = 0;
        int x, z, x1, z1;
        while(true) {
            sample_count += 1;
            if(sample_count >= count) break;

            int radius = Mathf.FloorToInt(Mathf.Max(height/2f, width/2f))+1+UnityEngine.Random.Range(lower_bound, upper_bound+1); //3 -> function(width/height)

            if (use_xz && static_x != -1 && static_z != -1) {
                x = static_x;
                z = static_z;
            }
            else {
                x = UnityEngine.Random.Range(radius+2, Scene.areaHeight-radius-2);
                z = UnityEngine.Random.Range(radius+2, Scene.areaWidth-radius-2);
                //x = UnityEngine.Random.Range(0, 50-height);
                //z = UnityEngine.Random.Range(0, 50-width);
            }

            if (!Scene.JudgeWidthOccupancy(z-(radius+1), 2*(radius+1)+1)) continue;

            Scene.hexNum hexN = new Scene.hexNum {num_x=x, num_z=z};
            Scene.hexNum[] ring = Hexagon.GetSpiralRing(hexN, radius);
            if (ring == null) continue;
            Scene.hexNum[] ring_all = ring;
            while(true)
            {
                Scene.hexNum[] ring_external = Hexagon.GetRing(hexN, radius+1);
                if (ring_external == null) break;
                Scene.hexNum[] ring_external_2 = Hexagon.GetRing(hexN, radius+2);
                if (ring_external_2 != null)
                    Utils.Add(ref ring_external, ring_external_2);
                Utils.Add(ref ring_all, ring_external);
                break;
            }

            if(!Scene.JudgeAreaOccupancy(ring_all)) continue;
            
            Scene.AddHexagonCenter(hexN, hexN, same:true);

            Scene.SetWidthOccupancy(z-(radius+1), 2*(radius+1)+1);

            int min_x = ring[6*radius-7].num_x;
            int max_x = ring[3*radius-4].num_x;
            int min_z = ring[radius-2].num_z;
            int max_z = ring[4*radius-5].num_z;

            while(true) {
                x1 = UnityEngine.Random.Range(min_x+edge_distance, max_x-height-edge_distance+1);
                z1 = UnityEngine.Random.Range(min_z+edge_distance, max_z-width-edge_distance+1);
                if (Scene.JudgeRectInRing(x1, z1, height, width, ring))
                {
                    //if (use_xz)
                        //PlaceHexagon(x1, z1, obj);
                    //else
                    obj_new = PlaceSceneObject(x1, z1, height, width, obj);
                    if (checkpoint && num!=-1 && total!=-1)
                    {
                        obj_new.GetComponent<Checkpoint>().num = num;
                        obj_new.GetComponent<Checkpoint>().total = total;
                    }
                    PlaceHexagon(ring, hex[0], add_angle:true, follow:true); //grass
                    InsertSpeedDownModule(ring);
                    InsertBombModule(ring);

                    if (place_AI)
                    {
                        while(true)
                        {
                            int randomPos = UnityEngine.Random.Range(0, ring.Length);
                            int tmpNum = Scene.GetOneDimensionVal(ring[randomPos].num_x, ring[randomPos].num_z);
                            if (Scene.hexOccupied[tmpNum] && Scene.map[tmpNum].name == "grass(Clone)")
                            {
                                Vector3 pos = GetPosFromHexNum(ring[randomPos].num_x, ring[randomPos].num_z);
                                pos.y = 0.5f;
                                GameObject AI = Instantiate(AICharacter, pos, Quaternion.Euler(0, 30, 0), transform.parent.GetChild(0));

                                // Find two reborn places
                                AI.GetComponentInChildren<AICharacter>().Reborn1 = pos;

                                int rand = tmpNum + ring.Length/2;
                                rand %= ring.Length;
                                while (true)
                                {
                                    int tmpNum2 = Scene.GetOneDimensionVal(ring[rand].num_x, ring[rand].num_z);
                                    if (Scene.hexOccupied[tmpNum2] && Scene.map[tmpNum2].name == "grass(Clone)")
                                        break;
                                    rand--;
                                    rand %= ring.Length;
                                }
                                pos = GetPosFromHexNum(ring[rand].num_x, ring[rand].num_z);
                                pos.y = 0.5f;
                                AI.GetComponentInChildren<AICharacter>().Reborn2 = pos;
                                break;
                            }
                        }
                    }

                    place_count += 1;
                    break;
                }
            }

            if (place_count == place_num) break;
            
        }
    }

    void PlaceAllHexagonPatternsInArray(string[] name, int[] num, string dir, int count)
    {
        GameObject[] objects = new GameObject[name.Length];
        int height, width;
        for (int i=0; i<name.Length; i++)
        {
            objects[i] = (GameObject)Resources.Load(dir+name[i]);
            height = objects[i].GetComponent<Space>().height;
            width = objects[i].GetComponent<Space>().width;

            //if (i==0)
            PlaceHexagonPattern(height, width, objects[i], count, num[i], 2, 3, 2, place_AI:true);
            //else PlaceHexagonPattern(height, width, objects[i], count, num[i], 2, 3, 2, place_AI:false);
        }
    }

    void PlaceCheckpointHexagonPatterns(Scene.hexNum[] pos, int count)
    {
        GameObject obj = (GameObject)Resources.Load("Prefabs/Hex/checkpoint");
        int height = obj.GetComponent<Space>().height;
        int width = obj.GetComponent<Space>().width;
        for (int i=0; i<pos.Length; i+=2)
            PlaceHexagonPattern(height, width, obj, count, 1, 3, 4, 3, true, pos[i].num_x, pos[i].num_z, true, i, pos.Length);
    }
}
