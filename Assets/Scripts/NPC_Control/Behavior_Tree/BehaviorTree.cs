using System;
using System.Collections.Generic;
using NPC_Control.Behavior_Tree.Nodes;
using NPC_Control.Behavior_Tree.Nodes.SingleChildNodes;
using UnityEditor;
using UnityEngine;

namespace NPC_Control.Behavior_Tree {
	[CreateAssetMenu]
	public class BehaviorTree : ScriptableObject {
		[HideInInspector] public RootNode rootNode;

		[HideInInspector] public List<BehaviorNode> nodes = new List<BehaviorNode>();

		public BehaviorNode CreateNode(Type type) {
			BehaviorNode node;

			if (type == typeof(RootNode)) {
				if (rootNode != null) {
					Debug.LogWarning(
						"Do not attempt to add a root node to a tree that already has one. If this is not the case, please tell someone.");
					return null;
				}

				node = CreateInstance<RootNode>();
				
				node.position = Vector2.zero;

				rootNode = (RootNode) node;
			} else { node = (BehaviorNode) CreateInstance(type); }

			node.guid = Guid.NewGuid().ToString();

			nodes.Add(node);

			//AssetDatabase things
			AssetDatabase.AddObjectToAsset(node, this);
			Save();

			return node;
		}

		public void DeleteNode(BehaviorNode node) {
			if (node is RootNode) {
				Debug.LogWarning(
					"Attempted to delete the root node of the tree. Don't do that. Adding back a new one.");
				nodes.Remove(node);
				rootNode = null;
				CreateNode(typeof(RootNode));
				return;
			}

			nodes.Remove(node);

			//AssetDatabase things
			AssetDatabase.RemoveObjectFromAsset(node);
			Save();
		}

		public void AddChild(BehaviorNode parent, BehaviorNode child, string key = "") {
			if (parent is SingleChildNode singleChildNode) { singleChildNode.child = child; } else if (
				parent is MultiChildNode multiChildNode) { multiChildNode.children.Add(child); } else if (
				parent is MapChildNode mapChildNode) {
				if (key == "") {
					Debug.LogError($"Tried to add child {child} to {parent}, but no key was provided.");
					return;
				}
				mapChildNode.AddChildToChildren(key, child);
			}
		}

		public void RemoveChild(BehaviorNode parent, BehaviorNode child, string key = "") {
			if (parent is SingleChildNode singleChildNode) { singleChildNode.child = null; } else if (
				parent is MultiChildNode multiChildNode) { multiChildNode.children.Remove(child); } else if (
				parent is MapChildNode mapChildNode) {
				if (key == "") {
					Debug.LogError($"Tried to add child {child} to {parent}, but no key was provided.");
					return;
				}
				mapChildNode.RemoveChildFromChildren(key);
			}
		}

		public void Save() {
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
		}
	}
}