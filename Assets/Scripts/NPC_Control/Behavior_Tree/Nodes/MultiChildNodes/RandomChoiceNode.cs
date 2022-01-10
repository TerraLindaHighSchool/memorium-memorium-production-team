using Random = UnityEngine.Random;

namespace NPC_Control.Behavior_Tree.Nodes.MultiChildNodes {
	public class RandomChoiceNode : MultiChildNode {
		public override void Run(NPC.NPCDataHelper npcDataHelper) =>
			Complete(children[Random.Range(0, children.Count)]);
	}
}