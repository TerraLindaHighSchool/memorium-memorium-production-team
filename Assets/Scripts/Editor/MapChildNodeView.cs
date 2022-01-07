using System.Collections.Generic;
using System.Linq;
using NPC_Control.Behavior_Tree;
using NPC_Control.Behavior_Tree.Nodes;
using UnityEditor.Experimental.GraphView;

namespace Editor {
	public sealed class MapChildNodeView : NodeView<MapChildNode> {
		public Dictionary<string, Port> Outputs;

		public MapChildNodeView(MapChildNode node) : base(node) {
			CreateInputPort();
			CreateOutputPorts();

			CreateExtension();
		}

		public void AddOutputPort(string key) {
			Port newOutput = CreatePort(Direction.Output);
			
			if (newOutput != null) Outputs.Add(key, newOutput);
		}

		public bool RemoveOutputPort() {
			if (Outputs.Count <= 0) return false;

			KeyValuePair<string, Port> kvp = Outputs.Last();
			outputContainer.Remove(kvp.Value);
			Outputs.Remove(kvp.Key);
			return true;
		}

		private void CreateOutputPorts() {
			Outputs = new Dictionary<string, Port>();

			if (node.children.Count <= 0) {
				Port firstOutput = CreatePort(Direction.Output);
				if (firstOutput != null) Outputs.Add("😳", firstOutput);
			} else {
				foreach (KeyValuePair<string, BehaviorNode> kvp in node.children) {
					Port port = CreatePort(Direction.Output);
					if (port != null) Outputs.Add(kvp.Key, port);
				}
			}
		}
	}
}