using System.Collections.Generic;
using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Other {
	/*
	 * MonoBehaviour that lives on the cursor prefab and controls the interactions
	 */
	public class CursorController : MonoBehaviour {
		// The GameObject of whatever interactable is currently selected
		public GameObject selectedInteractableObject;

		// Reference to the player, right now just used to get the PlayerInputActions object
		public PlayerController player;

		// The color that interactables will be when they are in range, but not selected
		public Color baseHighlightColor;
		
		// The color interactables are highlighted when selected and they can be interacted with
		public Color selectedHighlightColor;
		
		// The color interactables are highlighted when selected but they can't yet be interacted with
		public Color notEnabledHighlightColor;

		// The max ray length for placing the cursor
		// The cursor will be at this distance from the camera if no interactable is hit
		[SerializeField] private float maxRayLength = 100f;

		// List of all interactables that are currently within range of the player
		private List<Interactable> _interactablesInRange;

		// A reference to the main camera in the scene, used for casting out the ray for cursor position
		private Camera _mainCamera;

		// A reference to the player's PlayerInputActions, will hopefully be moved to a manager class soon
		private PlayerInputActions _playerInputActions;

		//Vector2 for tracking the mouse in screen-space coords
		private Vector2 _mousePos;

		//Vector3 for tracking physical cursor position in the scene
		private Vector3 _cursorPosition;

		// Creates the _interactablesInRange list, gets the main camera, and subscribes to necessary events
		private void Start() {
			_interactablesInRange = new List<Interactable>();

			_mainCamera = Camera.main;

			player.Moved += CheckInteractactables;
			player.Moved += SetCursorPos;

			_playerInputActions = player.PlayerInputActions;

			_playerInputActions.Player.Interact.performed += TriggerInteract;
			_playerInputActions.Player.MousePos.performed += OnMousePos;
		}

		// When the mouse moves, stores the new position and checks if interactable highlights need recalculating
		private void OnMousePos(InputAction.CallbackContext context) {
			_mousePos = context.ReadValue<Vector2>();
			CheckInteractactables();
			SetCursorPos();
		}

		// Checks all interactables in the scene and sets the list of _interactablesInRange
		// If the list has been changed this frame, recompute the outlines
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

		// Computes and sets the outlines of all interactables within range of the player
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

		// Sets the position of the cursor in the scene, and sets the selected interactable to what is selected
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

		// Called from the Interact action in PlayerInputActions, interacts with an interactable if one is selected
		private void TriggerInteract(InputAction.CallbackContext context) {
			if (selectedInteractableObject) {
				selectedInteractableObject.GetComponent<Interactable>().onInteractEvent.Invoke();
				ComputeInteractableOutlines();
			}
		}

		// Sets the selectedInteractableObject and recomputes outlines
		private void OnHighlightStart(GameObject newInteractable) {
			if (player.GetDistanceToObject(newInteractable) > player.interactDistance) return;
			selectedInteractableObject = newInteractable;
			ComputeInteractableOutlines();
		}

		// Sets the selectedInteractableObject to null and recomputes outlines
		private void OnHighlightStop() {
			selectedInteractableObject = null;
			ComputeInteractableOutlines();
		}
	}
}