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
        /// Amount to rotate the flower by.
        /// Should be a factor of 360 so that a rotation can complete.
        /// </summary>
        public int rotationAmount = 90;

        /// <summary>
        /// Amount of rotation needed to complete the puzzle.
        /// Should be a multiple of rotationAmount.
        /// </summary>
        public int successfulRotation = 180;

        /// <summary>
        /// The flower's current rotation.
        /// </summary>
        private int currentRotation = 0;


        /// <summary>
        /// Method should be called upon instantiation of Light_P
        /// </summary>
        public override void StartPuzzle()
        {
			Interactable interactableComponent = GetComponent<Interactable>();
            // make sure the interactable component is enabled
            if (!interactableComponent.isEnabled)
            {
                Debug.Log("LightPuzzleFlower Interactable was not enabled by default. Error?");
                return;
            }
            // Assign OnInteract to the Interactable onInteractEvent
            interactableComponent.onInteractEvent.AddListener(OnInteract);
        }

        /// <summary>
        /// Called after the flower is rotated, checks if
        /// the flower is in the correct position.
        /// If so, completes the puzzle.
        /// </summary>
        private void CheckRotation()
        {

        }

        /// <summary>
        /// Called from OnInteract if we need to rotate the flower.
        /// </summary>
        private void Rotate()
        {
            // add the rotation amount to the current rotation
            currentRotation += rotationAmount;
            // if current rotation is greater than 360, reset to 0
            currentRotation > 360 ? currentRotation = 0 : currentRotation = currentRotation;

            // TODO: add interaction with rendering script to display rotation of the flower

            // call CheckRotation to see if the flower is in the correct position
            CheckRotation();
        }

        /// <summary>
        /// Called from OnInteract to check if the player has a shard.
        /// If they do, remove the shard from the player and set hasShard to true.
        /// </summary>
        private void CheckShard()
        {
            // FOR DEBUGGING: assume the player has the shard
            hasShard = true;
        }

        /// <summary>
        /// Called when the flower is interacted with.
        /// </summary>
        private void OnInteract()
        {
            // TODO: check if player actually has a shard

            // if we already have a shard, do rotation mode
            // otherwise, check if the player has a shard
            if (hasShard)
            {
                Rotate();
            }
            else
            {
                CheckShard();
            }
        }
    }
}