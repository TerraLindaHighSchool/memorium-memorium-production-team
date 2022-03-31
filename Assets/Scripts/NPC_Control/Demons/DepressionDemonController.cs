using Game_Managing.Game_Context;
using UnityEngine;

namespace NPC_Control.Demons {
	public class DepressionDemonController : MonoBehaviour {
		public void OnDeath() {
			AnimationManager.Instance.NPCOnDeath(GetComponent<NPC>());
		}
	}
}