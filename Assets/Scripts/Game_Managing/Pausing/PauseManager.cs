using System;
using Other;

namespace Game_Managing.Pausing {
	
	/// <summary>
	/// A global singleton that manages the pause state of the game.
	/// </summary>
	public class PauseManager : Singleton<PauseManager> {//TODO: make accept Optional<PauseType> instead of bools, to differentiate different kinds of pausing
		public enum PauseType {
			Cutscene, All
		}
		
		/// <summary>
		/// The current pause status. Is an optional because it has to represent three states -
		/// absolute pause, cutscene pause, and not paused.
		/// </summary>
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
