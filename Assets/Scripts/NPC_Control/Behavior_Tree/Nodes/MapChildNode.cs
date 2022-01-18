using System;
using System.Collections.Generic;
using Other;
using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes {
	public abstract class MapChildNode : BehaviorNode {
		[HideInInspector] public List<BehaviorNodeKVP> children;

		public override BehaviorNode[] Children() => GetChildrenValues().ToArray();

		public MapChildNode() { children = new List<BehaviorNodeKVP>(); }

		public List<string> GetChildrenKeys() {
			List<string> keys = new List<string>();
			foreach (BehaviorNodeKVP kvp in children) { keys.Add(kvp.key); }

			return keys;
		}

		public List<BehaviorNode> GetChildrenValues() {
			List<BehaviorNode> values = new List<BehaviorNode>();
			foreach (BehaviorNodeKVP kvp in children) { values.Add(kvp.value); }

			return values;
		}

		public Optional<BehaviorNode> GetChildFromKey(string key) {
			foreach (BehaviorNodeKVP kvp in children) {
				if (kvp.key.Equals(key)) return new Optional<BehaviorNode>(kvp.value, true);
			}

			return new Optional<BehaviorNode>(null, false);
		}

		public void AddChildToChildren(string key, BehaviorNode value) {
			children.Add(new BehaviorNodeKVP {key = key, value = value});
		}

		public void RemoveChildFromChildren(string key) {
			foreach (BehaviorNodeKVP kvp in children) {
				if (key.Equals(kvp.key)) {
					children.Remove(kvp);
					return;
				}
			}
		}

		public static Optional<string> FindKeyForBehaviorNodeInChildren(MapChildNode node, BehaviorNode child) {
			foreach (BehaviorNodeKVP kvp in node.children) {
				if (kvp.value.Equals(child)) { return new Optional<string>(kvp.key, true); }
			}

			return new Optional<string>(null, false);
		}
	}

	[Serializable]
	public struct BehaviorNodeKVP {
		public string       key;
		public BehaviorNode value;
	}
}