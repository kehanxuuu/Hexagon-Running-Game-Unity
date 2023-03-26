using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isAlive;
    public float speed;
    public Animator animator;
    protected Rigidbody rigid;
    //protected BoxCollider collider;
    protected Renderer render;

    public void Move(float multiplier)
    {
        if (!isAlive) return;

        float angle = transform.rotation.eulerAngles.y;
        rigid.velocity = new Vector3(Mathf.Sin(2*Mathf.PI*angle/360f), 0, Mathf.Cos(2*Mathf.PI*angle/360f)) * speed * multiplier;

        //
        //rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public Collider HexagonCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.3f);
        //Debug.Log(colliders.Length);
        int tmp = -1;
        float distance;
        float max_distance = 30;
        for (int i = 0; i < colliders.Length; i++)
        {
            //Debug.Log(colliders[i].gameObject.name);
            string s = colliders[i].gameObject.name.Substring(0, colliders[i].gameObject.name.Length-7);
            if (((IList)GenerateScene.hex_name).Contains(s))
            {
                distance = (colliders[i].transform.position - transform.position).magnitude;
                if(distance < max_distance)
                {
                    max_distance = distance;
                    tmp = i;
                }
                // return colliders[i];
            }
        }
        if(tmp>=0) return colliders[tmp];
        return null;
    }

    /*public Vector3 GetHexTurnPosition(Collider collider)
    {
        Transform currentHex = collider.transform;
        Vector3 dis = currentHex.position - transform.position;
        dis.y = 0;
        Vector3 turnPosition;

        if (Vector3.Dot(dis, transform.forward)>=0)
        {
            // Turn at the current hexagon
            turnPosition = currentHex.position;
            turnPosition.y = transform.position.y;
            return turnPosition;
        }

        // Vector3.Dot(dis, transform.forward)<0
        // Turn at the next hexagon
        turnPosition = currentHex.position + transform.forward * 1.1f;
        turnPosition.y = transform.position.y;
        return turnPosition;
    }*/

    public Vector3 GetHexTurnPosition(Collider collider)
    {
        Vector3 currentHex = collider.transform.position;
        Vector3 turnPosition = currentHex + transform.forward * Scene.xDistanceHex;
        turnPosition.y = transform.position.y;
        return turnPosition;
    }
}
