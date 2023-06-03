using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody m_rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    bool terrainWasEdited = false;
    Vector3 terrainEditPoint;
    int editingRadius;

    void Awake()
    {
        // Get the rigidbody on this.
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        Vector2 targetVelocity = getDesiredVelocity();

        // Apply movement.
        m_rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, m_rigidbody.velocity.y, targetVelocity.y);
    }

    void LateUpdate()
    {
        if(terrainWasEdited)
        {
            // move player a bit up in case terrain is edited by his feet. Prevents falling throught regenerated collider
            float pointOffset = MeshGenerator.Instance.pointsOffset;
            float dst = Vector3.Distance(transform.position, terrainEditPoint);
            if (dst < editingRadius * pointOffset / 2)
            {
                Vector3 dir = transform.up * (editingRadius * pointOffset - dst*0.5f);
                transform.position = transform.position + dir;
            }
            terrainWasEdited = false;
        }
    }

    public Vector2 getDesiredVelocity()
    {
        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetVelocity = targetVelocity.normalized * targetMovingSpeed;

        return targetVelocity;
    }

    public void NotifyTerrainChange(Vector3 point, int radius)
    {
        terrainWasEdited = true;
        terrainEditPoint = point;
        editingRadius = radius;
    }
}