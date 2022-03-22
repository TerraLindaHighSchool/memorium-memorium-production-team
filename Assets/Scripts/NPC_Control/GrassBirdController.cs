using System;
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

		private void Awake() { npc = GetComponent<NPC>(); }

		private void Start() { StartCoroutine(MoveToRandom()); }

		public void OnPet() {
			Debug.Log($"{this} was pet");
			AnimationManager.Instance.PetGrassBird(this);
		}

		public void OnKick() {
			Debug.Log($"{this} was kicked");
			AnimationManager.Instance.KickGrassBird(this);
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