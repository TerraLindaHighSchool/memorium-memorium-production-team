using UnityEngine;

namespace Puzzle_Control.Light_P
{
    /// <summary>
    /// Puzzle controller for the Flower Light Puzzle
    /// still in progress, summary will be updated later
    //
    /// planned to be used as a utility script, will handle the logic of the puzzle
    /// </summary>
    public class Light_P : PuzzleController
    {
        private Dictionary<LightPuzzleFlower, bool> flowers = new Dictionary<LightPuzzleFlower, bool>();

        public override void StartPuzzle()
        {
            Debug.Log("Picked up first puzzle object");
            Complete();
            gameObject.SetActive(false);
        }
    }
}