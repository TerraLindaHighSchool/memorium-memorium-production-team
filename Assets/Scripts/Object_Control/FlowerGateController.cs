using Game_Managing;
using Other;
using Puzzle_Control;
using UnityEngine;

namespace Object_Control
{
	[RequireComponent(typeof(Interactable))]
	public class FlowerGateController : PuzzleController
	{

		public const int SHARD_COUNT = 1; // Set to 1 for testing purposes; change to however many shards there are other than the flower island shards
		public bool has_completed { get; private set; }

		private int shards_collected = 0;

		public override void StartPuzzle()
		{
			// on interact with cage

			Interactable interact = GetComponent<Interactable>();

			if (has_completed || shards_collected < SHARD_COUNT)
			{
				return;
			}
			has_completed = !has_completed;

			this.gameObject.SetActive(false);

			interact.SetIsEnabled(false);

			SaveManager.Instance.flowerGate = true;
		}

		public void add_collected_shard()
		{
			// on pick up shard

			Interactable interact = GetComponent<Interactable>();

			shards_collected++;
			if (shards_collected >= SHARD_COUNT)
			{
				interact.SetIsEnabled(true);
			}
		}


	}
}