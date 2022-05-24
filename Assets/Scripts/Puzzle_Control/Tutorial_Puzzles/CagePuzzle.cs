using Other;
using UnityEngine;

namespace Puzzle_Control.Cage_Puzzle 
{
	[RequireComponent(typeof(Interactable))]
	public class CagePuzzle : PuzzleController {

		public const int KEY_COUNT = 1; // Set to 1 for testing purposes; change later
		
		public bool has_completed { get; private set; }

		private int keys_collected = 0;

		public override void StartPuzzle ()
		{
			// on interact with cage

			Interactable interact = GetComponent<Interactable>();

			if (has_completed || keys_collected < KEY_COUNT)
			{
				return;
			}
			has_completed = !has_completed;

			interact.SetIsEnabled(false);
        }

		public void add_collected_key ()
        {
			// on pick up key

			Interactable interact = GetComponent<Interactable>();

			keys_collected++;
			if (keys_collected >= KEY_COUNT)
            {
				interact.SetIsEnabled(true);
            }
        }
	}
}