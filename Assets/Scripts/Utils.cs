using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void Add<T>(ref T[] array, T element)
    {
        Expand(ref array, 1);
        array[array.Length - 1] = element;
    }

    public static void Add<T>(ref T[] array, T[] elements)
    {
        int oriLength = array.Length;
        Expand(ref array, elements.Length);
        Array.Copy(elements, 0, array, oriLength, elements.Length);
    }

    public static void Expand<T>(ref T[] array, int length)
    {
        Array.Resize(ref array, array.Length + length);
    }

    public static void Sort(ref Scene.areaStartEnd[] array)
    {
        Scene.areaStartEnd tmp;
        for (int i=1; i<=array.Length-1; i++)
            for (int j=0; j<array.Length-i; j++)
        {
            if(array[j]>array[j+1])
            {
                tmp = array[j];
                array[j] = array[j+1];
                array[j+1] = tmp;
            }
        }
    }

    public static Scene.areaStartEnd[] Reverse(Scene.areaStartEnd[] array)
    {
        Scene.areaStartEnd[] tmp = new Scene.areaStartEnd[array.Length];
        for (int i=0; i<array.Length; i++)
        {
            tmp[i] = array[array.Length-1-i];
        }
        return tmp;
    }

    public static bool FindElementInArray(Scene.hexNum[] array, Scene.hexNum element)
    {
        for (int i=0; i<array.Length; i++)
        {
            if (array[i].num_x==element.num_x && array[i].num_z==element.num_z)
                return true;
        }
        return false;
    }

    public static void DebugDrawPoint(Scene.hexNum hexN, int angle, Color color, float y=0.5f)
    {
        Scene.hexAxis hexA = Hexagon.NumToAxis(hexN.num_x, hexN.num_z);
        Vector3 pos;
        pos.x = hexA.axis_x;
        pos.z = hexA.axis_z;
        pos.y = y;

        Vector3 dir = Matrix4x4.Rotate(Quaternion.Euler(0, angle, 0)).MultiplyVector(Vector3.forward);

        Debug.DrawLine(pos, pos+dir*0.5f, color, 10f);
    }

    public static int getVectorAngle(Vector3 from, Vector3 to)
    {
        //Debug.Log("from: "+from);
        //Debug.Log("to: "+to);
        int angle = Mathf.RoundToInt(Vector3.Angle(from, to));
        if (Vector3.Cross(from, to).y<0)
            angle = -angle;
        return angle;
    }

    public static int[] GetRandomNum(int length)
    {
        int[] num = new int[length];
        for (int i = 0; i < length; i++)
            num[i] = i;
        for (int i = 0; i < length; i++)
        {
            int tmp = num[i];
            int randomIndex = UnityEngine.Random.Range(0, length);
            num[i] = num[randomIndex];
            num[randomIndex] = tmp;
        }
        return num;
    }
}
