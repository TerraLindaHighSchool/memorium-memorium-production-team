using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle_Control {
	public class PuzzleSet : MonoBehaviour {
		[SerializeField] private UnityEvent onPuzzleSetComplete;

		[SerializeField] private PuzzleController[] puzzleComponents;

		private Dictionary<string, bool> puzzlesComplete;

		private void Start() {
			foreach (PuzzleController puzzle in puzzleComponents) {
				puzzlesComplete.Add(puzzle.guid, false);
				puzzle.notifyPuzzleSet += HandlePuzzleComplete;
			}
		}

		private void OnDestroy() {
			foreach (PuzzleController puzzle in puzzleComponents) {
				puzzle.notifyPuzzleSet -= HandlePuzzleComplete;
			}
		}

		private void HandlePuzzleComplete(String guid) {
			puzzlesComplete[guid] = true;
			if (puzzlesComplete.All(entry => entry.Value)) { onPuzzleSetComplete.Invoke();}
		}
	}
}