using UnityEngine;

namespace Object_Control {
	/// <summary>
	/// Just a dummy Interactable controller.
	/// Meant to be used as an example or as a test to ensure interactions are working.
	/// </summary>
	public class DummyInteractable : MonoBehaviour {
		/// <summary>
		/// Dummy interact method.
		/// Just logs a message to the console.
		/// </summary>
		public void Interact() { Debug.Log($"Testing Interactable Object {this}"); }
	}
}