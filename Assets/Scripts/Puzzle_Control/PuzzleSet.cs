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
				puzzlesComplete.Add(puzzle.Guid, false);
				puzzle.NotifyPuzzleSet += HandlePuzzleComplete;
			}
		}

		private void OnDestroy() {
			foreach (PuzzleController puzzle in puzzleComponents) {
				puzzle.NotifyPuzzleSet -= HandlePuzzleComplete;
			}
		}

		private void HandlePuzzleComplete(String guid) {
			puzzlesComplete[guid] = true;
			if (puzzlesComplete.All(entry => entry.Value)) { onPuzzleSetComplete.Invoke();}
		}
		
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.cyan;
			Gizmos.DrawSphere(gameObject.transform.position, 0.1f);
			
			
			foreach (PuzzleController puzzle in puzzleComponents) {
				Gizmos.color = Color.white;
				Gizmos.DrawLine(gameObject.transform.position, puzzle.gameObject.transform.position);
				
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(puzzle.gameObject.transform.position, 0.1f);
			}
		}
	}
}