using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    public int height; // hexagon num in x direction
    public int width; // hexagon num in z direction
    // from bottom to top: x(left), x(right), x(left), x(right)
    
    //for road fence: center != rot center
    public float bias_x = 0;
    public float bias_z = 0;
    public float bias_y = 0;

}
