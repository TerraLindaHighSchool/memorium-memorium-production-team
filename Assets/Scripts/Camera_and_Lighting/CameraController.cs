using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera_and_Lighting {
	/*
	 * This MonoBehaviour lives on the Player VCAM
	 * It controls the orbit functionality for the camera
	 * In the future may also control switching between VCAMS for cutscenes
	 */
	public class CameraController : MonoBehaviour {
		// Orbit camera settings, set in the editor or by settings
		[SerializeField] private float sensitivity  = 1.0f;
		[SerializeField] private int   cameraYBound = 60;

		// The empty on the player that gets rotated to make the orbit camera
		[SerializeField] public Transform playerFollowCamTarget;

		// A reference to the player, used primarily to get the PlayerInputActions object
		[SerializeField] private PlayerController player;

		// Fields for keeping track of mouse data from the input actions
		private Vector2 _mouseDelta;
		private bool    _inOrbitMode;

		// Returns the float that is the y of the follow target's rotation eulers
		// Used for getting the forward vector of the follow target for player movement
		public float GetYRotForForwards() { return playerFollowCamTarget.eulerAngles.y; }

		// Gets the PlayerInputActions from the player and subscribes to the necessary events
		private void Start() {
			PlayerInputActions playerInputActions = player.PlayerInputActions;

			playerInputActions.Player.MouseDelta.performed += OnMouseDelta;

			playerInputActions.Player.Orbit.started  += OnOrbitStarted;
			playerInputActions.Player.Orbit.canceled += OnOrbitCancelled;
		}

		// Orbits the camera if necessary whenever the mouse delta changes
		private void OnMouseDelta(InputAction.CallbackContext context) {
			if (_inOrbitMode) OrbitCamera(context.ReadValue<Vector2>());
		}

		// Sets _inOrbitMode whenever the right mouse button changes
		private void OnOrbitStarted(InputAction.CallbackContext   context) { _inOrbitMode = true; }
		private void OnOrbitCancelled(InputAction.CallbackContext context) { _inOrbitMode = false; }

		// Uses the passed in mouse delta to orbit the camera around the player, within the set bounds
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