﻿using System;
using System.Collections.Generic;
using System.Reflection;
using NPC_Control.Behavior_Tree;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	[Serializable]
	public abstract class NodeView<T> : Node where T : BehaviorNode {
		public static Action SaveTree;
		
		public Action<NodeView<T>> OnNodeSelected;

		public T node;

		public Port Input;

		public NodeView(T node) {
			this.node   = node;
			viewDataKey = node.guid;

			title = node.GetType().Name;

			style.left = node.position.x;
			style.top  = node.position.y;
		}

		//https://forum.unity.com/threads/uielements-and-scriptableobjects-in-editorwindow.729113/
		public static FieldInfo[] GetVisibleSerializedFields(Type T) {
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
		public static VisualElement CreateUIElementInspector(UnityEngine.Object target, List<string> propertiesToExclude) {
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

		protected virtual void CreateExtension() {
			VisualElement containerElement =
				new VisualElement {style = {backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f))}};

			VisualElement nodeBody = CreateUIElementInspector(node, null);
			containerElement.Add(nodeBody);

			extensionContainer.Add(containerElement);
			RefreshExpandedState();
		}

		protected virtual void CreateInputPort() {
			Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, null);

			if (Input != null) {
				Input.portName = "Input";
				inputContainer.Add(Input);
			}
		}
		
		protected Port CreatePort(Direction dir, string portName = "") {
			Port port = InstantiatePort(Orientation.Horizontal, dir, Port.Capacity.Single, null);

			if (port != null) {
				if (dir == Direction.Input) {
					port.portName = portName == "" ? "Input" : portName;
					inputContainer.Add(port);
				} else {
					port.portName = portName == "" ? "Output" : portName;
					outputContainer.Add(port);
				}
			}

			return port;
		}

		public override void SetPosition(Rect newPos) {
			base.SetPosition(newPos);
			node.position.x = newPos.xMin;
			node.position.y = newPos.yMin;
		}

		public override void OnSelected() {
			base.OnSelected();
			OnNodeSelected?.Invoke(this);
		}
	}
}