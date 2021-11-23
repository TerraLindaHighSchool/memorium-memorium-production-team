using System;
using Other;

namespace Game_Managing.Pausing {
	public class PauseManager : Singleton<PauseManager> {//TODO: make accept Optional<PauseType> instead of bools, to differentiate different kinds of pausing
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
