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
        // copied from PuzzleSet.cs, unsure if it's necessary
		//[SerializeField] private UnityEvent onPuzzleSetComplete;
		[SerializeField] private LightPuzzleFlower[] flowerPuzzles;

        /// <summary>
        /// Dictionary of all the flowers in the puzzle
        /// as well as boolean values to keep track of whether
        /// they have been completed
        /// </summary>
        private Dictionary<LightPuzzleFlower, bool> flowers;

        public override void StartPuzzle()
        {
            // for each flower in flowerPuzzles, add it to the flowers dictionary, set bool to false
            // and subscribe to the OnComplete event
            int i = 0;
            flowers = new Dictionary<LightPuzzleFlower, bool>();
            foreach (LightPuzzleFlower flower in flowerPuzzles)
            {
                flowers.Add(flower, false);
                flower.NotifyPuzzleSet += OnCompletion(i);
            }

        }

        /// <summary>
        /// Event handler for when a flower is completed
        /// </summary>
        private void OnCompletion(int flowerIndex)
        {
            // set the flower's completion to true
            flowers[flowerPuzzles[flowerIndex]] = true;

            // if all flowers are complete, invoke the event
            if (flowers.All(entry => entry.Value))
            {
                // onPuzzleSetComplete.Invoke();
            }
        }
    }
}