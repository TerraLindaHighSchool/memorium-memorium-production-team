using UnityEngine;

namespace Other {
	public class CursorController : MonoBehaviour {
		public Camera mainCamera;

		public float maxRayLength = 100f;

		public GameObject selectedInteractable;

		public Material highlightMat;

		private Vector3 _cursorPosition;

		private void Start() { mainCamera = Camera.main; }

		private void Update() {
			SetMousePos();
			//TODO: this is old input system, once merged modify to use new input system
			if (Input.GetKeyDown("e")) { TriggerInteract(); }
		}

		private void SetMousePos() {
			//TODO: this is old input system, once merged modify to use new input system
			Vector3 mousePos = Input.mousePosition;

			Ray ray = mainCamera.ScreenPointToRay(mousePos);

			LayerMask colliderMask = LayerMask.GetMask("Interactable");

			if (Physics.Raycast(ray, out RaycastHit hit, maxRayLength, colliderMask)) {
				_cursorPosition = hit.point;
				Interactable highlighted = hit.collider.gameObject.GetComponent<Interactable>();
				if (!highlighted) {
					Debug.LogWarning(
						$"Object {hit.collider.gameObject} is in the Interactable layer, but does not have the Interactable component!");
					selectedInteractable = null;
				} else {
					selectedInteractable = hit.collider.gameObject;

					//add highlightMat to the selected object
					MeshRenderer selectedMeshRenderer = selectedInteractable.GetComponent<MeshRenderer>();

					if (!selectedMeshRenderer.materials[selectedMeshRenderer.materials.Length - 1].Equals(highlightMat)) {
						Material[] mats = new Material[selectedMeshRenderer.materials.Length + 1];

						for (int i = 0; i < selectedMeshRenderer.materials.Length; i++) {
							mats[i] = selectedMeshRenderer.materials[i];
						}

						mats[mats.Length - 1] = highlightMat;

						selectedMeshRenderer.materials = mats;
					}
				}
			} else {
				_cursorPosition      = ray.direction * maxRayLength + mainCamera.transform.position;
				selectedInteractable = null;
			}

			transform.position = _cursorPosition;
		}

		private void TriggerInteract() {
			if (selectedInteractable) { selectedInteractable.GetComponent<Interactable>().onInteractEvent.Invoke(); }
		}
	}
}