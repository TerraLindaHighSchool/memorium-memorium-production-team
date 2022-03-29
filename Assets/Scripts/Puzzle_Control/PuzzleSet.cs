using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle_Control {
	/// <summary>
	/// A utility component used to assemble multiple puzzles into one larger puzzle.
	/// Keeps track of the status of contained puzzles, and when all are complete emits
	/// a unity event.
	/// </summary>
	public class PuzzleSet : MonoBehaviour {
		[SerializeField] private UnityEvent onPuzzleSetComplete;

		[SerializeField] private PuzzleController[] puzzleComponents;

		
		/// <summary>
		/// A dictionary mapping Guids to bools to store the status of each child PuzzleController.
		/// </summary>
		private Dictionary<string, bool> _puzzlesComplete;

		/// <summary>
		/// Adds all child PuzzleControllers' statuses to a Dictionary indexed by Guid,
		/// and subscribes to their OnComplete c# events.
		/// </summary>
		private void OnEnable() {
			_puzzlesComplete = new Dictionary<string, bool>();
			foreach (PuzzleController puzzle in puzzleComponents) {
				_puzzlesComplete.Add(puzzle.Guid, false);
				puzzle.NotifyPuzzleSet += HandlePuzzleComplete;
			}
		}

		/// <summary>
		/// Unsubscribes from PuzzleControllre event publishers when this component is destroyed
		/// </summary>
		private void OnDestroy() {
			foreach (PuzzleController puzzle in puzzleComponents) {
				puzzle.NotifyPuzzleSet -= HandlePuzzleComplete;
			}
		}

		/// <summary>
		/// Used as an event handler for child PuzzleController completion, it sets the puzzle's completion to
		/// true in the dictionary, and if all puzzles are then complete, invokes the onPuzzleSetComplete event.
		/// </summary>
		/// <param name="guid">the Guid of the puzzle that has been completed</param>
		private void HandlePuzzleComplete(String guid) {
			_puzzlesComplete[guid] = true;
			if (_puzzlesComplete.All(entry => entry.Value)) { onPuzzleSetComplete.Invoke();}
		}
		
		/// <summary>
		/// Draws a line between the PuzzleSet's GameObject and each of its child PuzzleControllers
		/// </summary>
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