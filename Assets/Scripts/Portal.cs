using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool inDoor;
    public Portal pairedDoor;
    public Scene.hexNum nearestHex; // nearest Hex
    public Vector3 forwardVector;
    public AudioClip transmitSound;
    PlayerController controller;
    ActiveCamera activeCamera;


    void Start()
    {
        //Vector3 v = transform.position + transform.forward*Scene.xDistanceHex;
        //Debug.DrawLine(v, v+transform.forward*0.5f, Color.blue, 10f);
        //Debug.Log("transform.rotation.y: "+transform.eulerAngles.y);

        //int angle = Utils.getVectorAngle(Vector3.forward, forwardVector);
        //Utils.DebugDrawPoint(nearestHex, angle, Color.red);

        controller = FindObjectOfType<PlayerController>();
        activeCamera = GameObject.Find("Camera").GetComponent<ActiveCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();

        if (inDoor)
        {
            //player.Move(0); // stop player, cancel original speed
            Vector3 pos;
            Scene.hexAxis hexA = Hexagon.NumToAxis(pairedDoor.nearestHex.num_x, pairedDoor.nearestHex.num_z);
            pos.x = hexA.axis_x;
            pos.z = hexA.axis_z;
            pos.y = 0.5f;

            controller.waitToTurn = false;
            controller.waitToTransmit = true;
            activeCamera.transmit = true;
            //controller.waitToTransmit = true;
            player.transform.forward = pairedDoor.forwardVector;
            player.transform.position = pos;
            
            AudioSource.PlayClipAtPoint(transmitSound, player.transform.position);
            //Debug.Log("Set rot: "+player.transform.eulerAngles.y);
        }
    }
}
