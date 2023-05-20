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

    FirstPersonMovement fpsMovement;

    private void Awake()
    {
        GetComponentInParent<Gun>().onPickUp += Initialize;
    }

    private void Initialize()
    {
        fpsMovement = GetComponentInParent<FirstPersonMovement>();
        initPosition = transform.localPosition;
    }

    void Update()
    {
        if (!fpsMovement)
            return;

        float delta = idleSpeed;
        Vector2 playerInputVelocity = fpsMovement.getDesiredVelocity();
        float velocity = playerInputVelocity.magnitude * walkSpeedMultiplier;
        delta += Mathf.Clamp(velocity, 0, walkSpeedMax);
        delta *= Time.deltaTime;

        // Reduce by two so that the gun animation is more U shaped
        sinX += delta / 2;
        sinY += delta;

        sinX %= Mathf.PI * 2;
        sinY %= Mathf.PI * 2;

        transform.localPosition = initPosition;
        transform.localPosition += Vector3.up * Mathf.Sin(sinY) * magnitude;
        transform.localPosition += Vector3.right * Mathf.Sin(sinX) * magnitude;
    }
}