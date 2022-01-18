using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes.SingleChildNodes {
	public class DebugLogNode : SingleChildNode {
		public string message;

		public override void Run(NPC.NPCDataHelper npcDataHelper) {
			Debug.Log(message);
			Complete(child);
		}
	}
}