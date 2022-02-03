using UnityEngine;
using UnityEngine.Events;

namespace Puzzle_Control.Puzzle2D {
	public class PuzzleObject2D : MonoBehaviour {
		[SerializeField] private bool       draggable  = false;
		[SerializeField] private bool       clickable  = false;
		[SerializeField] public UnityEvent onClick;
		
		[SerializeField, Range(0f, 1f)] private float x;
		[SerializeField, Range(0f, 1f)] private float y;

		public bool Draggable => draggable;
		public bool Clickable => clickable;

		public float X => x;

		public float Y => y;
	}
}