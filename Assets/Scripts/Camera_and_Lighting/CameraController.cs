using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Camera_and_Lighting {
	public class CameraController : MonoBehaviour {
		[SerializeField] private float sensitivity  = 1.0f;
		[SerializeField] private int   cameraYBound = 60;

		[SerializeField] public Transform playerFollowCamTarget;

		private bool _inOrbitMode;

		private Mouse _mouse;

		// Returns the float that is the y of the follow target's rotation eulers
		// Used for getting the forward vector of the follow target for player movement
		public float GetYRotForForwards() { return playerFollowCamTarget.eulerAngles.y; }

		private void OnEnable() { _mouse = Mouse.current; }

		private void LateUpdate() {
			Vector2 mouseDelta = _mouse.delta.ReadValue();
			_inOrbitMode = _mouse.rightButton.ReadValue() >= 1;

			if (mouseDelta == Vector2.zero || !_inOrbitMode) return;

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

			Quaternion removeZComponent =
				Quaternion.Euler(new Vector3(playerFollowTargetEulers.x, playerFollowTargetEulers.y,
				                             0));
			playerFollowCamTarget.SetPositionAndRotation(playerFollowCamTarget.position, removeZComponent);
		}
	}
}