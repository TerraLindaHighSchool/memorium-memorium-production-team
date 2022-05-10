using Other;
using Player_Control;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class RespawnManager : Singleton<RespawnManager> {
		private Vector3 _respawnPoint;

		public float respawnHeightThreshold = -20.0f;

		private GameObject _player;

		public void SetRespawnPoint(Vector3 newPoint) {
			_respawnPoint = newPoint;
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

		public void Respawn() {
			_player.transform.position = _respawnPoint;
			_player.GetComponent<PlayerController>().OnDeath();
		}
	}
}