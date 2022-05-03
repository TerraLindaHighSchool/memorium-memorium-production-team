using System.Collections;
using Game_Managing.Game_Context;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC_Control {
	[RequireComponent(typeof(EntityController))]
	public class GrassBirdController : MonoBehaviour {
		public EntityController entityController;

		[SerializeField] private Vector3 origin;
		[SerializeField] private Vector3 boxSize     = new Vector3(5, 5, 5);
		[SerializeField] private bool    lockOnYAxis = true;

		[SerializeField] private float maxWait = 10;
		[SerializeField] private float minWait = 2;

		public NPC npc;

		private bool canMove = true;

		private AudioSource _audioSource;

		// Grass Bird Audio clips
		private static AudioClip _idleSound;
		private static AudioClip _footstepSound;
		private static AudioClip _talk1Sound;
		private static AudioClip _talk2Sound;
		private static AudioClip _petSound;
		private static AudioClip _kickedSound;

		private void Awake() {
			npc          = GetComponent<NPC>();
			_audioSource = GetComponent<AudioSource>();

			AssignAudioClips();
		}

		private void Start() { StartCoroutine(MoveToRandom()); }

		public void OnPet() {
			Debug.Log($"{this} was pet");
			AnimationManager.Instance.PetGrassBird(this);
		}

		public void OnKick() {
			Debug.Log($"{this} was kicked");
			AnimationManager.Instance.KickGrassBird(this);
		}

		private void AssignAudioClips() {
			_idleSound     = Resources.Load<AudioClip>("Audio/Sounds/Character/GrassBird/GrassBirdIdle");
			_footstepSound = Resources.Load<AudioClip>("Audio/Sounds/Character/GrassBird/Grassbird_Footstep");
			_talk1Sound    = Resources.Load<AudioClip>("Audio/Sounds/Character/GrassBird/GrassBirdConversation");
			_talk2Sound    = Resources.Load<AudioClip>("Audio/Sounds/Character/GrassBird/GrassBirdConversation_2");
			_petSound      = Resources.Load<AudioClip>("Audio/Sounds/Character/GrassBird/GrassBirdFidgetPet");
			_kickedSound   = Resources.Load<AudioClip>("Audio/Sounds/Character/GrassBird/GrassBirdKicked");
		}

		private Vector3 GetRandomPoint() {
			float x = Random.Range(-boxSize.x, boxSize.x);
			float y = lockOnYAxis ? 0 : Random.Range(-boxSize.y, boxSize.y);
			float z = Random.Range(-boxSize.z, boxSize.z);
			return new Vector3(x, y, z) + origin;
		}

		private IEnumerator MoveToRandom() {
			while (canMove) {
				if (!npc.DialogueActive) entityController.MoveToPoint(GetRandomPoint());
				yield return new WaitForSeconds(Random.Range(minWait, maxWait));
			}
		}
	}
}