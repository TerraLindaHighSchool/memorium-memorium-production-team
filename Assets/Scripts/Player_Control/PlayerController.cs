using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Control {
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerController : MonoBehaviour {
		public float speed = 1f;

		private CharacterController _characterController;
		private PlayerInput         _playerInput;

		private (bool, bool, bool, bool) _wasd;

		void Start() {
			_characterController = GetComponent<CharacterController>();
			_playerInput         = GetComponent<PlayerInput>();

			PlayerInputActions playerInputActions = new PlayerInputActions();
			playerInputActions.Enable();

			playerInputActions.Player.W.started  += OnWStarted;
			playerInputActions.Player.W.canceled += OnWCancelled;
			playerInputActions.Player.A.started  += OnAStarted;
			playerInputActions.Player.A.canceled += OnACancelled;
			playerInputActions.Player.S.started  += OnSStarted;
			playerInputActions.Player.S.canceled += OnSCancelled;
			playerInputActions.Player.D.started  += OnDStarted;
			playerInputActions.Player.D.canceled += OnDCancelled;
		}

		private void OnWStarted(InputAction.CallbackContext   context) { _wasd.Item1 = true; }
		private void OnWCancelled(InputAction.CallbackContext context) { _wasd.Item1 = false; }
		private void OnAStarted(InputAction.CallbackContext   context) { _wasd.Item2 = true; }
		private void OnACancelled(InputAction.CallbackContext context) { _wasd.Item2 = false; }
		private void OnSStarted(InputAction.CallbackContext   context) { _wasd.Item3 = true; }
		private void OnSCancelled(InputAction.CallbackContext context) { _wasd.Item3 = false; }
		private void OnDStarted(InputAction.CallbackContext   context) { _wasd.Item4 = true; }
		private void OnDCancelled(InputAction.CallbackContext context) { _wasd.Item4 = false; }

		private void Move() {
			Vector3 motion = new Vector3();

			if (_wasd.Item1) { motion += transform.forward; }

			if (_wasd.Item2) { motion += -transform.right; }

			if (_wasd.Item3) { motion += -transform.forward; }

			if (_wasd.Item4) { motion += transform.right; }

			motion.Normalize();

			_characterController.Move(motion * speed * Time.deltaTime);
		}

		void Update() { Move(); }
	}
}