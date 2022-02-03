using System;
using Other;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class OrbitCameraManager : Singleton<OrbitCameraManager>, IGameContext {
		[SerializeField] private float sensitivity = 0.7f;

		[SerializeField] private int cameraYBound = 60;

		private Transform _playerFollowCamTarget;

		public void      GCUpdatePos(Vector2   mousePos,   bool lcDown, bool rcDown) { }
		public float     GetYRotForForwards()       { return _playerFollowCamTarget.eulerAngles.y; }
		public Transform GetPlayerFollowCamTarget() { return _playerFollowCamTarget; }

		public void GCStart() { _playerFollowCamTarget = GameObject.Find("LookAtTarget").transform; }

		public void GCStart() { }
		
		public void GCExit()  { throw new NotImplementedException(); }

		public void GCUpdateDelta(Vector2 mouseDelta, bool lcDown, bool rcDown) {
			if (!rcDown || mouseDelta == Vector2.zero) return;

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
				Quaternion.Euler(new Vector3(playerFollowTargetEulers.x, playerFollowTargetEulers.y,0));
			_playerFollowCamTarget.SetPositionAndRotation(_playerFollowCamTarget.position,
			                                              removeFollowTargetZComponent);
		}

		public event Action OnExit;
	}
}