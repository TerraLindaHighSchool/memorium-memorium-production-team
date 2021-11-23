using System;
using Other;
using UnityEngine;

namespace Game_Managing.Pausing {
	public class PauseManager : Singleton<PauseManager> {
		public enum PauseType {
			Cutscene, All
		}
		
		private bool              _paused = false;
		public event Action<bool> OnSetPaused;

		public bool Paused {
			get => _paused;
			set {
				_paused = value;
				OnSetPaused?.Invoke(value);
			}
		}
	}
}
