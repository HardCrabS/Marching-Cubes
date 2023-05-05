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
            float moveDistance = speed * Time.deltaTime;
            Vector3 direction = (destination - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveDistance);

            yield return null;
        }
        isMoving = false;
    }
}
