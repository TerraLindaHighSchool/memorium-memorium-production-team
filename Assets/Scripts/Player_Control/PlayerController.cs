using System;
using Game_Managing.Game_Context;
using NPC_Control.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Control {
	/// <summary>
	/// MonoBehaviour that lives on the <c>Player</c> prefab and controls many player-related things. 
	/// Handles player movement, and for the moment handles all input in the game with <c>PlayerInputActions</c>. 
	/// </summary>
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Animator))]
	public class PlayerController : MonoBehaviour {
		///Player speed multiplier. 
		public float speed;

		///Player jump height multiplier.
		public float jump;

		///Player gravity multiplier.
		public float gravity;

		///Player maximum interaction distance.
		public float interactDistance;

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

		///Boolean to keep track of whether the player was grounded last frame
		private bool _wasGroundedLastFrame;

		///float to keep track of how long the player has been falling
		private float _timeFalling;

		private int _minTimeFalling = 20;
		
		//Boolean to keep track of whether or not you can jump
		private bool _isCoyoteTime;

		//Number of frames CoyoteTime works
		private int _coyoteTimer = 20;

		///Boolean to track whether or not the player has jumped
		public bool hasJumped;

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
		/// The Animation Manager, for setting animations.
		/// </summary>
		private AnimationManager _animationManager;

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
			_animationManager   = AnimationManager.Instance;

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
		/// Rotates the player to face towards a certain point, i.e. an NPC for dialogue.
		/// </summary>
		/// <param name="point">The point to face towards.</param>
		public void FaceTowards(Vector3 point) {
			Vector3 heightNormalizedPoint = new Vector3(point.x, transform.position.y, point.z);
			
			transform.LookAt(heightNormalizedPoint);
			
			transform.Rotate(transform.up, -90f);
		}

		/// <summary>
		/// If the player is on the ground, adds the jump force to the vertical velocity. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>Jump.performed</c> event.</param>
		private void OnJump(InputAction.CallbackContext context) {
			if ((_characterController.isGrounded || _isCoyoteTime)
			 && (_gameContextManager.ActiveContext is OrbitCameraManager
			  || _gameContextManager.ActiveContext is FixedCameraContextController)) {
				_velocity.y += jump;
				_animationManager.SetPlayerOnLand(false);
				hasJumped = true;
			}
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

				motion = transform.right * (speed * Time.deltaTime);
				
				_animationManager.SetPlayerRunning(true);
			} else {
				_animationManager.SetPlayerRunning(false);
			}

			_velocity.x = motion.x;
			_velocity.z = motion.z;

			_velocity.y -= gravity * Time.deltaTime;

			_characterController.Move(_velocity);

			if (_characterController.isGrounded) {
				_velocity.y = 0;
				_animationManager.SetPlayerInAir(false);
			} else 
				{ 
					if(_timeFalling >= _minTimeFalling)
					{
						_animationManager.SetPlayerInAir(true);
					}
						 
				}

			if (_characterController.isGrounded && !_wasGroundedLastFrame) { _animationManager.SetPlayerOnLand(true); }

			Moved?.Invoke();

			_wasGroundedLastFrame = _characterController.isGrounded;
		}

		/// <summary>
		/// Checks how long the player has been falling and resets whenever the player touches the ground
		/// </summary>
		private void CoyoteTime()
		{
			if(!_wasGroundedLastFrame)
			{ 
				_timeFalling++; 
			}
			else {
				_timeFalling = 0;
				if(_wasGroundedLastFrame & _timeFalling == 0)
				{
					hasJumped = false;
				}
			}

			if(_timeFalling >= _coyoteTimer)
			{
				_isCoyoteTime = false;
			}
			else
			{
				if(!hasJumped)
				{
					_isCoyoteTime = true;
				}
				else
				{
					_isCoyoteTime = false;
				}
			}
		}

		/// <summary>
		/// Calls <c>Move()</c> each frame.
		/// </summary>
		private void Update() {
			if (_gameContextManager.ActiveContext is OrbitCameraManager
			 || _gameContextManager.ActiveContext is FixedCameraContextController)
				Move();

		}

		/// Calls <c>CoyoteTime()</c> at a fixed framerate.
		private void FixedUpdate()
		{
			CoyoteTime();
		}
	}
}