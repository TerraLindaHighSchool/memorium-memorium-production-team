using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace NPC_Control {
	[RequireComponent(typeof(NavMeshAgent))]
	public class EntityController : MonoBehaviour {
		[SerializeField] private Vector3[]    patrolPoints;

		public MovementMode movementMode;

		public Vector3      headPos;
		public float        waitTime;
		public NavMeshAgent agent;

		public void StartPatrol() {
			movementMode = MovementMode.Patrol;
			MoveAlongPoints(patrolPoints);
		}

		public void StopPatrol() {
			movementMode    = MovementMode.Default;
			agent.isStopped = true;
		}

		public void LookAtPosition(Vector3 position) {
			position = new Vector3(position.x, this.transform.position.y, position.z);
			this.transform.LookAt(position);
		}

		public void MoveToPoint(Vector3 position) {
			agent.isStopped = false;
			agent.SetDestination(position);
		}

		public void MoveAlongPoints(Vector3[] points) { StartCoroutine(MovePoints(points)); }

		private IEnumerator MovePoints(Vector3[] points) {
			agent.isStopped = false;
			foreach (Vector3 point in points) {
				agent.SetDestination(point);
				if (agent.remainingDistance <= 0.1) { yield return new WaitForSeconds(1); }

				yield return new WaitForSeconds(waitTime);
			}

			yield return null;
		}
	}

	public enum MovementMode {
		Default,
		Patrol,
		PathFollow,
		MoveToPoint,
		FreeMove
	}
}