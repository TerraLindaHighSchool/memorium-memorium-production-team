using System;
using System.Collections.Generic;
using NPC_Control.Behavior_Tree.Nodes;
using NPC_Control.Behavior_Tree.Nodes.SingleChildNodes;
using UnityEditor;
using UnityEngine;

namespace NPC_Control.Behavior_Tree {
	[CreateAssetMenu()]
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

				rootNode = (RootNode) node;
			} else { node = (BehaviorNode) CreateInstance(type); }

			node.guid = Guid.NewGuid().ToString();

			nodes.Add(node);

			//AssetDatabase things
			AssetDatabase.AddObjectToAsset(node, this);
			AssetDatabase.SaveAssets();

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
			AssetDatabase.SaveAssets();
		}

		public List<BehaviorNode> GetChildren(BehaviorNode node) {
			List<BehaviorNode> returnList = new List<BehaviorNode>();

			if (node is SingleChildNode singleChildNode) { returnList.Add(singleChildNode.child); } else if (
				node is MultiChildNode multiChildNode) {
				foreach (BehaviorNode child in multiChildNode.children) { returnList.Add(child); }
			} else if (node is MapChildNode<string> mapChildNode) {
				foreach (KeyValuePair<string, BehaviorNode> kvp in mapChildNode.children) { returnList.Add(kvp.Value); }
			}

			return returnList;
		}

		public void AddChild(BehaviorNode parent, BehaviorNode child) {
			if (parent is SingleChildNode singleChildNode) { singleChildNode.child = child; } else if (
				parent is MultiChildNode multiChildNode) { multiChildNode.children.Add(child); } else if (
				parent is MapChildNode<string> mapChildNode) {
				//uhhhh idk rn
			}
		}

		public void RemoveChild(BehaviorNode parent, BehaviorNode child) {
			{
				RootNode parentNode = parent as RootNode;
				if (parentNode != null) {
					parentNode.child = null;
					return;
				}
			}

			{
				DebugLogNode parentNode = parent as DebugLogNode;
				if (parentNode != null) {
					parentNode.child = null;
					return;
				}
			}

			if (parent is SingleChildNode singleChildNode) { singleChildNode.child = null; } else if (
				parent is MultiChildNode multiChildNode) { multiChildNode.children.Remove(child); } else if (
				parent is MapChildNode<string>) {
				//really no idea
			}
		}
	}
}