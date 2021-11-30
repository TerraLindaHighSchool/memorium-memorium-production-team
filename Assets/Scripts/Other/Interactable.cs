using UnityEngine;
using UnityEngine.Events;

namespace Other {
	//temporary, need for Asset Store outline plugin
	[RequireComponent(typeof(Outline))]
	public class Interactable : MonoBehaviour {
		public UnityEvent onInteractEvent;

		public void SetHighlight(bool value) {
			Outline outline = GetComponent<Outline>();

			outline.enabled = value;
		}
	}
}