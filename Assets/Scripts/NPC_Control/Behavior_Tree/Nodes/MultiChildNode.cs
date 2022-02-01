using System.Collections.Generic;
using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes {
	public abstract class MultiChildNode : BehaviorNode {
		[HideInInspector] public List<BehaviorNode> children;

		public override BehaviorNode[] Children() => children.ToArray();

		public MultiChildNode() {
			children = new List<BehaviorNode>();
		}
	}
}