using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform arrowTransform;

    Vector3 northDirection = Vector3.forward;
    Vector3 westDirection = Vector3.left;
    Transform playerTransform;

    private void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        float angle = Vector3.Angle(playerTransform.forward, northDirection);

        if (Vector3.Dot(playerTransform.forward, westDirection) < 0)
            angle = 360f - angle;
        arrowTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
