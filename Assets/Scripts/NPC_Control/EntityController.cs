using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityController : MonoBehaviour
{
    [SerializeField] private Vector3[] patrolPoints;
    private MovementMode _movementMode;
    public MovementMode MovementMode { get { return _movementMode; } set { _movementMode = value; } }
    public Vector3 HeadPos;
    public float WaitTime;
    public NavMeshAgent agent;

    public void StartPatrol()
    {
        MovementMode = MovementMode.Patrol;
        MoveAlongPoints(patrolPoints);
    }
    public void StopPatrol()
    {
        MovementMode = MovementMode.Default;
        agent.isStopped = true;
    }
    public void LookAtPosition(Vector3 position)
    {
        position = new Vector3(position.x, this.transform.position.y, position.z);
        this.transform.LookAt(position);
    }
    public void MoveToPoint(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
    }
    public void MoveAlongPoints(Vector3[] points)
    {
        StartCoroutine(MovePoints(points));
    }
    private IEnumerator MovePoints(Vector3[] points)
    {
        agent.isStopped = false;
        foreach (Vector3 point in points)
        {
            agent.SetDestination(point);
            if (agent.remainingDistance <= 0)
            {
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(WaitTime);
        }

        yield return null;
    }
}

public enum MovementMode
{
    Default,
    Patrol,
    PathFollow,
    MoveToPoint,
    FreeMove
}