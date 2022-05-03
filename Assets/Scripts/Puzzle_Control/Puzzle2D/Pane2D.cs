using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Game_Managing.Game_Context;
using UnityEditor;
#if UNITY_EDITOR
	using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Puzzle_Control.Puzzle2D {
	[ExecuteInEditMode]
	public class Pane2D : PuzzleController, IGameContext {
		[SerializeField] private float                        width;
		private                  float                        height => width / VCam.m_Lens.Aspect;
		[SerializeField] private CinemachineVirtualCamera     vcam;
		public                   CinemachineVirtualCamera     VCam => vcam;
		private                  List<PuzzleObject2D>         elements = new List<PuzzleObject2D>();
		private                  PuzzleObject2D               dragObj;
		private                  Dictionary<DragTarget, bool> completion = new Dictionary<DragTarget, bool>();
		private                  bool                         isContextActive;
		private                  bool                         lcDownLastFrame;

		private float   HalfWidth   => width  / 2;
		private float   HalfHeight  => height / 2;
		private Vector3 TopLeft     => transform.TransformPoint(new Vector3(-HalfWidth, HalfHeight, 0));
		private Vector3 TopRight    => transform.TransformPoint(new Vector3(HalfWidth, HalfHeight, 0));
		private Vector3 BottomLeft  => transform.TransformPoint(new Vector3(-HalfWidth, -HalfHeight, 0));
		private Vector3 BottomRight => transform.TransformPoint(new Vector3(HalfWidth, -HalfHeight, 0));

		#if UNITY_EDITOR
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
		#endif

		private void Start() {
			elements = GetComponentsInChildren<PuzzleObject2D>().ToList();
		}

		private void Update() {
			UpdateCamTransform();
			foreach (PuzzleObject2D elem in elements) UpdateElementTransform(elem);
		}

		private void OnTransformChildrenChanged() {
			elements = GetComponentsInChildren<PuzzleObject2D>().ToList();
			completion.Clear();
			foreach (PuzzleObject2D elem in elements) {
				if (elem is DragTarget dragEl) {
					completion.Add(dragEl, false);
				}
			}
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
			Vector3 localPos = PaneToLocalSpace(new Vector2(elem.x, elem.y));
			localPos.z = dragObj == elem ? -0.2f : 0.0f;

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
			if (lcDown && !lcDownLastFrame) HandleClick(mousePos);
			
			lcDownLastFrame = lcDown;
		}

		public float GetYRotForForwards() { throw new NotImplementedException(); }

		public void HandleClick(Vector2 mousePos) {
			Vector2        paneMousePos = PaneFromScreenSpace(mousePos);
			Vector3        target       = transform.TransformPoint(PaneToLocalSpace(paneMousePos));
			PuzzleObject2D targetObj    = GetObjForPoint(target);
			if (targetObj != null) {
				if (targetObj.Clickable) targetObj.onClick?.Invoke();

				if (targetObj.Draggable) {
					if (targetObj == dragObj) {
						foreach (var el in elements) {
							if (el is DragTarget drag_el) {
								var pos  = new Vector2(drag_el.x, drag_el.y);
								var dist = (pos - paneMousePos).magnitude;
								if (dist < drag_el.fuzz && targetObj == drag_el.Target) {
									targetObj.x = drag_el.x;
									targetObj.y = drag_el.y;
									
									completion.Add(drag_el, true);
								}
							}
						}
						dragObj = null;
					} else {
						//todo: make it so you can't pick up placed objects once they're on target
						dragObj = targetObj;
					}
				}
			}
		}

		private static Vector2 PaneFromScreenSpace(Vector2 mousePos) =>
			new Vector2(
				mousePos.x / Screen.width,
				mousePos.y / Screen.height
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
		
		public Transform GetPlayerFollowCamTarget() { throw new NotImplementedException(); }
		
		public event Action OnExit;
		public override void StartPuzzle() { throw new NotImplementedException(); }
	}
}