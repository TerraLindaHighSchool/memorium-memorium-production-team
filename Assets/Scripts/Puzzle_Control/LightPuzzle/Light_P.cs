using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Dictionary of all the flowers in the puzzle
        /// as well as boolean values to keep track of whether
        /// they have been completed
        /// </summary>
        private Dictionary<LightPuzzleFlower, bool> flowers;

        public override void StartPuzzle()
        {
            Debug.Log("Picked up first puzzle object");
            Complete();
            gameObject.SetActive(false);
        }
    }
}