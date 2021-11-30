using System;
using Other;

namespace Game_Managing.Pausing {
	public class PauseManager : Singleton<PauseManager> {//TODO: make accept Optional<PauseType> instead of bools, to differentiate different kinds of pausing
		public enum PauseType {
			Cutscene, All
		}
		
		private Optional<PauseType>              _paused = new Optional<PauseType>(PauseType.All, false);
		public event Action<Optional<PauseType>> OnSetPaused;

		public Optional<PauseType> Paused {
			get => _paused;
			set {
				_paused = value;
				OnSetPaused?.Invoke(value);
			}
		}
	}
}
