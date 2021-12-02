using Other;
using UnityEngine;

namespace Puzzle_Control.Prototype_Puzzle {
	public class PuzzlePedestal : PuzzleController {
		private bool _canComplete;

		public override void StartPuzzle() {
			if (!_canComplete) {
				Debug.Log("The requirements to do this puzzle are not met.");
				return;
			}
			
			Debug.Log("First puzzle completed!");
			Complete();
			GetComponent<Interactable>().isEnabled = false;
		}

		public void SetCanComplete(bool value) {
			_canComplete = value;
		}
	}
}