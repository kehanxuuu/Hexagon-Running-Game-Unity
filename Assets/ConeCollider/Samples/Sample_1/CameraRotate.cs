using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour {

    private float mouseX, mouseY;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        mouseX = -Input.GetAxis("Mouse Y");
        mouseY = Input.GetAxis("Mouse X");
        var ang = this.transform.localEulerAngles;
        this.transform.localEulerAngles += new Vector3(mouseX, mouseY, 0);
    }
}
