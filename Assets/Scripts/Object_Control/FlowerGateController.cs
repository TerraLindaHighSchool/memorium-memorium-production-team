using Other;
using UnityEngine;
using Game_Managing.Game_Context;

namespace Puzzle_Control.FlowerGateController
{
	[RequireComponent(typeof(Interactable))]
	public class FlowerGateController : PuzzleController
	{ 
		public bool has_completed { get; private set; }

		public override void StartPuzzle()
		{
			// on interact with cage

			Interactable interact = GetComponent<Interactable>();

			if (has_completed || !Game_Managing.SaveManager.Instance.libraryShard && !Game_Managing.SaveManager.Instance.featherShard)
			{
				return;
			}
			has_completed = !has_completed;

			this.gameObject.SetActive(false);

			interact.SetIsEnabled(false);
		}
	}
}