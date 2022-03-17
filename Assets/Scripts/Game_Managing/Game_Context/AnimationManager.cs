using NPC_Control;
using Other;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class AnimationManager : Singleton<AnimationManager> {
		private Animator _playerAnimator;

		private void OnEnable() { _playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>(); }

		public void SetPlayerOnLand(bool value) {
			if (value) { _playerAnimator.SetTrigger("OnLand"); } else { _playerAnimator.ResetTrigger("OnLand"); }
		}

		public void SetPlayerInAir(bool value) { _playerAnimator.SetBool("IsInAir", value); }

		public void EnterDialogueWithPlayer(NPC otherNPC) {
			_playerAnimator.SetBool("IsInDialogue", true);

			//TODO: Add animators to NPCs so this will actually work
			//otherNPC.GetComponent<Animator>()?.SetBool("IsInDialogue", true);
		}

		public void ExitDialogueWithPlayer(NPC otherNPC) {
			_playerAnimator.SetBool("IsInDialogue", false);

			//TODO: Add animators to NPCs so this will actually work
			//otherNPC.GetComponent<Animator>()?.SetBool("IsInDialogue", false);
		}
	}
}