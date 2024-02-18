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
        Vector3 newPos = new Vector3(target.position.x + xOffset, target.position.y + 10 + yOffset, zOffset);
        
        // Define your minimum bound for the x coordinate
        float minX = -1014f;

        // Ensure the new position's x coordinate is at least minX
        newPos.x = Mathf.Max(newPos.x, minX);

        // Set the camera's position using Lerp
        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
