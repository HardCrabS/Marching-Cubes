using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float speed = 15f;

    bool isMoving = false;
    Vector3 destination;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        if (!isMoving)
            StartCoroutine(MoveToCoroutine());
    }

    public void StopMoving()
    {
        StopAllCoroutines();
        isMoving = false;
    }

    IEnumerator MoveToCoroutine()
    {
        isMoving = true;
        while ((transform.position - destination).magnitude > Mathf.Epsilon)
        {
            yield return new WaitForFixedUpdate();

            float moveDistance = speed * Time.fixedDeltaTime;
            Vector3 direction = (destination - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveDistance);

            LookAt(destination);
        }
        isMoving = false;
    }

    public void LookAt(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion yRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        rb.MoveRotation(yRotation);
    }
}
