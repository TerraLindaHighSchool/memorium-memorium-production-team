using Camera_and_Lighting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Control {
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerController : MonoBehaviour {
		public float speed            = 10f;
		public float jump             = 1f;
		public float gravity          = 1f;
		public float interactDistance = 5f;

		public PlayerInputActions PlayerInputActions;

		[SerializeField] private CameraController cameraController;

		private Vector3 _momentum;

		private readonly bool[] _wasd = new bool[4];

		private CharacterController _characterController;

		private void OnEnable() {
			_characterController = GetComponent<CharacterController>();

			PlayerInputActions = new PlayerInputActions();
			PlayerInputActions.Enable();

			PlayerInputActions.Player.W.started  += OnWStarted;
			PlayerInputActions.Player.W.canceled += OnWCancelled;
			PlayerInputActions.Player.A.started  += OnAStarted;
			PlayerInputActions.Player.A.canceled += OnACancelled;
			PlayerInputActions.Player.S.started  += OnSStarted;
			PlayerInputActions.Player.S.canceled += OnSCancelled;
			PlayerInputActions.Player.D.started  += OnDStarted;
			PlayerInputActions.Player.D.canceled += OnDCancelled;

			PlayerInputActions.Player.Jump.performed += OnJump;
		}

		public float GetDistanceToObject(GameObject obj) {
			return Vector3.Distance(transform.position, obj.transform.position);
		}


		private void OnJump(InputAction.CallbackContext context) {
			if (_characterController.isGrounded) { _momentum.y += jump; }
		}

		private void OnWStarted(InputAction.CallbackContext   context) { _wasd[0] = true; }
		private void OnWCancelled(InputAction.CallbackContext context) { _wasd[0] = false; }
		private void OnAStarted(InputAction.CallbackContext   context) { _wasd[1] = true; }
		private void OnACancelled(InputAction.CallbackContext context) { _wasd[1] = false; }
		private void OnSStarted(InputAction.CallbackContext   context) { _wasd[2] = true; }
		private void OnSCancelled(InputAction.CallbackContext context) { _wasd[2] = false; }
		private void OnDStarted(InputAction.CallbackContext   context) { _wasd[3] = true; }
		private void OnDCancelled(InputAction.CallbackContext context) { _wasd[3] = false; }

		private void Move() {
			Vector3 motion = new Vector3();

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
					Vector3    storedCamTargetPos = cameraController.playerFollowCamTarget.position;
					Quaternion storedCamTargetRot = cameraController.playerFollowCamTarget.rotation;

					transform.SetPositionAndRotation(transform.position,
					                                 Quaternion.Euler(
						                                 new Vector3(eulers.x, cameraController.GetYRotForForwards(),
						                                             eulers.z)));

					cameraController.playerFollowCamTarget.SetPositionAndRotation(
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

				Vector3    storedCamPos = cameraController.playerFollowCamTarget.position;
				Quaternion storedCamRot = cameraController.playerFollowCamTarget.rotation;

				transform.SetPositionAndRotation(transform.position, motionRot);
				cameraController.playerFollowCamTarget.SetPositionAndRotation(storedCamPos, storedCamRot);

				motion = transform.forward * (speed * Time.deltaTime);
			}

			_momentum.x = motion.x;
			_momentum.z = motion.z;

			_momentum.y -= gravity * (1 + Time.deltaTime);

			_characterController.Move(_momentum);

			if (_characterController.isGrounded) _momentum.y = 0;
		}

		private void Update() { Move(); }
	}
}