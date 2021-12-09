using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes {
	public abstract class MapChildNode<T> : BehaviorNode {
		[HideInInspector] public Dictionary<T, BehaviorNode> children;

		public override BehaviorNode[] Children() => children.Values.ToArray();
	}
}