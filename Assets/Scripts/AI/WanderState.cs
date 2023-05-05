using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    public float obstacleRange = 5f;
    public Vector2 timeToWanderRange = new Vector2(1f, 5f);

    AIMovement movement;

    private void Start()
    {
        movement = GetComponent<AIMovement>();
    }

    public override void OnEnterState()
    {
        Debug.Log("WanderState OnEnterState");
        StartCoroutine(WanderAroundCo());
    }

    public override void OnExitState()
    {
        Debug.Log("WanderState OnExitState");
        movement.StopMoving();
        StopAllCoroutines();
    }

    public override StateType DecideTransition()
    {
        return StateType.Wander;
    }

    public override void Execute()
    {

    }

    IEnumerator WanderAroundCo()
    {
        while(true)
        {
            float time = 0f;
            float timeToWander = Random.Range(timeToWanderRange.x, timeToWanderRange.y);

            RotateRandomly();
            while (time < timeToWander)
            {
                WanderAround();
                time += Time.deltaTime;
                yield return null;
            }
            movement.StopMoving();

            float timeToStandStill = Random.Range(timeToWanderRange.x, timeToWanderRange.y);
            yield return new WaitForSeconds(timeToStandStill);
        }
    }

    void WanderAround()
    {
        movement.SetDestination(transform.position + transform.forward * obstacleRange);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.75f, out hit))
        {
            if (hit.distance < obstacleRange)
            {
                RotateRandomly();
            }
        }
    }

    void RotateRandomly()
    {
        float angle = Random.Range(-180, 180);
        transform.Rotate(0, angle, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstacleRange);
    }
}
