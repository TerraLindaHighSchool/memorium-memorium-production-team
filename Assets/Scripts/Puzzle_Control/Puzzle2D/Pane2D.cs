using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Game_Managing.Game_Context;
using Player_Control;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Puzzle_Control.Puzzle2D {
	[ExecuteInEditMode]
	public class Pane2D : MonoBehaviour, IGameContext {
		[SerializeField] private float                    width;
		private                  float                    height => width / VCam.m_Lens.Aspect;
		[SerializeField] private CinemachineVirtualCamera vcam;
		public                   CinemachineVirtualCamera VCam => vcam;
		private                  List<PuzzleObject2D>     elements = new List<PuzzleObject2D>();
		private                  PuzzleObject2D           dragTarget;
		private                  bool                     isContextActive = false;
		private                  bool                     lcDownLastFrame = false;

		private float   HalfWidth   => width  / 2;
		private float   HalfHeight  => height / 2;
		private Vector3 TopLeft     => transform.TransformPoint(new Vector3(-HalfWidth, HalfHeight, 0));
		private Vector3 TopRight    => transform.TransformPoint(new Vector3(HalfWidth, HalfHeight, 0));
		private Vector3 BottomLeft  => transform.TransformPoint(new Vector3(-HalfWidth, -HalfHeight, 0));
		private Vector3 BottomRight => transform.TransformPoint(new Vector3(HalfWidth, -HalfHeight, 0));


		private void OnDrawGizmos() {
			int     thickness = 5;
			Vector3 vcamPos   = VCam.transform.position;

			Handles.DrawBezier(TopLeft, TopRight, TopLeft, TopRight, Color.yellow, null, thickness);
			Handles.DrawBezier(TopRight, BottomRight, TopRight, BottomRight, Color.yellow, null, thickness);
			Handles.DrawBezier(BottomRight, BottomLeft, BottomRight, BottomLeft, Color.yellow, null, thickness);
			Handles.DrawBezier(BottomLeft, TopLeft, BottomLeft, TopLeft, Color.yellow, null, thickness);

			Handles.DrawBezier(vcamPos, TopLeft, vcamPos, TopLeft, Color.yellow, null, thickness);
			Handles.DrawBezier(vcamPos, TopRight, vcamPos, TopRight, Color.yellow, null, thickness);
			Handles.DrawBezier(vcamPos, BottomLeft, vcamPos, BottomLeft, Color.yellow, null, thickness);
			Handles.DrawBezier(vcamPos, BottomRight, vcamPos, BottomRight, Color.yellow, null, thickness);

			Gizmos.color = Color.white;
			Vector3 pos = transform.position;
			Gizmos.DrawSphere(pos, 0.1f);
			Gizmos.DrawRay(pos, transform.forward);
		}

		private void Start() {
			elements = GetComponentsInChildren<PuzzleObject2D>().ToList();
		}

		private void Update() {
			UpdateCamTransform();
			foreach (PuzzleObject2D elem in elements) UpdateElementTransform(elem);
		}

		private void OnTransformChildrenChanged() {
			elements = GetComponentsInChildren<PuzzleObject2D>().ToList();
			/*foreach (PuzzleObject2D elem in elements) {
				elem.onElemUpdate += OnElementUpdate;
			}*/
		}

		private void OnValidate() { UpdateCamTransform(); }

		private void UpdateCamTransform() {
			float dist = FovWidth(HalfWidth, HalfHeight) / (2 * Mathf.Tan(VCam.m_Lens.FieldOfView * Mathf.Deg2Rad / 2));
			Vector3 forward = transform.forward;
			Vector3 cameraPos = transform.position - forward * dist;
			Quaternion cameraRot = Quaternion.LookRotation(forward, transform.up);

			VCam.transform.SetPositionAndRotation(cameraPos, cameraRot);
			VCam.m_Lens.NearClipPlane = dist - 1;
		}

		private void UpdateElementTransform(PuzzleObject2D elem) {
			Vector3 localPos = PaneToLocalSpace(new Vector2(elem.X, elem.Y));
			localPos.z = dragTarget == elem ? -0.2f : 0.0f;

			elem.transform.localPosition = localPos;
		}

		private float FovWidth(float w, float h) { return Mathf.Sqrt(w * w + h * h); }

		public void GCStart() {
			isContextActive                                                      =  true;
			PlayerInputManager.Instance.PlayerInputActions.Player.Jump.performed += Exit;
		}

		public void temp() {
			Debug.Log("testing");
		}

		private void Exit(InputAction.CallbackContext context) { //note: context is just to make sure the method signature matches
			isContextActive                                                      =  false;
			PlayerInputManager.Instance.PlayerInputActions.Player.Jump.performed -= Exit;
			OnExit?.Invoke();
		}

		public void GCUpdateDelta(Vector2 mouseDelta, bool lcDown, bool rcDown) { }

		public void GCUpdatePos(Vector2 mousePos, bool lcDown, bool rcDown) {
			if (rcDown && !lcDownLastFrame) HandleClick(mousePos);
			
			lcDownLastFrame = rcDown;
		}

		public void HandleClick(Vector2 mousePos) {
			Vector3        target    = transform.TransformPoint(PaneToLocalSpace(PaneFromScreenSpace(mousePos)));
			PuzzleObject2D targetObj = GetObjForPoint(target);
			if (targetObj.Clickable) targetObj.onClick?.Invoke();
		}

		private static Vector2 PaneFromScreenSpace(Vector2 mousePos) =>
			new Vector2(
				mousePos.x / Screen.width,
				mousePos.x / Screen.height
			);

		private static bool IsPointInside(Collider c, Vector3 point) => c.ClosestPoint(point) == point;

		private PuzzleObject2D GetObjForPoint(Vector3 point) {
			foreach (PuzzleObject2D elem in elements) {
				Collider _collider = elem.GetComponent<Collider>();
				if (IsPointInside(_collider, point)) return elem;
			}

			return null;
		}

		private Vector3 PaneToLocalSpace(Vector2 paneLoc) =>
			new Vector3(
				paneLoc.x * width - HalfWidth,
				paneLoc.y * height - HalfHeight
			);
		

		public float        GetYRotForForwards()       { throw new NotImplementedException(); }
		
		public Transform    GetPlayerFollowCamTarget() { throw new NotImplementedException(); }
		public event Action OnExit;
	}
}