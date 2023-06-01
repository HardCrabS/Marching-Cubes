using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;  // The point to orbit around
    public float distance = 5f;  // Distance from the target
    public float orbitSpeed = 10f;  // Speed of the orbit

    private Vector3 offset;  // Offset from the target to the camera

    private void Start()
    {
        // Calculate the initial offset from the target to the camera
        offset = transform.position - target.position;
    }

    private void Update()
    {
        // Calculate the desired rotation based on user input or a predetermined path
        Quaternion desiredRotation = Quaternion.Euler(0f, orbitSpeed * Time.deltaTime, 0f);

        // Apply rotation to the offset
        offset = desiredRotation * offset;

        // Calculate the desired position based on the target's position and the offset
        Vector3 desiredPosition = target.position + offset;

        // Set the camera's position to the desired position
        transform.position = desiredPosition;

        // Make the camera look at the target
        transform.LookAt(target.position);
    }
}
