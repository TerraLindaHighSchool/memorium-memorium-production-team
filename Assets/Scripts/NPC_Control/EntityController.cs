using System;
using System.Collections;
using Game_Managing.Game_Context;
using UnityEngine;
using UnityEngine.AI;

namespace NPC_Control {
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(Animator))]
	public class EntityController : MonoBehaviour {
		[SerializeField] private Vector3[] patrolPoints;

		public float isMovingThreshold = 0.5f;

		public MovementMode movementMode;

		public Vector3      headPos;
		public float        waitTime;
		public NavMeshAgent agent;

		private AnimationManager _animationManager;
		private Animator         _animator;

		private bool _isAnimationPlaying;

		private void OnEnable() {
			_animationManager = AnimationManager.Instance;
			_animator         = GetComponent<Animator>();
		}

		private void Update() {
			if (agent.velocity.magnitude < isMovingThreshold && _isAnimationPlaying) {
				_animationManager.SetAnimatorRunning(_animator, false);
				_isAnimationPlaying = false;
			} else if (agent.velocity.magnitude >= isMovingThreshold && !_isAnimationPlaying) {
				_animationManager.SetAnimatorRunning(_animator, true);
				_isAnimationPlaying = true;
			}

			if (_isAnimationPlaying) {
				transform.LookAt(agent.destination, transform.up);
				transform.Rotate(transform.up, 90f);
			}
		}

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