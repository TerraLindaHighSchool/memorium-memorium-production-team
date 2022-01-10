using UnityEngine;

namespace Puzzle_Control.Prototype_Puzzle {
	public class FirstPuzzle : PuzzleController {
		public override void StartPuzzle() {
			Debug.Log("Picked up first puzzle object");
			Complete();
			gameObject.SetActive(false);
		}
	}
}