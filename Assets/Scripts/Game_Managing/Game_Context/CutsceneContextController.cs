using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Game_Managing.Game_Context {
	public class CutsceneContextController : MonoBehaviour, IGameContext {
		public Dictionary<string, TimedCutsceneEvent> events;

		[Min(1.0f)] public float time = 5.0f;

		public bool usePath;

		private CinemachineVirtualCamera _vcam;
		private CinemachineTrackedDolly  _dolly;

		private float _startTime;

		private void OnEnable() { events = new Dictionary<string, TimedCutsceneEvent>(); }

		public void StartCutscene() { GameContextManager.Instance.EnterContext(this); }

		public void GCStart() {
			_vcam = GetComponentInChildren<CinemachineVirtualCamera>();

			if (usePath) {
				_dolly                = _vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
				_dolly.m_PathPosition = 0.0f;
			}

			_vcam.enabled = true;

			_startTime = Time.time;
		}

		public void GCUpdate(Vector2 mouseDelta, bool rcDown) {
			float timeSinceStarted = Time.time - _startTime;

			if (timeSinceStarted <= time) {
				//Scaled so there will always be a 1 second lag time at the end
				//If the cutscene uses a path, then this will be after it finishes moving
				float scaledTime = timeSinceStarted / (time - 1);

				if (usePath) _dolly.m_PathPosition = scaledTime;
			} else {
				_vcam.enabled = false;

				OnExit?.Invoke();
			}
		}

		public TimedCutsceneEvent AddTimedEvent() {
			if (events == null) events = new Dictionary<string, TimedCutsceneEvent>();

			TimedCutsceneEvent newEvent = ScriptableObject.CreateInstance<TimedCutsceneEvent>();
			newEvent.guid = Guid.NewGuid().ToString();

			events.Add(newEvent.guid, newEvent);
			
			AssetDatabase.SaveAssets();
			
			Debug.Log($"{this} added a timed event {newEvent}");
			return newEvent;
		}

		public void RemoveTimedEvent() {
			if (events == null) {
				events = new Dictionary<string, TimedCutsceneEvent>();
				return;
			}

			KeyValuePair<string, TimedCutsceneEvent> lastKVP = events.Last();

			events.Remove(events.Last().Key);
			
			AssetDatabase.SaveAssets();
			
			Debug.Log($"{this} removed a timed event {lastKVP.Value}");
		}

		public float GetYRotForForwards() { throw new NotImplementedException(); }

		public Transform GetPlayerFollowCamTarget() { throw new NotImplementedException(); }

		public event Action OnExit;
	}

	[Serializable]
	public class TimedCutsceneEvent : ScriptableObject {
		[HideInInspector] public string guid;

		public string     label;
		public float      time;
		public UnityEvent unityEvent;
	}
}