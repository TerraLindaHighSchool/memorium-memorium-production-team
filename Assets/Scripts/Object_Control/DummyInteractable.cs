using UnityEngine;

namespace Object_Control {
	/*
	 * Just a dummy default interactable
	 * Place it in a scene to make sure interactions are working properly
	 */
	public class DummyInteractable : MonoBehaviour {
		// Dummy interact method, just sends a message to the console
		public void Interact() { Debug.Log("Testing Interactable Object"); }
	}
}