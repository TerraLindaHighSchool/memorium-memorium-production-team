using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Game_Managing.Pausing;
using Other;
using UnityEditor;
using UnityEngine;

namespace Game_Managing.Game_Context.Cutscene {
	public class CutsceneContextController : MonoBehaviour, IGameContext {
		public List<TimedCutsceneEvent> events;

		[Min(1.0f)] public float lengthTime = 5.0f;

		public string key = "New Cutscene";

		public bool usePath;

		private CinemachineVirtualCamera _vcam;
		private CinemachineTrackedDolly  _dolly;

		private float _startTime;

		public void StartCutscene() { GameContextManager.Instance.EnterContext(this); }

		public void GCStart() {
			_vcam = GetComponentInChildren<CinemachineVirtualCamera>();

			if (usePath) {
				_dolly                = _vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
				_dolly.m_PathPosition = 0.0f;
			}

			foreach (TimedCutsceneEvent timedEvent in events) { timedEvent.hasFired = false; }

			_vcam.enabled = true;

			_startTime = Time.time;

			PauseManager.Instance.Paused = new Optional<PauseManager.PauseType>(PauseManager.PauseType.Cutscene, true);
		}

		public void GCExit() {
			_vcam.enabled = false;

			PauseManager.Instance.Paused = new Optional<PauseManager.PauseType>(PauseManager.PauseType.Cutscene, false);

			OnExit?.Invoke();
		}

		public void GCUpdate(Vector2 mouseDelta, bool rcDown) {
			float timeSinceStarted = Time.time - _startTime;

			if (timeSinceStarted <= lengthTime) {
				//Scaled so there will always be a 1 second lag time at the end
				//If the cutscene uses a path, then this will be after it finishes moving
				float scaledTime = timeSinceStarted / (lengthTime - 1);

				foreach (TimedCutsceneEvent timedEvent in events) {
					if (timedEvent.time <= timeSinceStarted && !timedEvent.hasFired) {
						timedEvent.unityEvent?.Invoke();
						timedEvent.hasFired = true;
					}
				}

				if (usePath) _dolly.m_PathPosition = scaledTime;
			} else { GCExit(); }
		}

		public TimedCutsceneEvent AddTimedEvent() {
			if (events == null) events = new List<TimedCutsceneEvent>();

			TimedCutsceneEvent newEvent = ScriptableObject.CreateInstance<TimedCutsceneEvent>();

			events.Add(newEvent);

			AssetDatabase.SaveAssets();

			return newEvent;
		}

		public void RemoveTimedEvent() {
			if (events == null) {
				events = new List<TimedCutsceneEvent>();
				return;
			}

			TimedCutsceneEvent lastEvent = events.Last();

			events.Remove(lastEvent);

			AssetDatabase.SaveAssets();
		}

		public float GetYRotForForwards() { throw new NotImplementedException(); }

		public Transform GetPlayerFollowCamTarget() { throw new NotImplementedException(); }

		public event Action OnExit;
	}
}