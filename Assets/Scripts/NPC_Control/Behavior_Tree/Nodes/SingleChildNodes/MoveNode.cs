using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes.SingleChildNodes {
	public class MoveNode : SingleChildNode {
		public Vector3 target;

		public override void Run(NPC.NPCDataHelper npcDataHelper) {
			// tells the npcDataHelper._outerObj NPC to move to the target position using its navmesh
		}
	}
}