using System;
using System.ComponentModel;
using Other;
using UnityEngine;

namespace Game_Managing {
	[RequireComponent(typeof(Collider)), RequireComponent(typeof(Interactable))]
	public class SceneTransitionTarget : MonoBehaviour {
		[SerializeField] private SceneManager.Scene targetScene;

		private Collider trigger;

		private void Awake() {
			trigger = GetComponent<Collider>();
			if (!trigger.isTrigger) throw new WarningException("ATTACHED COLLIDER IS NOT A TRIGGER");
		}

		private void OnTriggerEnter(Collider other) { SceneManager.Load(targetScene); }
	}
}