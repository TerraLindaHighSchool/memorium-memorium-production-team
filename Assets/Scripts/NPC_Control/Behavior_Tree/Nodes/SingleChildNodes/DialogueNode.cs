namespace NPC_Control.Behavior_Tree.Nodes.SingleChildNodes {
	public class DialogueNode : SingleChildNode {
		public string message;

		public override void Run(NPC.NPCDataHelper npcDataHelper) {
			// Displays a standard dialogue box with no options (other than continue)
		}
	}
}