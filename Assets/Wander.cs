using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float stayTime = 5f;
    public float maxDistance = 10f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float maxTimeToReach = 5f;

    Rigidbody myRb;
    Animator animator;

    bool resting = false;
    Vector3 targetPosition;

    private void Start()
    {
        myRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        StartCoroutine(WanderAround());
    }

    IEnumerator WanderAround()
    {
        while (true)
        {
            if (!resting)
            {
                animator.SetBool("Idle", false);
                yield return StartCoroutine(GoToPosition());
                resting = true;
                targetPosition = PickRandomPos();
            }
            
            if (resting)
            {
                animator.SetBool("Idle", true);
                yield return new WaitForSeconds(stayTime);
                resting = false;
            }
        }
    }

    private Vector3 PickRandomPos()
    {
        float sampleX = UnityEngine.Random.Range(-maxDistance, maxDistance);
        float sampleY = UnityEngine.Random.Range(-maxDistance, maxDistance);
        Vector3 rayStart = new Vector3(sampleX, 0, sampleY) + transform.position + Vector3.up * 50f;

        if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            return Vector3.zero;

        return hit.point;
    }

    IEnumerator GoToPosition()
    {
        float timer = 0f;
        
        if (targetPosition == Vector3.zero)
            yield break;
        
        while(Vector3.Distance(transform.position, targetPosition) > 1f)
        {
            myRb.velocity = (targetPosition - transform.position).normalized * moveSpeed;
            RotateTowardsTarget(targetPosition);
            timer += Time.deltaTime;
            if (timer >= maxTimeToReach)
                yield break;          
            yield return null;
        }
    }

    void RotateTowardsTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
