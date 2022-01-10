namespace NPC_Control.Behavior_Tree.Nodes.SingleChildNodes {
	public class EventNode : SingleChildNode {
		public string eventKey;

		public override void Run(NPC.NPCDataHelper npcDataHelper) {
			npcDataHelper.InvokeDialogueEvent(eventKey);
			Complete(child);
		}
	}
}