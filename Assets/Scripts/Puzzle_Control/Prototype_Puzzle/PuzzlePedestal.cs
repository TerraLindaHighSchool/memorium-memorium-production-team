using Other;
using UnityEngine;

namespace Puzzle_Control.Prototype_Puzzle {
	[RequireComponent(typeof(Interactable))]
	public class PuzzlePedestal : PuzzleController {
		public bool IsCompleted { get; private set; }

		public override void StartPuzzle() {
			Interactable interactableComponent = GetComponent<Interactable>();

			if (IsCompleted) {
				Debug.Log("This puzzle has already been completed.");
				return;
			}

			if (!interactableComponent.isEnabled) {
				Debug.Log("The requirements to do this puzzle are not met.");
				return;
			}

			Debug.Log("First puzzle completed!");
			Complete();
			GetComponent<Interactable>().isEnabled = false;
			IsCompleted                            = true;
		}
	}
}