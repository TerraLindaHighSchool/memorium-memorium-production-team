using System.ComponentModel;
using UnityEngine;

namespace Game_Managing {
	[RequireComponent(typeof(Collider))]
	public class SceneTransitionTarget : MonoBehaviour {
		[SerializeField] private SceneManager.Scene targetScene;

		private Collider _trigger;

		private void Awake() {
			_trigger = GetComponent<Collider>();
			if (!_trigger.isTrigger) throw new WarningException("ATTACHED COLLIDER IS NOT A TRIGGER");
		}

		private void OnTriggerEnter(Collider other) { SceneManager.Load(targetScene); }
	}
}