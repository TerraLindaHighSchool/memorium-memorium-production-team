using System;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle_Control {
	public abstract class PuzzleController : MonoBehaviour {
		[SerializeField] UnityEvent OnPuzzleComplete;

		public string Guid { get; private set; }

		public event Action<string> NotifyPuzzleSet; //Tell the contained puzzle set the puzzle is complete
		
		public abstract void StartPuzzle();

		void OnEnable() {
			Guid = System.Guid.NewGuid().ToString();
		}

		protected void Complete() {
			OnPuzzleComplete.Invoke();
			NotifyPuzzleSet?.Invoke(Guid);
		}

	}
}