using NPC_Control.Behavior_Tree.Nodes;
using NPC_Control.Behavior_Tree.Nodes.SingleChildNodes;
using UnityEditor.Experimental.GraphView;

namespace Editor {
	public sealed class SingleChildNodeView : NodeView<SingleChildNode> {
		public Port Output;

		public SingleChildNodeView(SingleChildNode node) : base(node) {
			CreateInputPort();
			CreateOutputPort();

			CreateExtension();
		}

		protected override void CreateInputPort() {
			if (node is RootNode) {
				//nothing, a root node has no inputs
			} else { Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, null); }

			if (Input != null) {
				Input.portName = "Input";
				inputContainer.Add(Input);
			}
		}

		private void CreateOutputPort() {
			Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, null);

			if (Output != null) {
				Output.portName = "Output";
				outputContainer.Add(Output);
			}
		}
	}
}