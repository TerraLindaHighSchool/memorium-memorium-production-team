using System;
using System.Collections.Generic;
using System.Linq;
using NPC_Control.Behavior_Tree.Nodes;
using Other;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	public sealed class MapChildNodeView : NodeView<MapChildNode> {
		public static Action<Edge> OnDisconnectChild;
		public static Action       RegenerateEditor;

		public Dictionary<string, Port> Outputs;

		public MapChildNodeView(MapChildNode node) : base(node) {
			CreateInputPort();
			CreateOutputPorts();

			CreateExtension();
		}

		public void OnAddOutputButton() { AddOutputMenu.Init(this); }

		public void OnRemoveOutputButton() { RemoveOutputPort(); }

		public void AddOutputPort(string key = "bruh") {
			if (Outputs.ContainsKey(key)) {
				Debug.LogWarning($"Output port with key {key} already exists on this node.");
				return;
			}

			Port newOutput = CreatePort(Direction.Output, key);

			if (newOutput != null) Outputs.Add(key, newOutput);
		}

		public void RemoveOutputPort() {
			if (Outputs.Count <= 0) return;

			KeyValuePair<string, Port> kvp = Outputs.Last();
			if (kvp.Value.connected) {
				if (EditorUtility.DisplayDialog("Remove Connected Port",
				                                "This port has a connection. Do you still want to remove it?", "Yes",
				                                "No")) {
					Edge edge = kvp.Value.connections.First();
					OnDisconnectChild?.Invoke(edge);
					kvp.Value.Disconnect(edge);
					outputContainer.Remove(kvp.Value);
					Outputs.Remove(kvp.Key);
					RegenerateEditor?.Invoke();
				}
			} else {
				outputContainer.Remove(kvp.Value);
				Outputs.Remove(kvp.Key);
				RegenerateEditor?.Invoke();
			}
		}

		protected override void CreateExtension() {
			VisualElement containerElement =
				new VisualElement {style = {backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f))}};

			VisualElement nodeBody = UIElementsExtensions.CreateUIElementInspector(node);
			containerElement.Add(nodeBody);

			extensionContainer.Add(containerElement);
			RefreshExpandedState();
		}

		private void CreateOutputPorts() {
			Button addOutputButton    = new Button(OnAddOutputButton) {text    = "+", style = {fontSize = 10f}};
			Button removeOutputButton = new Button(OnRemoveOutputButton) {text = "-", style = {fontSize = 10f}};
			titleButtonContainer.Add(addOutputButton);
			titleButtonContainer.Add(removeOutputButton);

			Outputs = new Dictionary<string, Port>();

			if (node.children.Count <= 0) { outputContainer.Add(new VisualElement()); } else {
				foreach (BehaviorNodeKVP kvp in node.children) { AddOutputPort(kvp.key); }
			}
		}
	}
}