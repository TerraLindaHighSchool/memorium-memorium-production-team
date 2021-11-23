using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Control {
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerController : MonoBehaviour {
		public float speed = 1f;
		
		private CharacterController _characterController;
		private PlayerInput _playerInput;

		private (bool, bool, bool, bool) _wasd;

		void Start() {
			_characterController = GetComponent<CharacterController>();
			_playerInput         = GetComponent<PlayerInput>();

			PlayerInputActions playerInputActions = new PlayerInputActions();
			playerInputActions.Enable();
			playerInputActions.Player.Move.started += OnMoveStarted;
			playerInputActions.Player.Move.canceled += OnMoveCancelled;
		}

		private void OnMoveStarted(InputAction.CallbackContext context) {
			Debug.Log($"{context.control} started");
			switch (context.control.ToString()) {
				case "Key:/Keyboard/w":
					_wasd.Item1 = true;
					break;
				case "Key:/Keyboard/upArrow":
					_wasd.Item1 = true;
					break;
				case "Key:/Keyboard/a":
					_wasd.Item2 = true;
					break;
				case "Key:/Keyboard/leftArrow":
					_wasd.Item2 = true;
					break;
				case "Key:/Keyboard/s":
					_wasd.Item3 = true;
					break;
				case "Key:/Keyboard/downArrow": 
					_wasd.Item3 = true;
					break;
				case "Key:/Keyboard/d":
					_wasd.Item4 = true;
					break;
				case "Key:/Keyboard/rightArrow":
					_wasd.Item4 = true;
					break;
				default:
					Debug.LogWarning("Unrecognized move action");
					break;
			}
		}

		private void OnMoveCancelled(InputAction.CallbackContext context) {
			Debug.Log($"{context.control} cancelled");
			switch (context.control.ToString()) {
				case "Key:/Keyboard/w":
					_wasd.Item1 = false;
					break;
				case "Key:/Keyboard/upArrow":
					_wasd.Item1 = false;
					break;
				case "Key:/Keyboard/a":
					_wasd.Item2 = false;
					break;
				case "Key:/Keyboard/leftArrow":
					_wasd.Item2 = false;
					break;
				case "Key:/Keyboard/s":
					_wasd.Item3 = false;
					break;
				case "Key:/Keyboard/downArrow": 
					_wasd.Item3 = false;
					break;
				case "Key:/Keyboard/d":
					_wasd.Item4 = false;
					break;
				case "Key:/Keyboard/rightArrow":
					_wasd.Item4 = false;
					break;
				default:
					Debug.LogWarning("Unrecognized move action");
					break;
			}
		}
		
		private void Move() {
			Vector3 motion = new Vector3();

			if (_wasd.Item1) { motion += transform.forward; }

			if (_wasd.Item2) { motion += -transform.right; }

			if (_wasd.Item3) { motion += -transform.forward; }

			if (_wasd.Item4) { motion += transform.right; }
			
			motion.Normalize();
			
			_characterController.Move(motion * speed * Time.deltaTime);
		}

		void Update() {
			Move();
		}
	}
}