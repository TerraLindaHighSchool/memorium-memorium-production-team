namespace Puzzle_Control.Stained_Glass_Assembly_Puzzle {
	public class AssemblyPiece : PuzzleController {
		public override void StartPuzzle() {
			Complete();
			gameObject.SetActive(false);
		}
	}
}