using UnityEngine;
using UnityEngine.Events;

namespace Other {
	//temporary, need for Asset Store outline plugin
	[RequireComponent(typeof(Outline))]
	public class Interactable : MonoBehaviour {
		//Primarily used for puzzles, this is for determining whether this will have its "normal" behavior when interacted with
		//Does not actually prevent anything from happening on its own
		public bool isEnabled;
		
		public UnityEvent onInteractEvent;
		
		public void SetHighlight(bool value, Color color, float width = 0.0f) {
			Outline outline = GetComponent<Outline>();

			if (width > 0) outline.OutlineWidth = width;
			outline.OutlineColor = color;
			outline.enabled      = value;
		}
		
		public void SetIsEnabled(bool value) {
			isEnabled = value;
		}
	}
}