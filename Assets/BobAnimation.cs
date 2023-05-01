using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAnimation : MonoBehaviour
{
    public float magnitude;
    public float idleSpeed;
    public float walkSpeedMultiplier;
    public float walkSpeedMax;

    float sinY = 0f;
    float sinX = 0f;
    Vector3 initPosition;
    Vector3 lastPosition;

    FirstPersonMovement fpsMovement;

    private void Start()
    {
        fpsMovement = GetComponentInParent<FirstPersonMovement>();
        initPosition = transform.localPosition;
        lastPosition = transform.position;
    }


    void Update()
    {
        float delta = Time.deltaTime * idleSpeed;
        Vector2 playerInputVelocity = fpsMovement.getDesiredVelocity();
        float velocity = playerInputVelocity.magnitude * walkSpeedMultiplier;
        delta += Mathf.Clamp(velocity, 0, walkSpeedMax);

        // Reduce by two so that the gun animation is more U shaped
        sinX += delta / 2;
        sinY += delta;

        sinX %= Mathf.PI * 2;
        sinY %= Mathf.PI * 2;

        transform.localPosition = initPosition;
        transform.localPosition += Vector3.up * Mathf.Sin(sinY) * magnitude;
        transform.localPosition += Vector3.right * Mathf.Sin(sinX) * magnitude;

        lastPosition = transform.position;
    }
}