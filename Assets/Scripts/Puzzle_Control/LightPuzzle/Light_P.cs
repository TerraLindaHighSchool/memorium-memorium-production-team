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
        private Dictionary<LightPuzzleFlower, string> flowers;
        private Dictionary<string, bool> flowersComplete;

        public override void StartPuzzle()
        {
            // for each flower in the puzzle, add it to the dictionary with its Guid as the key,
            // add the guid to the dictionary with a value of false, and subscribe to the NotifyPuzzleSet event

            flowers = new Dictionary<LightPuzzleFlower, string>();
            flowersComplete = new Dictionary<string, bool>();
            foreach (LightPuzzleFlower flower in flowerPuzzles)
            {
                flowers.Add(flower, flower.Guid);
                flowersComplete.Add(flower.Guid, false);
                flower.NotifyPuzzleSet += OnCompletion;
            }
        

        }

        /// <summary>
        /// Event handler for when a flower is completed
        /// </summary>
        private void OnCompletion(string guid)
        {
            flowersComplete[guid] = true;
            if (flowersComplete.All(entry => entry.Value))
            {
                onPuzzleSetComplete.Invoke();
            }
        }
    }
}