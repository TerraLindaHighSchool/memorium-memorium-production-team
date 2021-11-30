using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Other {
	public class CursorController : MonoBehaviour {
		public GameObject selectedInteractable;

		public PlayerController player;

		[SerializeField] private float maxRayLength = 100f;

		private Camera _mainCamera;

		private PlayerInputActions _playerInputActions;

		private Mouse   _mouse;
		private Vector3 _cursorPosition;

		private void Start() {
			_mainCamera = Camera.main;
			_mouse      = Mouse.current;

			_playerInputActions = player.PlayerInputActions;

			_playerInputActions.Player.Interact.performed += TriggerInteract;
		}

		private void Update() { SetMousePos(); }

		private void SetMousePos() {
			Vector2 mousePos = _mouse.position.ReadValue();

			Ray ray = _mainCamera.ScreenPointToRay(mousePos);

			LayerMask colliderMask = LayerMask.GetMask("Interactable");

			if (Physics.Raycast(ray, out RaycastHit hit, maxRayLength, colliderMask)) {
				_cursorPosition = hit.point;
				Interactable highlighted = hit.collider.gameObject.GetComponent<Interactable>();
				if (!highlighted) {
					Debug.LogWarning(
						$"Object {hit.collider.gameObject} is in the Interactable layer, but does not have the Interactable component!");
					if (selectedInteractable != null) OnHighlightStop();
					selectedInteractable = null;
				} else {
					if (!hit.collider.gameObject.Equals(selectedInteractable))
						OnHighlightStart(hit.collider.gameObject);
				}
			} else {
				_cursorPosition = ray.direction * maxRayLength + _mainCamera.transform.position;
				if (selectedInteractable != null) OnHighlightStop();
			}

			transform.position = _cursorPosition;
		}

		private void TriggerInteract(InputAction.CallbackContext context) {
			if (selectedInteractable) { selectedInteractable.GetComponent<Interactable>().onInteractEvent.Invoke(); }
		}

		private void OnHighlightStart(GameObject newInteractable) {
			selectedInteractable = newInteractable;
			selectedInteractable.GetComponent<Interactable>().SetHighlight(true);
		}

		private void OnHighlightStop() {
			selectedInteractable.GetComponent<Interactable>().SetHighlight(false);
			selectedInteractable = null;
		}
	}
}