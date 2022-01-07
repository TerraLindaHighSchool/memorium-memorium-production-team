using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes {
	public abstract class MapChildNode : BehaviorNode {
		[HideInInspector] public Dictionary<string, BehaviorNode> children;

		public override BehaviorNode[] Children() => children.Values.ToArray();
	}
}