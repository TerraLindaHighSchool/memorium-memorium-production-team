using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game_Managing.Game_Context.Cutscene {
	[Serializable]
	public class TimedCutsceneEvent : ScriptableObject {
		[HideInInspector] public string guid;
		
		[HideInInspector] public bool hasFired;

		public string     label;
		public float      time;
		public UnityEvent unityEvent;

		public TimedCutsceneEvent() {
			guid       = Guid.NewGuid().ToString();
			label      = "New Cutscene Event";
			time       = 0.0f;
			unityEvent = new UnityEvent();
			hasFired   = false;
		}
	}
}