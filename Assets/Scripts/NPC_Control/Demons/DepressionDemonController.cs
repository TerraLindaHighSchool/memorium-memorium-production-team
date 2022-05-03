using Game_Managing;
using Game_Managing.Game_Context;
using UnityEngine;

namespace NPC_Control.Demons {
	public class DepressionDemonController : MonoBehaviour {
		public void OnDeath() {
			SaveManager.Instance.flowerShard = true;
			AnimationManager.Instance.NPCOnDeath(GetComponent<NPC>());
		}
	}
}