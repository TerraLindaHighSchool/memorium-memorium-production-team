using System;
using Cinemachine;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class FixedCameraContextController : MonoBehaviour, IGameContext {
		private void OnCollisionEnter(Collision other) {
			if (other.gameObject.CompareTag("Player")) GameContextManager.Instance.EnterContext(this);
		}

		private void OnCollisionExit(Collision other) {
			if (other.gameObject.CompareTag("Player")) GCExit();
		}

		public void OnPlayerEnter() { GameContextManager.Instance.EnterContext(this); }

		public void OnPlayerExit() {
			GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
			OnExit?.Invoke();
		}

		public void GCStart() { GetComponentInChildren<CinemachineVirtualCamera>().enabled = true; }

		public void GCExit() {
			GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
			OnExit?.Invoke();
		}

		public void  GCUpdatePos(Vector2 mousePos, bool lcDown, bool rcDown) { }
		public float GetYRotForForwards() { return transform.GetChild(0).eulerAngles.y; }

		public Transform GetPlayerFollowCamTarget() { return GameObject.Find("LookAtTarget").transform; }

		public void GCUpdateDelta(Vector2 mouseDelta, bool lcDown, bool rcDown) { }

		public event Action OnExit;
	}
}