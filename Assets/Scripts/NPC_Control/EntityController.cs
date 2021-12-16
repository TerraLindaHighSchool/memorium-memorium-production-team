using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityController : MonoBehaviour
{
    [SerializeField] private List<Vector3> patrolPoints = new List<Vector3>();
    private MovementMode _movementMode;
    public MovementMode MovementMode { get { return MovementMode; } set { MovementMode = value; } }
    public Vector3 HeadPos { get { return HeadPos; } set { HeadPos = value; } }
    public float WalkSpeed { get { return WalkSpeed;  } set { WalkSpeed = value; } }
    public float TurnSpeed { get { return TurnSpeed; } set { TurnSpeed = value; } }

    public NavMeshAgent navMeshAgent;
    public float StandTime; //How long the Entity will stand after they move before moving to next point
    
    private bool isAtPoint = false;

    public void Start()
    {
        SetSpeed();
    }

    public void StartPatrol() { }
    public void StopPatrol() { }
    public void LookAtPos(Vector3 pos) { 
        //Turn toward a position
    }
    public void MoveToPoint(Vector3 pos) {
        //Moves to a postion
        isAtPoint = false;
        navMeshAgent.SetDestination(pos);
        while (navMeshAgent.remainingDistance > 0) { }
        isAtPoint = true;
    }
    public void MoveAlongPath(Vector3[] points) {
        //Moves to a series of points
        foreach (Vector3 point in points)
        {
            MoveToPoint(point);
            while (!isAtPoint) { }
        }
        
    }

    private void SetSpeed()
    {
        navMeshAgent.speed = WalkSpeed;
        navMeshAgent.angularSpeed = TurnSpeed;
    }
}

public enum MovementMode
{
    Patrol,
    PathFollow,
    MoveToPoint,
    FreeMove
}