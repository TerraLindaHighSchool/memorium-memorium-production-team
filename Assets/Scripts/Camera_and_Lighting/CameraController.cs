using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Camera_and_Lighting {
	/// <summary>
	/// Lives on the Player VCAM and controls the camera. 
	/// Manages the orbit camera code around the player. 
	/// In the future may also manage switching between VCAMs.
	/// </summary>
	public class CameraController : MonoBehaviour {
		///Orbit camera sensitivity setting, set in the editor or by settings menu.
		[SerializeField] private float sensitivity = 1.0f;

		///Orbit camera vertical angle boundary, set in the editor or by settings menu.
		[SerializeField] private int cameraYBound = 60;

		///The empty on the player that gets rotated to make the orbit camera.
		[SerializeField] public Transform playerFollowCamTarget;

		///Keeps track of the mouse movement delta from frame to frame. 
		private Vector2 _mouseDelta;

		///Controls whether or not the camera is in "orbit mode", controlled by right click.
		private bool _inOrbitMode;

		///Returns the float that is the y of the follow target's rotation eulers. 
		///Used for getting the forward vector of the follow target for player movement. 
		public float GetYRotForForwards() { return playerFollowCamTarget.eulerAngles.y; }

		///Gets the PlayerInputActions from the player and subscribes to the necessary events. 
		private void Start() {
			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.MouseDelta.performed += OnMouseDelta;

			playerInputActions.Player.Orbit.started  += OnOrbitStarted;
			playerInputActions.Player.Orbit.canceled += OnOrbitCancelled;
		}

		/// <summary>
		/// Orbits the camera if necessary whenever the mouse delta changes. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>MouseDelta.performed</c> event.</param>
		private void OnMouseDelta(InputAction.CallbackContext context) {
			if (_inOrbitMode) OrbitCamera(context.ReadValue<Vector2>());
		}

		/// <summary>
		/// Sets <c>_inOrbitMode</c> to the state of the right click whenever it changes. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>Orbit.started</c> event.</param>
		private void OnOrbitStarted(InputAction.CallbackContext context) { _inOrbitMode = true; }

		/// <summary>
		/// Sets <c>_inOrbitMode</c> to the state of the right click whenever it changes. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>Orbit.canceled</c> event.</param>
		private void OnOrbitCancelled(InputAction.CallbackContext context) { _inOrbitMode = false; }

		/// <summary>
		/// Uses the passed in mouse delta to orbit the camera around the player, within the set bounds.
		/// </summary>
		/// <param name="mouseDelta">A Vector2 for the x,y of the mouse movement delta this frame.</param>
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