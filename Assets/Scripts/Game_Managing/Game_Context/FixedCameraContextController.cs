using System;
using Cinemachine;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class FixedCameraContextController : MonoBehaviour, IGameContext {
		private void OnCollisionEnter(Collision other) {
			Debug.Log("Entering fixed cam");
			if (other.gameObject.CompareTag("Player")) GameContextManager.Instance.EnterContext(this);
		}

		private void OnCollisionExit(Collision other) {
			Debug.Log("Exiting fixed cam");
			if (!other.gameObject.CompareTag("Player")) return;
			GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
			onExit?.Invoke();
		}
		
		public void OnPlayerEnter() { GameContextManager.Instance.EnterContext(this); }

		public void OnPlayerExit() {
			GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
			onExit?.Invoke();
		}

		public void GCStart() { GetComponentInChildren<CinemachineVirtualCamera>().enabled = true; }

		public float GetYRotForForwards() { return transform.GetChild(0).eulerAngles.y; }

		public Transform GetPlayerFollowCamTarget() { return GameObject.Find("LookAtTarget").transform; }

		public void GCUpdate(Vector2 mouseDelta, bool rcDown) { }

		public event Action onExit;
	}
}