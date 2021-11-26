using Camera_and_Lighting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Control {
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerController : MonoBehaviour {
		public float speed = 10f;
		
		[SerializeField] private CameraController cameraController;

		private CharacterController _characterController;

		private (bool, bool, bool, bool) _wasd;

		private void Start() {
			_characterController = GetComponent<CharacterController>();

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
			if (!_wasd.Item1 && !_wasd.Item2 && !_wasd.Item3 && !_wasd.Item4) return;
			
			Vector3 eulers = transform.eulerAngles;

			{
				Vector3    storedCamTargetPos = cameraController.playerFollowCamTarget.position;
				Quaternion storedCamTargetRot = cameraController.playerFollowCamTarget.rotation;
			
				transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(eulers.x, cameraController.GetYRotForForwards(), eulers.z)));
			
				cameraController.playerFollowCamTarget.SetPositionAndRotation(storedCamTargetPos, storedCamTargetRot);
			}
			
			Vector3 motion = new Vector3();

			if (_wasd.Item1) { motion += transform.forward; }

			if (_wasd.Item2) { motion += -transform.right; }

			if (_wasd.Item3) { motion += -transform.forward; }

			if (_wasd.Item4) { motion += transform.right; }

			motion.Normalize();

			_characterController.Move(motion * (speed * Time.deltaTime));
		}

		private void Update() { Move(); }
	}
}