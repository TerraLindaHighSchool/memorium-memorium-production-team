using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game_Managing.Game_Context.Cutscene {
	[Serializable]
	public class TimedCutsceneEvent : ScriptableObject {
		[HideInInspector] public string guid;
		
		[HideInInspector] public bool hasFired;

		public                                      string     label;
		public                                      float      time;
		[FormerlySerializedAs("unityEvent")] public UnityEvent @event;

		public TimedCutsceneEvent() {
			guid       = Guid.NewGuid().ToString();
			label      = "New Cutscene Event";
			time       = 0.0f;
			@event = new UnityEvent();
			hasFired   = false;
		}
	}
}