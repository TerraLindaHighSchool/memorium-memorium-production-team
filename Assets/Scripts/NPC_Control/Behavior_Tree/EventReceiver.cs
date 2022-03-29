using System;
using UnityEngine;
using UnityEngine.Events;

namespace NPC_Control.Behavior_Tree {
	[Serializable]
	public class EventReceiver : ScriptableObject {
		[HideInInspector] public string guid;

		public string     key;
		public UnityEvent @event;

		public EventReceiver() {
			guid   = Guid.NewGuid().ToString();
			key    = "New Event Receiver";
			@event = new UnityEvent();
		}
	}
}