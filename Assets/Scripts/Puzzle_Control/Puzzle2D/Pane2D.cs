using System;
using Cinemachine;
using UnityEditor;
using UnityEngine;

namespace Puzzle_Control.Puzzle2D {
	public class Pane2D : MonoBehaviour {
		[SerializeField] private float                    width;
		private                  float                    height => width / VCam.m_Lens.Aspect;
		[SerializeField] private float                    fov = 90;
		[SerializeField] private CinemachineVirtualCamera vcam;
		public                   CinemachineVirtualCamera VCam => vcam;
		private                  PuzzleObject2D[]         elements;
		private                  PuzzleObject2D           dragTarget;

		private void OnDrawGizmos() {
			int     thickness = 5;
			Vector3 tl        = transform.TransformPoint(new Vector3(-width, height, 0));
			Vector3 tr        = transform.TransformPoint(new Vector3(width, height, 0));
			Vector3 bl        = transform.TransformPoint(new Vector3(-width, -height, 0));
			Vector3 br        = transform.TransformPoint(new Vector3(width, -height, 0));
			Vector3 vcamPos   = VCam.transform.position;

			Handles.DrawBezier(tl, tr, tl, tr, Color.yellow, null, thickness);
			Handles.DrawBezier(tr, br, tr, br, Color.yellow, null, thickness);
			Handles.DrawBezier(br, bl, br, bl, Color.yellow, null, thickness);
			Handles.DrawBezier(bl, tl, bl, tl, Color.yellow, null, thickness);

			Handles.DrawBezier(vcamPos, tl, vcamPos, tl, Color.yellow, null, thickness);
			Handles.DrawBezier(vcamPos, tr, vcamPos, tr, Color.yellow, null, thickness);
			Handles.DrawBezier(vcamPos, bl, vcamPos, bl, Color.yellow, null, thickness);
			Handles.DrawBezier(vcamPos, br, vcamPos, br, Color.yellow, null, thickness);

			Gizmos.color = Color.white;
			Vector3 pos = transform.position;
			Gizmos.DrawSphere(pos, 0.1f);
			Gizmos.DrawRay(pos, transform.forward);
		}

		private void Update() {
			UpdateCamTransform();
			UpdateElementTransforms();
		}

		private void OnTransformChildrenChanged() {
			elements = GetComponentsInChildren<PuzzleObject2D>();
			/*foreach (PuzzleObject2D elem in elements) {
				elem.onElemUpdate += OnElementUpdate;
			}*/
		}

		private void OnValidate() {
			VCam.m_Lens.FieldOfView = fov;
			UpdateCamTransform();
		}

		private void UpdateCamTransform() {
			float      dist = FovWidth(width, height) / (2 * Mathf.Tan(VCam.m_Lens.FieldOfView * Mathf.Deg2Rad / 2));
			Vector3    forward = transform.forward;
			Vector3    cameraPos = transform.position - forward * dist;
			Quaternion cameraRot = Quaternion.LookRotation(forward, transform.up);

			VCam.transform.SetPositionAndRotation(cameraPos, cameraRot);
		}

		private void UpdateElementTransforms() {
			foreach (PuzzleObject2D elem in elements) {
				float targetX = elem.X * width  - width  / 2;
				float targetY = elem.Y * height - height / 2;
				//float targetZ = dragTarget != null && dragTarget == elem ? -0.2f : 0.0f;
				float targetZ = Mathf.Sin(Time.time);
				elem.transform.localPosition = new Vector3(targetX, targetY, targetZ);
			}
		}

		/*private void OnElementUpdate(PuzzleObject2D elem) {
			float targetX = elem.X * width  - width  / 2;
			float targetY = elem.Y * height - height / 2;
			//float targetZ = dragTarget != null && dragTarget == elem ? -0.2f : 0.0f;
			float targetZ = Mathf.Sin(Time.time);
			elem.transform.localPosition = new Vector3(targetX, targetY, targetZ);
		}*/

		private float FovWidth(float w, float h) { return Mathf.Sqrt(w * w + h * h); }
		
		
	}
}