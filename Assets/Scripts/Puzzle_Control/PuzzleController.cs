using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle_Control {
	public abstract class PuzzleController : MonoBehaviour {
		[SerializeField] UnityEvent onPuzzleComplete;
		public event Action<string> notifyPuzzleSet; //Tell the contained puzzle set the puzzle is complete

		public readonly string guid = GUID.Generate().ToString();

		public abstract void StartPuzzle();
	}
}