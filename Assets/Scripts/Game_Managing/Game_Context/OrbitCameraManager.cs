using System;
using Other;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class OrbitCameraManager : Singleton<OrbitCameraManager>, IGameContext {
		[SerializeField] private float sensitivity = 0.7f;

		[SerializeField] private int cameraYBound = 60;

		private Transform _playerFollowCamTarget;

		public float     GetYRotForForwards()       { return _playerFollowCamTarget.eulerAngles.y; }
		public Transform GetPlayerFollowCamTarget() { return _playerFollowCamTarget; }

		private void Start() { _playerFollowCamTarget = GameObject.Find("LookAtTarget").transform; }

		public void GCStart() { }

		public void GCUpdate(Vector2 mouseDelta, bool rcDown) {
			if (!rcDown) return;

			mouseDelta   *= sensitivity * (1 + Time.deltaTime);
			mouseDelta.y *= -1;

			Transform playerTransform = _playerFollowCamTarget.parent;

			//Rotate around up axis by mouse x delta
			_playerFollowCamTarget.Rotate(playerTransform.up, mouseDelta.x);

			//Clamp rotation around sideways axis
			float attemptedNewCameraX = _playerFollowCamTarget.eulerAngles.x + mouseDelta.y;
			if (attemptedNewCameraX <= cameraYBound) {
				_playerFollowCamTarget.Rotate(Vector3.right, mouseDelta.y);
			} else if (attemptedNewCameraX >= 360 - cameraYBound) {
				_playerFollowCamTarget.Rotate(Vector3.right, mouseDelta.y);
			}

			Vector3 playerFollowTargetEulers = _playerFollowCamTarget.eulerAngles;

			Quaternion removeFollowTargetZComponent =
				Quaternion.Euler(new Vector3(playerFollowTargetEulers.x, playerFollowTargetEulers.y,
				                             0));
			_playerFollowCamTarget.SetPositionAndRotation(_playerFollowCamTarget.position,
			                                              removeFollowTargetZComponent);
		}

		public event Action onExit;
	}
}