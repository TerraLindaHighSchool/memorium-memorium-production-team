using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes.SingleChildNodes {
	public class RootNode : SingleChildNode {
		public override void Run(NPC.NPCDataHelper npcDataHelper) {
			//Nothing, just starts the tree
			Complete(child);
		}
	}
}