namespace NPC_Control.Behavior_Tree.Nodes.MultiChildNodes {
	public class OrderedChoiceNode : MultiChildNode {
		private int choiceIndex = 0;

		public override void Run(NPC.NPCDataHelper npcDataHelper) =>
			Complete(children[choiceIndex++ % children.Count]);
	}
}