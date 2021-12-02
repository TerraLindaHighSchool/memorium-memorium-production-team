using UnityEngine;
using UnityEngine.Events;

namespace Other {
	//temporary, need for Asset Store outline plugin
	[RequireComponent(typeof(Outline))]
	public class Interactable : MonoBehaviour {
		public bool isEnabled;

		private readonly Color _defaultHighlightColor = new Color(200, 200, 0);
		
		public UnityEvent onInteractEvent;
		
		public void SetHighlight(bool value, Color color, float width = 0.0f) {
			Outline outline = GetComponent<Outline>();

			if (width > 0) outline.OutlineWidth = width;
			outline.OutlineColor = color;
			outline.enabled      = value;
		}
	}
}