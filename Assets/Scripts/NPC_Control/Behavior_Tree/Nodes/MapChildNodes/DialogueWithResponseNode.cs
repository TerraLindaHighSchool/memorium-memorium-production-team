namespace NPC_Control.Behavior_Tree.Nodes.MapChildNodes {
	public class DialogueWithResponseNode : MapChildNode {
		public string message;

		public override void Run(NPC.NPCDataHelper npcDataHelper) {
			// creates a dialogue box with options, which come from the various keys of the child nodes
		}
	}
}