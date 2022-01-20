using System;
using UnityEditor;
using UnityEngine;

namespace Puzzle_Control.Puzzle2D {
	public class Pane2D : MonoBehaviour {
		[SerializeField] private float width;
		private                  float height => width / 16 * 9;

		private void OnDrawGizmos() {
			int     thickness = 5;
			Vector3 tl        = transform.TransformPoint(new Vector3(-width, height, 0));
			Vector3 tr        = transform.TransformPoint(new Vector3(width, height, 0));
			Vector3 bl        = transform.TransformPoint(new Vector3(-width, -height, 0));
			Vector3 br        = transform.TransformPoint(new Vector3(width, -height, 0));

			Handles.DrawBezier(tl, tr, tl , tr, Color.yellow, null, thickness);
			Handles.DrawBezier(tr, br, tr , br, Color.yellow, null, thickness);
			Handles.DrawBezier(br, bl, br , bl, Color.yellow, null, thickness);
			Handles.DrawBezier(bl, tl, bl , tl, Color.yellow, null, thickness);
			
			Gizmos.color = Color.white;
			Gizmos.DrawSphere(transform.position, 0.1f);
			Gizmos.DrawRay(transform.position, transform.forward);
		}

		private void Update() {
			
		}
	}
}