using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPoint : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = Quaternion.identity; 
        
    }
}
