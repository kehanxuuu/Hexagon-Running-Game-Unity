using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private PlayerCharacter m_Player;
    public float distanceUp=15f;
    public float distanceAway = 10f;
    public float smooth = 5f;//位置平滑移动值
    public float camDepthSmooth = 1f;
    public float x_angle = 30f;
    
    void Start() 
    {
        m_Player = FindObjectOfType<PlayerCharacter>();
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
       //相机的位置
        Vector3 disPos = m_Player.transform.position + Vector3.up * distanceUp - m_Player.transform.forward * distanceAway;
        transform.position=Vector3.Lerp(transform.position,disPos,Time.deltaTime*smooth);
        //相机的角度
        transform.LookAt(m_Player.transform.position);
        Vector3 angle = transform.rotation.eulerAngles;
        angle.x = x_angle;
        transform.rotation = Quaternion.Euler(angle);
    }
}