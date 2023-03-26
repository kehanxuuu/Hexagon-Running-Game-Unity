using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowNoTurn : MonoBehaviour {
    private PlayerCharacter m_Player;
    public float distanceUp=15f;
    public float distanceAway = 10f;
    public float smooth = 5f;//位置平滑移动值
    public float camDepthSmooth = 1f;
    public float x_angle = 30f;
    public float y_angle;
    public bool follow = true;
    Vector3 initial_forward;
    float multiplier = 1;
    
    void Start() 
    {
        m_Player = FindObjectOfType<PlayerCharacter>();
        initial_forward = m_Player.transform.forward;
        y_angle = m_Player.transform.rotation.eulerAngles.y;
    }

    /*
    void Update()
    {
        if ((Input.mouseScrollDelta.y < 0 && Camera.main.fieldOfView >= 3) || Input.mouseScrollDelta.y > 0 && Camera.main.fieldOfView <= 80)
        {
            Camera.main.fieldOfView += Input.mouseScrollDelta.y * camDepthSmooth * Time.deltaTime;
        }
    }*/
    void LateUpdate()
    {
        /*if (m_Player.transform.rotation.eulerAngles.y >= 150 && m_Player.transform.rotation.eulerAngles.y <= 270)
        {
            distanceAwayCur = 2 * distanceAway;
        }
        else {distanceAwayCur = distanceAway;}*/

       //相机的位置
        //相机的角度
        if (follow)
        {
            Vector3 disPos = m_Player.transform.position + Vector3.up * distanceUp - m_Player.transform.forward * distanceAway;
            transform.position=Vector3.Lerp(transform.position, disPos, Time.deltaTime*smooth);
            transform.LookAt(m_Player.transform.position);
            Vector3 angle = transform.rotation.eulerAngles;
            angle.x = x_angle;
            transform.rotation = Quaternion.Euler(angle);
        }
        else
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
            Vector3 disPos = m_Player.transform.position + Vector3.up * distanceUp - dir * distanceAway * multiplier;
            transform.position = Vector3.Lerp(transform.position, disPos,Time.deltaTime*smooth);


            transform.rotation = Quaternion.Euler(x_angle, Mathf.Lerp(cur_y_angle, y_angle, Time.deltaTime*smooth*1/4), 0);
        }
    }
}