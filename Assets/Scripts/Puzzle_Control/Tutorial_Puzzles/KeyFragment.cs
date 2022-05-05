using Other;
using UnityEngine;

namespace Puzzle_Control.Cage_Puzzle
{
	[RequireComponent(typeof(Interactable))]
	public class KeyFragment : PuzzleController
	{
		public override void StartPuzzle()
		{
			Complete();
			gameObject.SetActive(false);
		}
	}
}