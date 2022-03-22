using Other;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Other {
	
	/**
	 * <summary> A WIP implementation of a custom property drawer for an Optional&lt;T&gt; that doesn't work at all.
	 * This will likely be abandoned, in favor of defining instances for concrete Optionals. </summary>
	 */
	[CustomPropertyDrawer(typeof(Optional<int>))]
	public class OptionalPropertyDrawer : PropertyDrawer {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			// Create property container element.
			var container = new VisualElement();

			// Create property fields.
			var amountField = new PropertyField(property.FindPropertyRelative("_value"), "Value");
			var unitField   = new PropertyField(property.FindPropertyRelative("_enabled"), "Enabled");

			// Add fields to the container.
			container.Add(amountField);
			container.Add(unitField);

			return container; 
		}
	}
}