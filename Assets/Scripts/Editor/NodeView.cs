using System;
using System.Collections.Generic;
using System.Reflection;
using NPC_Control.Behavior_Tree;
using NPC_Control.Behavior_Tree.Nodes.SingleChildNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	[Serializable]
	public class NodeView : Node {
		public Action<NodeView> OnNodeSelected;

		public BehaviorNode node;

		public Port input;
		public Port output;

		public NodeView(BehaviorNode node) {
			this.node   = node;
			viewDataKey = node.guid;

			title = node.GetType().Name;

			style.left = node.position.x;
			style.top  = node.position.y;

			CreateInputPorts();
			CreateOutputPorts();

			CreateExtension();
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

		private void CreateExtension() {
			VisualElement containerElement =
				new VisualElement {style = {backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f))}};

			VisualElement nodeBody = CreateUIElementInspector(node, null);
			containerElement.Add(nodeBody);

			extensionContainer.Add(containerElement);
			RefreshExpandedState();
		}

		private void CreateInputPorts() {
			if (node is RootNode) {
				//do nothing, root node has no inputs
			} else { input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, null); }

			if (input != null) {
				input.portName = "";
				inputContainer.Add(input);
			}
		}

		private void CreateOutputPorts() {
			//right now no type, use this for any nodes with no output
			if (false) {
				//do nothing, this node has no outputs
			} else { output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, null); }

			if (output != null) {
				output.portName = "";
				outputContainer.Add(output);
			}
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