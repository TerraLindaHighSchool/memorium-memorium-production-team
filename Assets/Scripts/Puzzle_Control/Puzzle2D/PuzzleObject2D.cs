using UnityEngine;
using UnityEngine.Events;

namespace Puzzle_Control.Puzzle2D {
	public class PuzzleObject2D : MonoBehaviour {
		[SerializeField] protected bool draggable = false;
		[SerializeField] protected bool clickable = false;
		[SerializeField] public UnityEvent onClick;
		
		[SerializeField, Range(0f, 1f)] public float x;
		[SerializeField, Range(0f, 1f)] public float y;

		public bool  Draggable => draggable;
		public bool  Clickable  => clickable;
	}
}