using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHexagonMove : MonoBehaviour
{
    public float MoveSpeed;
    Vector3 positionOriginal, positionUp;
    bool up;

    void Awake()
    {
        positionOriginal = transform.position;
        up = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Character")
        {
            positionUp = transform.position;
            positionUp.y = 0.4f;
            up = true;
            //PlayerController controller = FindObjectOfType<PlayerController>();
            //controller.printInGround = true;
        }
    }

    void Update()
    {
        if (up)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionUp, MoveSpeed*Time.deltaTime);
            if (transform.position == positionUp)
                up = false;
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, positionOriginal, MoveSpeed*Time.deltaTime);
        }
    }
}
