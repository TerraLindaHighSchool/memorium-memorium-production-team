using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Other {
	public static class UIElementsExtensions {
		//https://forum.unity.com/threads/uielements-and-scriptableobjects-in-editorwindow.729113/
		private static FieldInfo[] GetVisibleSerializedFields(Type T) {
			List<FieldInfo> infoFields = new List<FieldInfo>();

			var publicFields = T.GetFields(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < publicFields.Length; i++) {
				if (publicFields[i].GetCustomAttribute<HideInInspector>() == null) { infoFields.Add(publicFields[i]); }
			}

			var privateFields = T.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
			for (int i = 0; i < privateFields.Length; i++) {
				if (privateFields[i].GetCustomAttribute<SerializeField>() != null) { infoFields.Add(privateFields[i]); }
			}

			return infoFields.ToArray();
		}

		//https://forum.unity.com/threads/uielements-and-scriptableobjects-in-editorwindow.729113/
		public static VisualElement CreateUIElementInspector(UnityEngine.Object target, List<string> propertiesToExclude = null) {
			var container = new VisualElement();

			var serializedObject = new SerializedObject(target);

			var fields = GetVisibleSerializedFields(target.GetType());

			for (int i = 0; i < fields.Length; ++i) {
				var field = fields[i];
				// Do not draw HideInInspector fields or excluded properties.
				if (propertiesToExclude != null && propertiesToExclude.Contains(field.Name)) { continue; }

				var serializedProperty = serializedObject.FindProperty(field.Name);

				var propertyField = new PropertyField(serializedProperty);

				container.Add(propertyField);
			}

			container.Bind(serializedObject);


			return container;
		}
	}
}