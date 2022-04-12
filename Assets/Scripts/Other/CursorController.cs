using System;
using System.Collections.Generic;
using Game_Managing.Game_Context;
using Game_Managing.Game_Context.Cutscene;
using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Other {
	/// <summary>
	/// This class lives on the <c>Cursor</c>. prefab in the scene.
	/// Manages placing the cursor in the scene, interactable highlighting,
	/// and interactions. 
	/// </summary>
	public class CursorController : MonoBehaviour {
		///The GameObject of whatever interactable is currently selected. 
		public GameObject selectedInteractableObject;

		/// <summary>
		/// Reference to the player, used to subscribe to the <c>Moved</c> event. 
		/// </summary>
		public PlayerController player;

		///The color that interactables will be when they are in range, but not selected. 
		public Color baseHighlightColor;

		///The color interactables are highlighted when selected and they can be interacted with. 
		public Color selectedHighlightColor;

		///The color interactables are highlighted when selected but they can't yet be interacted with. 
		public Color notEnabledHighlightColor;

		/// <summary>
		/// The max ray length for placing the cursor. 
		/// The cursor will be at this distance from the camera if no interactable is hit. 
		/// </summary>
		[SerializeField] private float maxRayLength = 100f;

		///List of all interactables that are currently within range of the player. 
		private List<Interactable> _interactablesInRange;

		///A reference to the main camera in the scene, used for casting out the ray for cursor position. 
		private Camera _mainCamera;

		///Vector2 for tracking the mouse in screen-space coords. 
		private Vector2 _mousePos;

		///Vector3 for tracking physical cursor position in the scene. 
		private Vector3 _cursorPosition;

		/// <summary>
		/// Creates the <c>_interactablesInRange</c> list, gets the main camera, and subscribes to necessary events. 
		/// </summary>
		private void Start() {
			_interactablesInRange = new List<Interactable>();

			_mainCamera = Camera.main;

			player.Moved += CheckInteractactables;
			player.Moved += SetCursorPos;

			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.Interact.performed += TriggerInteract;
			playerInputActions.Player.MousePos.performed += OnMousePos;
		}

		private void OnDisable() {
			player.Moved -= CheckInteractactables;
			player.Moved -= SetCursorPos;
			
			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.Interact.performed -= TriggerInteract;
			playerInputActions.Player.MousePos.performed -= OnMousePos;
		}

		/// <summary>
		/// Fires when the mouse screen position changes. 
		/// Stores the new position and checks if interactable highlights need recalculating. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>MousePos.performed</c> event.</param>
		private void OnMousePos(InputAction.CallbackContext context) {
			_mousePos = context.ReadValue<Vector2>();
			CheckInteractactables();
			SetCursorPos();
		}

		/// <summary>
		/// Checks all interactables in the scene and sets the list of <c>_interactablesInRange</c>. 
		/// If the list has been changed this frame, recompute the outlines. 
		/// </summary>
		private void CheckInteractactables() {
			bool hasModifiedInteractableList = false;
			foreach (Interactable interactable in FindObjectsOfType<Interactable>()) {
				if (player.GetDistanceToObject(interactable.gameObject) <= player.interactDistance) {
					if (!_interactablesInRange.Contains(interactable)) {
						_interactablesInRange.Add(interactable);
						hasModifiedInteractableList = true;
					}
				} else {
					if (_interactablesInRange.Remove(interactable)) {
						interactable.SetHighlight(false, Color.clear);
						hasModifiedInteractableList = true;
					}
				}
			}

			if (hasModifiedInteractableList) ComputeInteractableOutlines();
		}

		///Computes and sets the outlines of all interactables within range of the player. 
		private void ComputeInteractableOutlines() {
			for (int i = 0; i < _interactablesInRange.Count; i++) {
				Interactable interactable       = _interactablesInRange[i];
				GameObject   interactableObject = interactable.gameObject;
				interactable.SetHighlight(
					true,
					interactableObject == selectedInteractableObject
						? interactable.isEnabled ? selectedHighlightColor : notEnabledHighlightColor
						: baseHighlightColor,
					interactableObject == selectedInteractableObject ? 5.0f : 2.0f);
			}
		}

		///Sets the position of the cursor in the scene, and sets the selected interactable to what is selected. 
		private void SetCursorPos() {
			Ray ray = _mainCamera.ScreenPointToRay(_mousePos);

			LayerMask colliderMask = LayerMask.GetMask("Interactable");
			if (Physics.Raycast(ray, out RaycastHit hit, maxRayLength, colliderMask)) {
				_cursorPosition = hit.point;
				Interactable highlighted = hit.collider.gameObject.GetComponent<Interactable>();
				if (!highlighted) {
					Debug.LogWarning(
						$"Object {hit.collider.gameObject} is in the Interactable layer, but does not have the Interactable component!");
					if (selectedInteractableObject != null) OnHighlightStop();
					selectedInteractableObject = null;
				} else {
					if (!hit.collider.gameObject.Equals(selectedInteractableObject))
						OnHighlightStart(hit.collider.gameObject);
				}
			} else {
				_cursorPosition = ray.direction * maxRayLength + _mainCamera.transform.position;
				if (selectedInteractableObject != null) OnHighlightStop();
			}

			transform.position = _cursorPosition;
		}

		/// <summary>
		/// Called from the <c>Interact</c> action in <c>PlayerInputActions</c>.
		/// Interacts with an interactable if one is selected. 
		/// </summary>
		/// <param name="context">The Action CallbackContext, passed in from the <c>Interact.performed</c> event.</param>
		private void TriggerInteract(InputAction.CallbackContext context) {
			IGameContext activeContext = GameContextManager.Instance.ActiveContext;
			if (selectedInteractableObject
			 && !(activeContext is DialogueContextController
			   || activeContext is CutsceneContextController)) {
				selectedInteractableObject.GetComponent<Interactable>().onInteractEvent.Invoke();
				ComputeInteractableOutlines();
			}
		}

		/// <summary>
		/// Sets the <c>selectedInteractableObject</c> to <c>newInteractable</c> and recomputes outlines. 
		/// </summary>
		/// <param name="newInteractable">The new GameObject (that has is an interactable) to be set as the selected interactable.</param>
		private void OnHighlightStart(GameObject newInteractable) {
			if (player.GetDistanceToObject(newInteractable) > player.interactDistance) return;
			selectedInteractableObject = newInteractable;
			ComputeInteractableOutlines();
		}

		/// <summary>
		/// Sets the <c>selectedInteractableObject</c> to <c>null</c> and recomputes outlines. 
		/// </summary>
		private void OnHighlightStop() {
			selectedInteractableObject = null;
			ComputeInteractableOutlines();
		}
	}
}
