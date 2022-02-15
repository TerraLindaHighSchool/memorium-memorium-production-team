using System;
using Game_Managing.Game_Context;
using NPC_Control.Dialogue;
using Other;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Control {
	/// <summary>
	/// MonoBehaviour that lives on the <c>Player</c> prefab and controls many player-related things. 
	/// Handles player movement, and for the moment handles all input in the game with <c>PlayerInputActions</c>. 
	/// </summary>
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : MonoBehaviour {
		///Player speed multiplier. 
		public float speed = 10f;

		///Player jump height multiplier.
		public float jump = 1f;

		///Player gravity multiplier.
		public float gravity = 1f;

		///Player maximum interaction distance.
		public float interactDistance = 5f;

		/// <summary>
		/// Invoked after the player has moved.
		/// Used for recalculating interactable outlines.
		/// </summary>
		public event Action Moved;

		/// <summary>
		/// Field for keeping track of player velocity between frames.
		/// Currently vertical velocity is tracked, but horizontal velocity is just set by WASD. 
		/// </summary>
		private Vector3 _velocity;

		/// <summary>
		/// Array of booleans for keeping track if <c>W, A, S, D</c> are pressed. 
		/// </summary>
		private readonly bool[] _wasd = new bool[4];

		///CharacterController component for moving the player. 
		private CharacterController _characterController;

		/// <summary>
		/// The Game Context Manager, for checking what context the game is in.
		/// </summary>
		private GameContextManager _gameContextManager;

		/// <summary>
		/// Not actually used, only here to force a dialogue manager into existence by referencing <c>.Instance</c>
		/// </summary>
		private DialogueManager _unusedDialogueManager;

		/// <summary>
		/// Not actually used, only here to force a respawn manager into existence by referencing <c>.Instance</c>
		/// </summary>
		private RespawnManager _unusedRespawnManager;

		///Gets a reference to the CharacterController and subscribes to necessary events. 
		private void OnEnable() {
			_gameContextManager = GameContextManager.Instance;

			_unusedDialogueManager = DialogueManager.Instance;
			_unusedRespawnManager  = RespawnManager.Instance;

			_characterController = GetComponent<CharacterController>();

			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.W.started  += OnWStarted;
			playerInputActions.Player.W.canceled += OnWCancelled;
			playerInputActions.Player.A.started  += OnAStarted;
			playerInputActions.Player.A.canceled += OnACancelled;
			playerInputActions.Player.S.started  += OnSStarted;
			playerInputActions.Player.S.canceled += OnSCancelled;
			playerInputActions.Player.D.started  += OnDStarted;
			playerInputActions.Player.D.canceled += OnDCancelled;

			playerInputActions.Player.Jump.performed += OnJump;
		}

		private void OnDisable() {
			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.W.started  -= OnWStarted;
			playerInputActions.Player.W.canceled -= OnWCancelled;
			playerInputActions.Player.A.started  -= OnAStarted;
			playerInputActions.Player.A.canceled -= OnACancelled;
			playerInputActions.Player.S.started  -= OnSStarted;
			playerInputActions.Player.S.canceled -= OnSCancelled;
			playerInputActions.Player.D.started  -= OnDStarted;
			playerInputActions.Player.D.canceled -= OnDCancelled;

			playerInputActions.Player.Jump.performed -= OnJump;
		}

		/// <summary>
		/// Returns the distance in the scene to the specified GameObject. 
		/// </summary>
		/// <param name="obj">The other GameObject to find the distance to. </param>
		/// <returns>The float distance between the player and <c>obj</c>. </returns>
		public float GetDistanceToObject(GameObject obj) {
			return Vector3.Distance(transform.position, obj.transform.position);
		}

		/// <summary>
		/// If the player is on the ground, adds the jump force to the vertical velocity. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>Jump.performed</c> event.</param>
		private void OnJump(InputAction.CallbackContext context) {
			if (_characterController.isGrounded
			 && (_gameContextManager.ActiveContext is OrbitCameraManager
			  || _gameContextManager.ActiveContext is FixedCameraContextController)) { _velocity.y += jump; }
		}

		/// <summary>
		/// Sets <c>_wasd[0]</c> aka the <c>W</c> bool based on the input events. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>W.started</c> event.</param>
		private void OnWStarted(InputAction.CallbackContext context) { _wasd[0] = true; }

		/// <summary>
		/// Sets <c>_wasd[0]</c> aka the <c>W</c> bool based on the input events. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>W.cancelled</c> event.</param>
		private void OnWCancelled(InputAction.CallbackContext context) { _wasd[0] = false; }

		/// <summary>
		/// Sets <c>_wasd[1]</c> aka the <c>A</c> bool based on the input events. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>A.started</c> event.</param>
		private void OnAStarted(InputAction.CallbackContext context) { _wasd[1] = true; }

		/// <summary>
		/// Sets <c>_wasd[1]</c> aka the <c>A</c> bool based on the input events. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>A.cancelled</c> event.</param>
		private void OnACancelled(InputAction.CallbackContext context) { _wasd[1] = false; }

		/// <summary>
		/// Sets <c>_wasd[2]</c> aka the <c>S</c> bool based on the input events. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>S.started</c> event.</param>
		private void OnSStarted(InputAction.CallbackContext context) { _wasd[2] = true; }

		/// <summary>
		/// Sets <c>_wasd[2]</c> aka the <c>S</c> bool based on the input events. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>S.cancelled</c> event.</param>
		private void OnSCancelled(InputAction.CallbackContext context) { _wasd[2] = false; }

		/// <summary>
		/// Sets <c>_wasd[3]</c> aka the <c>D</c> bool based on the input events. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>D.started</c> event.</param>
		private void OnDStarted(InputAction.CallbackContext context) { _wasd[3] = true; }

		/// <summary>
		/// Sets <c>_wasd[3]</c> aka the <c>D</c> bool based on the input events. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>D.cancelled</c> event.</param>
		private void OnDCancelled(InputAction.CallbackContext context) { _wasd[3] = false; }

		private void OnTriggerEnter(Collider other) {
			if (other.gameObject.CompareTag("FixedCamCollider"))
				other.GetComponent<FixedCameraContextController>().OnPlayerEnter();
		}

		private void OnTriggerExit(Collider other) {
			if (other.gameObject.CompareTag("FixedCamCollider")) {
				other.GetComponent<FixedCameraContextController>()
				     .GetPlayerFollowCamTarget()
				     .SetPositionAndRotation(
					      other.GetComponent<FixedCameraContextController>().GetPlayerFollowCamTarget().position,
					      Quaternion.Euler(
						      other.GetComponent<FixedCameraContextController>()
						           .GetPlayerFollowCamTarget()
						           .eulerAngles.x,
						      other.GetComponent<FixedCameraContextController>()
						           .GetPlayerFollowCamTarget()
						           .eulerAngles.y
						    + 180,
						      other.GetComponent<FixedCameraContextController>()
						           .GetPlayerFollowCamTarget()
						           .eulerAngles.z));
				other.GetComponent<FixedCameraContextController>().OnPlayerExit();
			}
		}

		/// <summary>
		/// Moves and rotates the player based on whichever movement keys are pressed.
		/// Called each frame in <c>Update</c>.
		/// </summary>
		private void Move() {
			Vector3 motion = new Vector3();

			IGameContext activeContext         = _gameContextManager.ActiveContext;
			Transform    playerFollowCamTarget = activeContext.GetPlayerFollowCamTarget();

			//if counteracting keys are pressed, set both to false
			if (_wasd[0] && _wasd[2]) {
				_wasd[0] = false;
				_wasd[2] = false;
			}

			if (_wasd[1] && _wasd[3]) {
				_wasd[1] = false;
				_wasd[3] = false;
			}

			if (_wasd[0] || _wasd[1] || _wasd[2] || _wasd[3]) {
				Vector3 eulers = transform.eulerAngles;

				{
					Vector3    storedCamTargetPos = playerFollowCamTarget.position;
					Quaternion storedCamTargetRot = playerFollowCamTarget.rotation;

					transform.SetPositionAndRotation(transform.position,
					                                 Quaternion.Euler(
						                                 new Vector3(eulers.x, activeContext.GetYRotForForwards(),
						                                             eulers.z)));

					playerFollowCamTarget.SetPositionAndRotation(
						storedCamTargetPos, storedCamTargetRot);
				}

				Vector3 dir = new Vector3();

				if (_wasd[0]) { dir += transform.forward; }

				if (_wasd[1]) { dir += -transform.right; }

				if (_wasd[2]) { dir += -transform.forward; }

				if (_wasd[3]) { dir += transform.right; }

				dir.Normalize();

				Quaternion motionRot = Quaternion.identity;
				motionRot.SetLookRotation(dir);

				Vector3    storedCamPos = playerFollowCamTarget.position;
				Quaternion storedCamRot = playerFollowCamTarget.rotation;

				transform.SetPositionAndRotation(transform.position, motionRot);
				playerFollowCamTarget.SetPositionAndRotation(storedCamPos, storedCamRot);

				motion = transform.forward * (speed * Time.deltaTime);
			}

			_velocity.x = motion.x;
			_velocity.z = motion.z;

			_velocity.y -= gravity * Time.deltaTime;

			_characterController.Move(_velocity);

			if (_characterController.isGrounded) _velocity.y = 0;

			Moved?.Invoke();
		}

		/// <summary>
		/// Calls <c>Move()</c> each frame.
		/// </summary>
		private void Update() {
			if (_gameContextManager.ActiveContext is OrbitCameraManager
			 || _gameContextManager.ActiveContext is FixedCameraContextController)
				Move();
		}
	}
}