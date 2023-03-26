using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateHexagon : MonoBehaviour
{
    GameObject[] hex;
    public static string[] hex_name = {
        "grass",
        "sand",
        "stone",
        "water",
        "speedUp",
        "speedDown",
        "jump",
        "checkpoint",
    };

    int random_val;
    void Awake()
    {
        LoadHexagonObjects();

        for(int i=0; i<50; i++)
            for(int j=0; j<50; j++)
            {
                float x_axis = Scene.xDistanceHex * j;
                if(i%2==1) x_axis += Scene.xDistanceHex/2;
                float z_axis = Scene.zDistanceHex * i;

                if(i==10 && j==10)
                {
                    GenHexagon(hex[7], new Vector3(x_axis, 0, z_axis), Quaternion.identity, transform);
                    continue;
                }

                if(i%5==2 && j%5==2)
                {
                    GenHexagon(hex[6], new Vector3(x_axis, 0, z_axis), Quaternion.identity, transform);
                    continue;
                }

                random_val = Random.Range(0, 6);
                GenHexagon(hex[random_val], new Vector3(x_axis, 0, z_axis), Quaternion.identity, transform);
            }
            //new_grass[i] = Instantiate(grass, new Vector3(xDistance * i, 0, 0), Quaternion.identity, transform);
        //for(int i=10; i< 20; i++)
            //new_grass[i] = Instantiate(grass, new Vector3(xDistance * (i-10) + xDistance/2, 0, zDistance), Quaternion.identity, level.transform);
        //for(int i=0; i< 10; i++)
            //new_sand[i] = Instantiate(sand, new Vector3(xDistance * i, 0, zDistance*2), Quaternion.identity, level.transform);
    }

    void LoadHexagonObjects()
    {
        string dir = "Prefabs/Hex/";
        hex = new GameObject[hex_name.Length];
        for (int i=0; i<hex_name.Length; i++)
        {
            hex[i] = (GameObject)Resources.Load(dir+hex_name[i]);
        }
    }

    void GenHexagon(GameObject obj, Vector3 pos, Quaternion rot, Transform trans)
    {
        Instantiate(obj, pos, rot, trans);
        /*
        if(!map[num].GetComponent<BoxCollider>())
        {
            map[num].AddComponent<BoxCollider>();
        }*/
    }
}
