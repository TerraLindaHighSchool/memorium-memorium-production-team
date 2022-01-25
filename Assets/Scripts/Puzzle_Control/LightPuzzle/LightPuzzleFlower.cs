using UnityEngine;

namespace Puzzle_Control.LightPuzzleFlower
{
	[RequireComponent(typeof(Interactable))]
    public class LightPuzzleFlower : PuzzleController
    {
        /// <summary>
        /// Puzzle handling script for each flower.
        /// Will first handle the flower receiving the shard,
        /// then will handle rotation.
        /// Will complete the individual puzzle once the flower is
        /// rotated to the correct position.
        /// </summary>

        /// <summary>
        /// Controls whether flower can be rotated, as well as
        /// rendering the shard on the flower.
        /// </summary>
        private bool hasShard = false;

        /// <summary>
        /// Method should be called upon instantiation of Light_P
        /// </summary>
        public override void StartPuzzle()
        {
			Interactable interactableComponent = GetComponent<Interactable>();
            // make sure the interactable component is enabled

        }

        private void OnInteract()
        {
            // TODO: check if player actually has a shard

            // if we already have a shard, do rotation mode
            if (hasShard)
            {

            }
        }
    }
}