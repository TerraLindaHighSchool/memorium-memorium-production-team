using Game_Managing;
using Game_Managing.Game_Context;
using UnityEngine;

namespace NPC_Control.Demons {
	public class DepressionDemonController : MonoBehaviour {
		private AudioSource _audioSource;

		private static AudioClip _fidgetSound;
		private static AudioClip _idleSound;
		private static AudioClip _talk1;
		private static AudioClip _talk2;

		public void OnDeath() {
			SaveManager.Instance.flowerShard = true;
			AnimationManager.Instance.NPCOnDeath(GetComponent<NPC>());
		}

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();

			AssignAudioClips();
		}

		private void AssignAudioClips()
		{
			_fidgetSound = Resources.Load<AudioClip>("Audio/Sounds/Character/Tristitia/Tristitia_Fidget");
			_idleSound = Resources.Load<AudioClip>("Audio/Sounds/Character/Tristitia/Tristitia_Idle");
			_talk1 = Resources.Load<AudioClip>("Audio/Sounds/Character/Tristitia/TristitiaTalk_1");
			_talk2 = Resources.Load<AudioClip>("Audio/Sounds/Character/Tristitia/TristitiaTalk_2");
		}

		public void PlayFidgetSound()
		{
			_audioSource.PlayOneShot(_fidgetSound);
		}

		public void PlayIdleSound()
		{
			_audioSource.PlayOneShot(_idleSound);
		}

		public void PlayTalk1Sound()
		{
			_audioSource.PlayOneShot(_talk1);
		}

		public void PlayTalk2Sound()
		{
			_audioSource.PlayOneShot(_talk2);
		}
	}
}