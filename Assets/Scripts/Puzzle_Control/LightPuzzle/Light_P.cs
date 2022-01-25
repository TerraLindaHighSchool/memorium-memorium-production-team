using UnityEngine;

namespace Puzzle_Control.Light_P
{
    public class Light_P : PuzzleController
    {
        /// <summary>
        /// Puzzle controller for the Flower Light Puzzle
        /// still in progress, summary will be updated later
        //
        /// planned to be used as a utility script, will handle the logic of the puzzle
        /// </summary>
        public override void StartPuzzle()
        {
            Debug.Log("Picked up first puzzle object");
            Complete();
            gameObject.SetActive(false);
        }
    }
}