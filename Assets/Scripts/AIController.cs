using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    GameObject player;
    PlayerCharacter playerCharacter;
    AICharacter character;
    Vector3 turnPosition;
    bool findNextHex, stop;
    Scene.hexNum[] passed;
    int pointer;
    Scene.hexNum[] walkable;
    int walk;
    int pastStepNum;
    bool countSpeedDown;
    float speedChangeTime;
    float speedMultiplier;
    float timeSpeed;
    Collider collider;

    string[] perhebited_hex_name = {
            "speedUp(Clone)",
            "jump(Clone)",
            "checkpoint(Clone)",
            "bomb(Clone)",
            "sand(Clone)",
            "stone(Clone)",
            "water(Clone)",
        };

    void Start()
    {
        character = transform.parent.gameObject. GetComponentInChildren<AICharacter>();
        //Debug.Log("character.pos: "+character.transform.position);
        player = GameObject.FindWithTag("Character");
        playerCharacter = player.GetComponent<PlayerCharacter>();
        //if (player == null)
            //Debug.Log("No player found");
        findNextHex = true;
        pastStepNum = 10;
        passed = new Scene.hexNum[10];
        pointer = 0;
        walkable = new Scene.hexNum[0];
        walk = 0;
        countSpeedDown = false;
        speedChangeTime = 1f;
        speedMultiplier = 1f;
        timeSpeed = 0;
        //collider = character.HexagonCheck();
    }

    void Update()
    {
        if(!character.isAlive || Scene.gameStopped || !playerCharacter.isAlive)
        {
            return;
        }

        stop = Input.GetKeyDown(KeyCode.X);
        if (stop) character.Stop();

        Scene.hexNum hexN, hexNext;
        Scene.hexAxis hexA;
        Vector3 v, distance;
        int angle, num, count;
        if (findNextHex)
        {
            collider = character.HexagonCheck();
            Debug.Assert(collider!=null, "AI not in ground");
            
            hexN = GetHexNumFromPositon(collider.gameObject.transform.position);

            distance = player.transform.position-collider.gameObject.transform.position;
            distance.y = 0;
            if (Mathf.Sqrt(distance.sqrMagnitude) < 4*Scene.xDistanceHex)
            //Ray AIRay = new Ray(character.transform.position, character.transform.forward);
            //RaycastHit AIHit;
            //if (Physics.Raycast(AIRay, out AIHit) && AIHit.collider.name=="character")
            {
                //hexNext = GetHexNumFromPositon(collider.gameObject.transform.position + character.transform.forward*Scene.xDistanceHex);
                angle = Utils.getVectorAngle(Vector3.forward, distance);
                angle = Mathf.RoundToInt((angle-30)/60) * 60 + 30;
                hexNext = Hexagon.GetHexByAngle(hexN, angle);

                if (DetectWalkable(hexNext))
                {
                    walkable = new Scene.hexNum[1] {hexNext};
                    //Utils.DebugDrawPoint(hexNext, 0, Color.red);
                }
                else
                {
                    walkable = FindWalkableHex(hexN);
                }
            }
            else{
                walkable = FindWalkableHex(hexN);
                //Debug.Log("walkable.Length: "+walkable.Length);
            }

            if (walkable.Length == 1)
                walk = 0;
            else {
                count = 0;
                while (true)
                {
                    walk = UnityEngine.Random.Range(0, walkable.Length);
                    if (!Utils.FindElementInArray(passed, walkable[walk])) break;
                    // Utils.FindElementInArray(passed, walkable[walk]) = true
                    if (count >= 5) break;
                    count++;
                }
            }

            hexA = Hexagon.NumToAxis(walkable[walk].num_x, walkable[walk].num_z);
            turnPosition = new Vector3(hexA.axis_x, 0, hexA.axis_z);
            turnPosition.y = character.transform.position.y;
            v = Hexagon.CenterVectorBetweenTwoHex(hexN, walkable[walk]);
            angle = Utils.getVectorAngle(character.transform.forward, v);
            //Debug.Log("global rot before: "+character.transform.eulerAngles.y);
            //Debug.Log("angle: "+angle);
            //Debug.Log("now: "+hexN.num_x+" "+hexN.num_z);
            //Debug.Log("next: "+walkable[walk].num_x+" "+walkable[walk].num_z);
            //Debug.Log("now axis: "+Hexagon.NumToAxis(hexN.num_x, hexN.num_z).axis_x+" "+Hexagon.NumToAxis(hexN.num_x, hexN.num_z).axis_z);
            //Debug.Log("next axis: "+hexA.axis_x+" "+hexA.axis_z);
            character.transform.rotation = Quaternion.Euler(0, angle, 0) * character.transform.rotation;
            findNextHex = false;
            //Debug.Log("global rot after: "+character.transform.eulerAngles.y);

            //Utils.DebugDrawPoint(hexN, 0, Color.red);
            //Utils.DebugDrawPoint(walkable[walk], 0, Color.blue);
            num = Scene.GetOneDimensionVal(walkable[walk].num_x, walkable[walk].num_z);
            //Debug.Log("Walkable Scene.hexOccupied[num]: "+Scene.hexOccupied[num]);
            //Debug.Log("Scene.map[num].name: "+Scene.map[num].name);
        }

        if (countSpeedDown)
        {
            timeSpeed += Time.deltaTime;
            if (timeSpeed >= speedChangeTime)
            {
                timeSpeed = 0;
                speedMultiplier = 1;
                countSpeedDown = false;
            }
        }

        switch(collider.gameObject.name)
        {
            case "grass(Clone)":
                if (!countSpeedDown)
                    speedMultiplier = 1f;
                break;
            case "speedDown(Clone)":
                speedMultiplier = 0.5f;
                break;
            default:
                break;
        }
        character.transform.position = Vector3.MoveTowards(character.transform.position, turnPosition, character.speed*Time.deltaTime*0.7f*speedMultiplier);
        //Debug.Log("character.transform.position: "+character.transform.position);
        //Debug.Log("turnPosition: "+turnPosition);


        if (character.transform.position == turnPosition)
        {
            findNextHex = true;
            try{
                passed[pointer] = walkable[walk];
            }
            catch (Exception e) {}
            pointer = (pointer+1) % pastStepNum;
        }
        
    }

    Scene.hexNum GetHexNumFromPositon(Vector3 position)
    {
        return Hexagon.AxisToNum(position.x, position.z);
    }

    Scene.hexNum[] FindWalkableHex(Scene.hexNum hexN)
    {
        Scene.hexNum[] walkable = new Scene.hexNum[0];
        Scene.hexNum[] neighbour = Hexagon.GetNeighbourHex(hexN);
        for(int i=0; i<6; i++)
        {
            if (DetectWalkable(neighbour[i]))
                Utils.Add(ref walkable, neighbour[i]);
        }
        return walkable;
    }

    bool DetectWalkable(Scene.hexNum hexN)
    {
        if (hexN.num_x==-1 || hexN.num_z==-1) return false;
        int num = Scene.GetOneDimensionVal(hexN.num_x, hexN.num_z);
        if (!Scene.hexOccupied[num]) return false;
        try {
            if (((IList)perhebited_hex_name).Contains(Scene.map[num].name)) return false;
        }
        catch (Exception e)
        {
            //Debug.Log("hexN.num_x: "+hexN.num_x);
            //Debug.Log("hexN.num_z: "+hexN.num_z);
            return false; // Scene.map[num] = null
        }
        return true;
    }
}
