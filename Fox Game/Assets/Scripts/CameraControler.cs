using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float FollowSpeed = 100f;
    public float xOffset;
    public float yOffset;
    public float zOffset;
    public Transform target;

    void Update()
    {
        
        Vector3 newPos = new Vector3(target.position.x+xOffset,target.position.y+yOffset,zOffset);
        transform.position = Vector3.Slerp(transform.position,newPos,FollowSpeed*Time.deltaTime);

    }
}
