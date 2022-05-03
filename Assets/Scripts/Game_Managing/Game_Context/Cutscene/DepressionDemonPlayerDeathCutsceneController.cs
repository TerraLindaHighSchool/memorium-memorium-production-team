using UnityEngine;

namespace Game_Managing.Game_Context.Cutscene {
	public class DepressionDemonPlayerDeathCutsceneController : MonoBehaviour {
		public void OnDeath() { RespawnManager.Instance.Respawn(); }
	}
}