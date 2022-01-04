using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityController : MonoBehaviour
{
    [SerializeField] private Vector3[] patrolPoints;
    private MovementMode _movementMode;
    public MovementMode MovementMode { get { return _movementMode; } set { _movementMode = value; } }
    public Vector3 HeadPos { get; set; }
    public float WaitTime { get; set; }
    public NavMeshAgent agent;

    private bool isMoving;
    private bool canMove;
    private bool movementPaused;
    private bool isAtPoint;

    public void StartPatrol()
    {
        MovementMode = MovementMode.Patrol;
        if (isMoving)
        {
            MoveAlongPoints(patrolPoints);
        }
        else
        {
            movementPaused = false;
        }
    }
    public void StopPatrol()
    {
        MovementMode = MovementMode.Default;
        movementPaused = true;
    }
    public void LookAtPosition(Vector3 position)
    {

    }
    public void MoveToPoint(Vector3 position)
    {
        isMoving = true;

        agent.SetDestination(position);

        isMoving = false;
    }
    public void MoveAlongPoints(Vector3[] points)
    {
        StartCoroutine(MovePoints(points));
    }

    private IEnumerator WaitAtPoint()
    {
        isAtPoint = true;

        yield return new WaitForSeconds(WaitTime);

        isAtPoint = false;
    }
    private IEnumerator MovePoints(Vector3[] points)
    {
        isMoving = true;

        foreach (Vector3 point in points)
        {
            agent.SetDestination(point);
            StartCoroutine(WaitAtPoint());
        }

        isMoving = false;

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