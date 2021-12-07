using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Camera_and_Lighting {
	public class CameraController : MonoBehaviour {
		[SerializeField] private float sensitivity  = 1.0f;
		[SerializeField] private int   cameraYBound = 60;

		[SerializeField] public Transform playerFollowCamTarget;

		private PlayerInputActions _playerInputActions;

		private Vector2 _mouseDelta;
		private bool    _inOrbitMode;

		// Returns the float that is the y of the follow target's rotation eulers
		// Used for getting the forward vector of the follow target for player movement
		public float GetYRotForForwards() { return playerFollowCamTarget.eulerAngles.y; }

		private void Start() {
			_playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			_playerInputActions.Player.MouseDelta.performed += OnMouseDelta;

			_playerInputActions.Player.Orbit.started  += OnOrbitStarted;
			_playerInputActions.Player.Orbit.canceled += OnOrbitCancelled;
		}

		private void OnMouseDelta(InputAction.CallbackContext context) {
			if (_inOrbitMode) OrbitCamera(context.ReadValue<Vector2>());
		}

		private void OnOrbitStarted(InputAction.CallbackContext   context) { _inOrbitMode = true; }
		private void OnOrbitCancelled(InputAction.CallbackContext context) { _inOrbitMode = false; }

		private void OrbitCamera(Vector2 mouseDelta) {
			mouseDelta   *= sensitivity * (1 + Time.deltaTime);
			mouseDelta.y *= -1;

			Transform playerTransform = playerFollowCamTarget.parent;

			//Rotate around up axis by mouse x delta
			playerFollowCamTarget.Rotate(playerTransform.up, mouseDelta.x);

			//Clamp rotation around sideways axis
			float attemptedNewCameraX = playerFollowCamTarget.eulerAngles.x + mouseDelta.y;
			if (attemptedNewCameraX <= cameraYBound) {
				playerFollowCamTarget.Rotate(Vector3.right, mouseDelta.y);
			} else if (attemptedNewCameraX >= 360 - cameraYBound) {
				playerFollowCamTarget.Rotate(Vector3.right, mouseDelta.y);
			}

			Vector3 playerFollowTargetEulers = playerFollowCamTarget.eulerAngles;

			Quaternion removeFollowTargetZComponent =
				Quaternion.Euler(new Vector3(playerFollowTargetEulers.x, playerFollowTargetEulers.y,
				                             0));
			playerFollowCamTarget.SetPositionAndRotation(playerFollowCamTarget.position, removeFollowTargetZComponent);
		}
	}
}