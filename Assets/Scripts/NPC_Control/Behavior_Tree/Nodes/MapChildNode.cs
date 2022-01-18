using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes {
	public abstract class MapChildNode : BehaviorNode {
		//TODO: so, it turns out Unity doesn't actually serialize dictionaries after all. 
		//TODO: scream into the endless void
		public Dictionary<string, BehaviorNode> children;

		public string[] amogus;

		public override BehaviorNode[] Children() => children.Values.ToArray();

		public MapChildNode() { children = new Dictionary<string, BehaviorNode>(); }

		public void AddChildToChildren(string key, BehaviorNode value) {
			Debug.Log($"Adding {value} to {this}\'s children");
			children.Add(key, value);
		}

		public void RemoveChildFromChildren(string key) {
			Debug.Log($"Removing key {key} from {this}\'s children");
			children.Remove(key);
		}

		public void UpdateAmogus() {
			string[] newAmogus = children.Keys.ToArray();
			if (amogus != children.Keys.ToArray()) {
				string inAmogusNotNew = "";
				string inNewNotAmogus = "";
				string inBoth         = "";

				foreach (string s in newAmogus.Except(amogus)) { inNewNotAmogus += s; }

				foreach (string s in amogus.Except(newAmogus)) { inAmogusNotNew += s; }

				foreach (string s in amogus.Intersect(newAmogus)) { inBoth += s; }

				if (inAmogusNotNew != "") Debug.Log($"Items to remove: {inAmogusNotNew}");
				if (inNewNotAmogus != "") Debug.Log($"Items to add: {inNewNotAmogus}");
				if (inBoth         != "") Debug.Log($"Items in both: {inBoth}");
			}

			amogus = newAmogus;
		}
	}
}