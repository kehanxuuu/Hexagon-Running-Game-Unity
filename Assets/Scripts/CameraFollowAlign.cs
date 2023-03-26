using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAlign : MonoBehaviour
{
    private PlayerCharacter m_Player;
    public float distanceUp=15f;
    public float distanceAway = 10f;
    public float smooth = 5f;
    public float x_angle = 30f;
    public float y_angle;

    void Start()
    {
        m_Player = FindObjectOfType<PlayerCharacter>();
    }
    void LateUpdate()
    {
        float cur_y_angle = transform.rotation.eulerAngles.y;
        float cur_angle_1 = cur_y_angle + 360;
        float cur_angle_2 = cur_y_angle - 360;

        if (Mathf.Abs(cur_y_angle-y_angle)>Mathf.Abs(cur_angle_1-y_angle))
            cur_y_angle = cur_angle_1;
        if (Mathf.Abs(cur_y_angle-y_angle)>Mathf.Abs(cur_angle_2-y_angle))
            cur_y_angle = cur_angle_2;

        //if (cur_y_angle > 180) cur_y_angle -= 360;
        /*multiplier = 1;
        if (Mathf.Abs(cur_y_angle-y_angle)>60) 
        {
            multiplier = 2;
            Debug.Log("multiplier = 2");
        }*/

        Vector3 dir = Matrix4x4.Rotate(Quaternion.Euler(0, y_angle, 0)).MultiplyVector(Vector3.forward);
        Vector3 disPos = m_Player.transform.position + Vector3.up * distanceUp - dir * distanceAway;
        transform.position = Vector3.Lerp(transform.position, disPos,Time.deltaTime*smooth);


        transform.rotation = Quaternion.Euler(x_angle, Mathf.Lerp(cur_y_angle, y_angle, Time.deltaTime*smooth*1/4), 0);
    }
}
