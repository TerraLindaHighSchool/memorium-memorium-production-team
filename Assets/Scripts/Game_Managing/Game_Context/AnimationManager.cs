using NPC_Control;
using Other;
using Player_Control;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class AnimationManager : Singleton<AnimationManager> {
		private PlayerController _playerController;
		private Animator         _playerAnimator;
		public float _runMultiplier;

		private void OnEnable() {
			_playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			_playerAnimator   = _playerController.GetComponent<Animator>();
		}

		private void CheckFields() {
			if (!_playerController || !_playerAnimator) OnEnable();
		}

		public void SetPlayerOnLand(bool value) {
			CheckFields();
			
			if (value) { _playerAnimator.SetTrigger("OnLand"); } else { _playerAnimator.ResetTrigger("OnLand"); }
		}

		public void SetPlayerInAir(bool value) {
			CheckFields();
			
			_playerAnimator.SetBool("IsInAir", value);
		}

		public void SetPlayerRunning(bool value) {
			CheckFields();
			
			_playerAnimator.SetBool("IsRunning", value);
		}

		public void PetGrassBird(GrassBirdController grassBird) {
			grassBird.GetComponent<Animator>().ResetTrigger("OnKick");
			grassBird.GetComponent<Animator>().SetTrigger("OnPet");
		}

		public void KickGrassBird(GrassBirdController grassBird) {
			grassBird.GetComponent<Animator>().ResetTrigger("OnPet");
			grassBird.GetComponent<Animator>().SetTrigger("OnKick");
		}

		public void NPCOnDeath(NPC npc) {
			npc.GetComponent<Animator>().SetTrigger("OnDeath");
		}

		public void EnterDialogueWithPlayer(NPC otherNPC) {
			CheckFields();
			
			_playerAnimator.SetBool("IsInDialogue", true);
			_playerController.FaceTowards(otherNPC.transform.position);

			//TODO: Add animators to NPCs so this will actually work
			otherNPC.GetComponent<Animator>()?.SetBool("IsInDialogue", true);
		}

		public void ExitDialogueWithPlayer(NPC otherNPC) {
			CheckFields();
			
			_playerAnimator.SetBool("IsInDialogue", false);

			//TODO: Add animators to NPCs so this will actually work
			otherNPC.GetComponent<Animator>()?.SetBool("IsInDialogue", false);
		}

		public void SetAnimatorRunning(Animator animator, bool value) {
			animator.SetBool("IsRunning", value);
		}
	}
}