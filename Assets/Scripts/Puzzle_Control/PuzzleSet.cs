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

		private Dictionary<string, bool> _puzzlesComplete;

		private void OnEnable() {
			_puzzlesComplete = new Dictionary<string, bool>();
			foreach (PuzzleController puzzle in puzzleComponents) {
				_puzzlesComplete.Add(puzzle.Guid, false);
				puzzle.NotifyPuzzleSet += HandlePuzzleComplete;
			}
		}

		private void OnDestroy() {
			foreach (PuzzleController puzzle in puzzleComponents) {
				puzzle.NotifyPuzzleSet -= HandlePuzzleComplete;
			}
		}

		private void HandlePuzzleComplete(String guid) {
			_puzzlesComplete[guid] = true;
			if (_puzzlesComplete.All(entry => entry.Value)) { onPuzzleSetComplete.Invoke();}
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