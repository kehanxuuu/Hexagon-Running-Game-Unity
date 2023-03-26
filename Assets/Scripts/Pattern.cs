using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    // Pattern 1: Jump
    public static int height = 13;
    public static int width = 10;
    public static bool start_even = true;
    public static int[] area;
    public static Scene.hexNum[] normalArea = new Scene.hexNum[] {
        new Scene.hexNum {num_x=0, num_z=0},
        new Scene.hexNum {num_x=0, num_z=1},
        new Scene.hexNum {num_x=1, num_z=2},
        new Scene.hexNum {num_x=3, num_z=6},
        new Scene.hexNum {num_x=3, num_z=7},
        new Scene.hexNum {num_x=4, num_z=7},
        //new Scene.hexNum {num_x=6, num_z=7},
        //new Scene.hexNum {num_x=7, num_z=7},
        new Scene.hexNum {num_x=8, num_z=7},
        new Scene.hexNum {num_x=9, num_z=7},
        new Scene.hexNum {num_x=10, num_z=7},
        new Scene.hexNum {num_x=11, num_z=8},
        new Scene.hexNum {num_x=10, num_z=9},
    };

    public static Scene.hexNum[] jumpArea = new Scene.hexNum[] {
        new Scene.hexNum {num_x=1, num_z=3},
        new Scene.hexNum {num_x=5, num_z=7},
    };

    public static Scene.hexNum start = new Scene.hexNum {num_x=0, num_z=0};
    public static Scene.hexNum end = new Scene.hexNum {num_x=10, num_z=9};

    void Awake()
    {
        Initialize();
    }

    public static void Initialize()
    {
        // start.z %2 must be 0
        area = new int[height*width];
        int num;
        Scene.hexNum hexN;
        for (int i=0; i<height; i++)
            for (int j=0; j<width; j++)
            {
                num = GetOneDimensionVal(i, j);
                hexN = new Scene.hexNum {num_x=i, num_z=j};
                if (((IList)jumpArea).Contains(hexN))
                    area[num] = 6;
                else if (((IList)normalArea).Contains(hexN))
                    //area[num] = Random.Range(0, 4);
                    area[num] = 0;
                else area[num] = -1;
            }
    }

    public static int GetOneDimensionVal(int x, int z)
    {
        return x*width + z;
    }


}
