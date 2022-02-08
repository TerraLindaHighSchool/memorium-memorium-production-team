using Other;
using UnityEngine;

namespace Puzzle_Control.Stained_Glass_Assembly_Puzzle {
	public class AssemblyPedestal : PuzzleController {
		private bool _isCompleted;

		public override void StartPuzzle() {
			Interactable interactableComponent = GetComponent<Interactable>();

			if (_isCompleted) {
				Debug.Log("This puzzle has already been completed.");
				return;
			}

			if (!interactableComponent.isEnabled) {
				Debug.Log("You don\'t have the required object for this puzzle.");
				return;
			}

			Complete();

			transform.GetChild(0).gameObject.SetActive(true);
			
			GetComponent<Interactable>().isEnabled = false;
			_isCompleted                           = true;
		}
	}
}