using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMinimap : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float smooth = 5f;
    Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*smooth);
    }
}
