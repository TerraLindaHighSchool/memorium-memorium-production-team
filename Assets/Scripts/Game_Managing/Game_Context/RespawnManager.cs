using Other;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class RespawnManager : Singleton<RespawnManager> {
		public Vector3 RespawnPoint { get; private set; }

		public float respawnHeightThreshold = -20.0f;

		private GameObject _player;

		public void SetRespawnPoint(Vector3 newPoint) {
			RespawnPoint = newPoint;
			_player      = GameObject.FindWithTag("Player");
		}

		private void OnEnable() {
			//TODO: This is only for starting on the tutorial island
			SetRespawnPoint(SceneManager.GetSceneData(SceneManager.Scene.TutorialIsland).DefaultRespawnPoint);
		}

		private void LateUpdate() {
			if (!_player) _player = GameObject.FindWithTag("Player");
			if (_player.transform.position.y <= respawnHeightThreshold) Respawn();
		}

		public void Respawn() { _player.transform.position = RespawnPoint; }
	}
}