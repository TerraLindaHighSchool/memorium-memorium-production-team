using System.Collections.Generic;
using System.Linq;
using NPC_Control.Behavior_Tree.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Editor {
	public sealed class MultiChildNodeView : NodeView<MultiChildNode> {
		public List<Port> Outputs;

		public MultiChildNodeView(MultiChildNode node) : base(node) {
			CreateInputPort();
			CreateOutputPorts();

			CreateExtension();
		}

		public void AddOutputPort() {
			Port newOutput = CreatePort(Direction.Output);
			
			if (newOutput != null) Outputs.Add(newOutput);
		}

		public bool RemoveOutputPort() {
			if (Outputs.Count <= 0) return false;
			
			Port lastPort = Outputs.Last();
			outputContainer.Remove(lastPort);
			Outputs.Remove(lastPort);
			return true;
		}

		private void CreateOutputPorts() {
			Outputs = new List<Port>();

			if (node.children.Count <= 0) {
				Port firstOutput = CreatePort(Direction.Output);
				if (firstOutput != null) Outputs.Add(firstOutput);
			} else {
				for (int i = 0; i < node.children.Count; i++) {
					Port port = CreatePort(Direction.Output);
					if (port != null) Outputs.Add(port);
				}
			}
		}
	}
}