using Other;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
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