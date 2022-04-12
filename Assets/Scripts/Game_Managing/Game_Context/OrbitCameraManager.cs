using System;
using Other;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class OrbitCameraManager : Singleton<OrbitCameraManager>, IGameContext {
		[SerializeField] private float sensitivity = 2f;

		[SerializeField] private int cameraYBound = 60;

		[SerializeField] private Transform playerFollowCamTarget;

		public void  GCUpdatePos(Vector2 mousePos, bool lcDown, bool rcDown) { }
		public float GetYRotForForwards() { return playerFollowCamTarget.eulerAngles.y - 90f; }

		public Transform GetPlayerFollowCamTarget() {
			if (!playerFollowCamTarget) playerFollowCamTarget = GameObject.Find("LookAtTarget").transform;
			return playerFollowCamTarget;
		}

		public void GCStart() { playerFollowCamTarget = GameObject.Find("LookAtTarget").transform; }

		public void GCUpdateDelta(Vector2 mouseDelta, bool lcDown, bool rcDown) {
			if (!rcDown || mouseDelta == Vector2.zero) return;

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
				Quaternion.Euler(new Vector3(playerFollowTargetEulers.x, playerFollowTargetEulers.y, 0));
			playerFollowCamTarget.SetPositionAndRotation(playerFollowCamTarget.position,
			                                             removeFollowTargetZComponent);
		}

		public event Action OnExit;
	}
}
