using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActiveCamera : MonoBehaviour
{
    public CinemachineVirtualCameraBase vcamFollow;
    public CinemachineVirtualCameraBase vcamNoTurn;
    public bool vcamF;
    public bool gameRestart;
    public bool transmit;
    CinemachineBrain brain;

    void Start () 
    {
        brain = FindObjectOfType<CinemachineBrain>();

        vcamF = true;
        gameRestart = false;
        transmit = false;
	    vcamFollow.MoveToTopOfPrioritySubqueue();
	}

    void Update()
    {
        if (gameRestart || transmit)
        {
            //Debug.Log("Reach here");
            brain.m_DefaultBlend.m_Time = 0;
            gameRestart = false;
            transmit = false;
            vcamNoTurn.VirtualCameraGameObject.SetActive(false);
            vcamFollow.VirtualCameraGameObject.SetActive(true);
            return;
        }
        else{
            brain.m_DefaultBlend.m_Time = 1.5f;
        }

        if (vcamF)
        {
            vcamNoTurn.VirtualCameraGameObject.SetActive(false);
            vcamFollow.VirtualCameraGameObject.SetActive(true);
        }
        else{
            vcamNoTurn.VirtualCameraGameObject.SetActive(true);
            vcamFollow.VirtualCameraGameObject.SetActive(false);
        }
    }
}
