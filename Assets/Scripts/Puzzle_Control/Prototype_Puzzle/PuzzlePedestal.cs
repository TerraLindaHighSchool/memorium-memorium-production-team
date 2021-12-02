using Other;
using UnityEngine;

namespace Puzzle_Control.Prototype_Puzzle {
	[RequireComponent(typeof(Interactable))]
	public class PuzzlePedestal : PuzzleController {
		public override void StartPuzzle() {
			Interactable interactableComponent = GetComponent<Interactable>();
			
			if (!interactableComponent.isEnabled) {
				Debug.Log("The requirements to do this puzzle are not met.");
				return;
			}
			
			Debug.Log("First puzzle completed!");
			Complete();
			GetComponent<Interactable>().isEnabled = false;
		}
	}
}