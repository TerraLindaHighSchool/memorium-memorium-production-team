using System.ComponentModel;
using NPC_Control.Dialogue;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game_Managing {
	[RequireComponent(typeof(Collider))]
	public class SceneTransitionTarget : MonoBehaviour {
		[SerializeField] private SceneManager.Scene targetScene;

		[SerializeField] private bool active = true;

		private Collider _trigger;

		private void Awake() {
			_trigger = GetComponent<Collider>();
			if (!_trigger.isTrigger) throw new WarningException("ATTACHED COLLIDER IS NOT A TRIGGER");
		}

		public void SetActive(bool value) { active = value; }

		private void OnTriggerEnter(Collider other) {
			if (active) SceneManager.Load(targetScene);
		}
	}
}