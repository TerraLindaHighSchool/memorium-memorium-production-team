using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes {
	public abstract class SingleChildNode : BehaviorNode {
		[HideInInspector] public BehaviorNode child;

		public override BehaviorNode[] Children() => new[] { child };
	}
}