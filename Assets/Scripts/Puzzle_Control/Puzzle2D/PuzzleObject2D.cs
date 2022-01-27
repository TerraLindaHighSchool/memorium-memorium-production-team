using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle_Control.Puzzle2D {
	public class PuzzleObject2D : MonoBehaviour {
		[SerializeField] private bool       draggable  = false;
		[SerializeField] private bool       clickable  = false;
		[SerializeField] private UnityEvent onClick;

		[SerializeField] private float x;
		[SerializeField] private float y;

		public float X => x;

		public float Y => y;

		/*public event Action<PuzzleObject2D> onElemUpdate;*/

		private void OnGUI() {
			Debug.Log("among us");
			x = EditorGUILayout.Slider("X Coordinate", x, 0.0f, 1.0f);
			x = EditorGUILayout.Slider("Y Coordinate", x, 0.0f, 1.0f);
		}

		/*private void OnValidate() {
			onElemUpdate?.Invoke(this);
		}*/
	}
}