using UnityEngine;
using UnityEngine.Events;

namespace Other {
	/*
	 * Parent class used for all interactable objects
	 * All interactables must also be set on the Interactable layer in the scene
	 *
	 * The RequireComponent is only for the Unity AssetStore outline plugin, may be changed later
	 */
	/// <summary>
	/// MonoBehaviour that is placed on all Interactable objects.
	/// Handles being interacted with, and will fire onInteractEvent in that case.
	/// All interactables must also be set on the "Interactable" layer in the scene.
	/// </summary>
	[RequireComponent(typeof(Outline))]
	public class Interactable : MonoBehaviour {
		
		/// <summary>
		/// Primarily used for puzzles, this is for determining whether this will have its "normal" behavior when interacted with. 
		/// Does not actually prevent anything from happening on its own.
		/// </summary>
		public bool isEnabled;
		
		///The UnityEvent that fires when the object is interacted with. 
		public UnityEvent onInteractEvent;
		
		/// <summary>
		/// Sets the highlight of the object based on value, and then with the given color and sometimes width. 
		/// </summary>
		/// <param name="value">Whether or not the outline should be enabled.</param>
		/// <param name="color">The new outline color. Pass in <c>Color.clear</c> for no outline.</param>
		/// <param name="width">Optional, the new outline width. Pass in nothing for no outline.</param>
		public void SetHighlight(bool value, Color color, float width = 0.0f) {
			Outline outline = GetComponent<Outline>();

			if (width > 0) outline.OutlineWidth = width;
			outline.OutlineColor = color;
			outline.enabled      = value;
		}
		
		/// <summary>
		/// Sets whether this interactable is "enabled".
		/// </summary>
		/// <param name="value">The new value for <c>isEnabled</c>.</param>
		public void SetIsEnabled(bool value) {
			isEnabled = value;
		}
	}
}