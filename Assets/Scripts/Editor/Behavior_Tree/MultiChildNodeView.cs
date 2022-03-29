using System;
using System.Collections.Generic;
using System.Linq;
using NPC_Control.Behavior_Tree.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Behavior_Tree {
	public sealed class MultiChildNodeView : NodeView<MultiChildNode> {
		public static Action<Edge> OnDisconnectChild;
		public static Action       RegenerateEditor;

		public List<Port> Outputs;

		public MultiChildNodeView(MultiChildNode node) : base(node) {
			CreateInputPort();
			CreateOutputPorts();

			CreateExtension();
		}

		public void AddOutputPort() {
			Port newOutput = CreatePort(Direction.Output, $"Output {Outputs.Count + 1}");

			if (newOutput != null) Outputs.Add(newOutput);
		}

		public void RemoveOutputPort() {
			if (Outputs.Count <= 0) return;

			Port lastPort = Outputs.Last();
			if (lastPort.connected) {
				if (EditorUtility.DisplayDialog("Remove Connected Port",
				                                "This port has a connection. Do you still want to remove it?", "Yes",
				                                "No")) {
					Edge edge = lastPort.connections.First();
					OnDisconnectChild?.Invoke(edge);
					lastPort.Disconnect(edge);
					outputContainer.Remove(lastPort);
					Outputs.Remove(lastPort);
					RegenerateEditor?.Invoke();
				}
			} else {
				outputContainer.Remove(lastPort);
				Outputs.Remove(lastPort);
				RegenerateEditor?.Invoke();
			}
		}

		private void CreateOutputPorts() {
			Outputs = new List<Port>();

			Button addOutputButton    = new Button(AddOutputPort) {text    = "+", style = {fontSize = 10f}};
			Button removeOutputButton = new Button(RemoveOutputPort) {text = "-", style = {fontSize = 10f}};
			titleButtonContainer.Add(addOutputButton);
			titleButtonContainer.Add(removeOutputButton);

			if (node.children.Count <= 0) { outputContainer.Add(new VisualElement()); } else {
				for (int i = 0; i < node.children.Count; i++) {
					Port port = CreatePort(Direction.Output, $"Output {i + 1}");
					if (port != null) Outputs.Add(port);
				}
			}
		}
	}
}