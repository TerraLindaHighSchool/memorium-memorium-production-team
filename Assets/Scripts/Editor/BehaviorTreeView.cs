using System;
using System.Collections.Generic;
using System.Linq;
using NPC_Control.Behavior_Tree;
using NPC_Control.Behavior_Tree.Nodes;
using NPC_Control.Behavior_Tree.Nodes.SingleChildNodes;
using Other;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Editor {
	public class BehaviorTreeView : GraphView {
		public Action RefreshEditorWindow;

		public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }

		public BehaviorTree Tree;

		public BehaviorTreeView() {
			Insert(0, new GridBackground());

			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			StyleSheet styleSheet =
				AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/BehaviorTreeEditor.uss");
			styleSheets.Add(styleSheet);

			MultiChildNodeView.OnDisconnectChild += RemoveChildrenFromEdge;
			MapChildNodeView.OnDisconnectChild   += RemoveChildrenFromEdge;
		}

		private NodeView<T> FindNodeView<T>(T node) where T : BehaviorNode {
			return GetNodeByGuid(node?.guid) as NodeView<T>;
		}

		public void PopulateView(BehaviorTree tree) {
			Tree = tree;

			graphViewChanged -= OnGraphViewChanged;
			DeleteElements(graphElements.ToList());
			graphViewChanged += OnGraphViewChanged;

			if (!Tree) return;

			foreach (BehaviorNode node in Tree.nodes) {
				//Creates node view
				CreateNodeView(node);
			}

			foreach (BehaviorNode treeNode in Tree.nodes) {
				//Create edges
				List<BehaviorNode> children = Tree.GetChildren(treeNode);
				for (int i = 0; i < children.Count; i++) {
					BehaviorNode child = children[i];
					if (child == null || child == treeNode) continue;

					SingleChildNodeView singleChildView = (SingleChildNodeView) FindNodeView(child as SingleChildNode);
					MultiChildNodeView  multiChildView  = (MultiChildNodeView) FindNodeView(child as MultiChildNode);
					MapChildNodeView    mapChildView    = (MapChildNodeView) FindNodeView(child as MapChildNode);

					if (singleChildView != null) { CreateEdge(Tree, treeNode, child as SingleChildNode, i); } else if (
						multiChildView  != null) { CreateEdge(Tree, treeNode, child as MultiChildNode, i); } else if (
						mapChildView    != null) { CreateEdge(Tree, treeNode, child as MapChildNode, i); } else {
						Debug.LogError(
							$"When adding an edge to connect {treeNode} to {child}, the child had an unknown type.");
					}
				}
			}

			BehaviorTree.Save();
		}

		/// <summary>
		/// This method is quite terrible. 
		/// </summary>
		/// <param name="tree">The behavior tree</param>
		/// <param name="treeNode"></param>
		/// <param name="child"></param>
		/// <param name="childIndex"></param>
		/// <typeparam name="T"></typeparam>
		private void CreateEdge<T>(BehaviorTree tree, BehaviorNode treeNode, T child, int childIndex)
			where T : BehaviorNode {
			NodeView<T> childView = FindNodeView(child);
			switch (treeNode) {
				case SingleChildNode node: {
					SingleChildNodeView parentView = (SingleChildNodeView) FindNodeView(node);
					AddElement(parentView.Output.ConnectTo(childView.Input));
					break;
				}
				case MultiChildNode node: {
					MultiChildNodeView parentView = (MultiChildNodeView) FindNodeView(node);

					if (parentView.Outputs.Count < node.children.Count) {
						Debug.LogError(
							$"NodeView for {node} had less outputs than the node had children. Removing excess children.");
						while (parentView.Outputs.Count < node.children.Count) {
							tree.RemoveChild(node, node.children.Last());
							Debug.LogWarning(
								$"Removed {node}\'s child while in the less outputs error fix. If this goes on for a while, something is going very wrong.");
						}
					}

					for (int j = 0; j < parentView.Outputs.Count; j++) {
						if (j == childIndex) {
							AddElement(parentView.Outputs[j].ConnectTo(childView.Input));
							break;
						}
					}

					break;
				}
				case MapChildNode node: {
					MapChildNodeView parentView = (MapChildNodeView) FindNodeView(node);

					if (parentView.Outputs.Count < node.children.Count) {
						Debug.LogError(
							$"NodeView for {node} had less outputs than the node had children. Removing excess children.");
						while (parentView.Outputs.Count < node.children.Count) {
							tree.RemoveChild(node, node.children.Last().Value);
							Debug.LogWarning(
								$"Removed {node}\'s child while in the less outputs error fix. If this goes on for a while, something is going very wrong.");
						}
					}

					string childKey = "";

					Optional<string> possibleChildKey = FindKeyInDictionary(node.children, child);
					if (!possibleChildKey.Enabled) {
						Debug.LogWarning($"Child {child} did not have a key in its parents dictionary.");
					} else { childKey = possibleChildKey.Value; }

					bool foundPortMatch = false;

					foreach (KeyValuePair<string, Port> portKVP in parentView.Outputs) {
						if (portKVP.Key.Equals(childKey)) {
							AddElement(portKVP.Value.ConnectTo(childView.Input));
							foundPortMatch = true;
							break;
						}
					}

					if (!foundPortMatch) Debug.LogError($"Could not find a port on {node} for child key {childKey}");

					break;
				}
			}
		}

		private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange) {
			if (graphviewchange.elementsToRemove != null) {
				foreach (GraphElement graphElement in graphviewchange.elementsToRemove) {
					switch (graphElement) {
						case SingleChildNodeView singleChildNodeView: {
							if (singleChildNodeView.node == null) {
								Debug.LogWarning("Please select a tree before trying to delete a node.");
								RefreshEditorWindow?.Invoke();
								continue;
							}

							if (singleChildNodeView.node is RootNode) {
								Debug.LogWarning("Attempted to delete the root node of a tree. Don\'t do that.");
								Vector2 nodePos = singleChildNodeView.node.position;
								Tree.nodes.Remove(singleChildNodeView.node);
								Tree.rootNode = null;
								CreateNode(typeof(RootNode), nodePos);
								RefreshEditorWindow?.Invoke();
							} else { Tree.DeleteNode(singleChildNodeView.node); }

							break;
						}
						case MultiChildNodeView multiChildNodeView: {
							if (multiChildNodeView.node == null) {
								Debug.LogWarning("Please select a tree before trying to delete a node.");
								RefreshEditorWindow?.Invoke();
								continue;
							}

							Tree.DeleteNode(multiChildNodeView.node);

							break;
						}
						case MapChildNodeView mapChildNodeView: {
							if (mapChildNodeView.node == null) {
								Debug.LogWarning("Please select a tree before trying to delete a node.");
								RefreshEditorWindow?.Invoke();
								continue;
							}

							Tree.DeleteNode(mapChildNodeView.node);

							break;
						}
						case Edge edge: {
							RemoveChildrenFromEdge(edge);
							break;
						}
					}
				}
			}

			if (graphviewchange.edgesToCreate != null) {
				foreach (Edge edge in graphviewchange.edgesToCreate) { AssignChildrenFromEdge(edge); }
			}

			BehaviorTree.Save();

			return graphviewchange;
		}

		private void AssignChildrenFromEdge(Edge edge) {
			switch (edge.output.node) {
				case SingleChildNodeView parentView: {
					switch (edge.input.node) {
						case SingleChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node);
							break;
						}
						case MultiChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node);
							break;
						}
						case MapChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node);
							break;
						}
					}

					break;
				}
				case MultiChildNodeView parentView: {
					switch (edge.input.node) {
						case SingleChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node);
							break;
						}
						case MultiChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node);
							break;
						}
						case MapChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node);
							break;
						}
					}

					break;
				}
				case MapChildNodeView parentView: {
					Optional<string> possiblePortKey = FindKeyInDictionary(parentView.Outputs, edge.output);
					if (!possiblePortKey.Enabled) {
						Debug.LogWarning($"Port {edge.output} not found in {parentView}\'s outputs.");
						return;
					}

					switch (edge.input.node) {
						case SingleChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node, possiblePortKey.Value);
							parentView.node.UpdateAmogus();
							break;
						}
						case MultiChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node, possiblePortKey.Value);
							parentView.node.UpdateAmogus();
							break;
						}
						case MapChildNodeView childView: {
							Tree.AddChild(parentView.node, childView.node, possiblePortKey.Value);
							parentView.node.UpdateAmogus();
							break;
						}
					}

					break;
				}
			}
		}

		private void RemoveChildrenFromEdge(Edge edge) {
			if (Selection.activeObject as BehaviorTree != null) { Tree = Selection.activeObject as BehaviorTree; }

			switch (edge.output.node) {
				case SingleChildNodeView parentView: {
					switch (edge.input.node) {
						case SingleChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");
							break;
						}
						case MultiChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");

							break;
						}
						case MapChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");

							break;
						}
					}

					break;
				}
				case MultiChildNodeView parentView: {
					switch (edge.input.node) {
						case SingleChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");

							break;
						}
						case MultiChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");

							break;
						}
						case MapChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");

							break;
						}
					}

					break;
				}
				case MapChildNodeView parentView: {
					Optional<string> possiblePortKey = FindKeyInDictionary(parentView.Outputs, edge.output);
					if (!possiblePortKey.Enabled) {
						Debug.LogWarning($"Port {edge.output} not found in {parentView}\'s outputs.");
						return;
					}

					switch (edge.input.node) {
						case SingleChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node, possiblePortKey.Value);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");
							parentView.node.UpdateAmogus();
							break;
						}
						case MultiChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node, possiblePortKey.Value);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");
							parentView.node.UpdateAmogus();
							break;
						}
						case MapChildNodeView childView: {
							Tree.RemoveChild(parentView.node, childView.node, possiblePortKey.Value);
							Debug.Log($"Removed child {childView.node} from {parentView.node}");
							parentView.node.UpdateAmogus();
							break;
						}
					}

					break;
				}
			}
		}

		private void CreateNode(Type type, Vector2 position = new Vector2()) {
			if (!Tree) {
				Debug.LogWarning("Please select a tree before trying to create a node.");
				return;
			}

			if (type != typeof(RootNode) && !Tree.rootNode) {
				Debug.LogWarning("Tried to add to a tree without a root node. Adding a new root node.");
				CreateNode(typeof(RootNode));
			}

			BehaviorNode node = Tree.CreateNode(type);
			if (node == null) return;
			node.position = position;

			CreateNodeView(node);
		}

		private void CreateNodeView(BehaviorNode node) {
			SingleChildNodeView singleChildNodeView = null;
			MultiChildNodeView  multiChildNodeView  = null;
			MapChildNodeView    mapChildNodeView    = null;

			switch (node) {
				case SingleChildNode singleChildNode: {
					singleChildNodeView = new SingleChildNodeView(singleChildNode);
					break;
				}
				case MultiChildNode multiChildNode: {
					multiChildNodeView = new MultiChildNodeView(multiChildNode);
					break;
				}
				case MapChildNode mapChildNode: {
					mapChildNodeView = new MapChildNodeView(mapChildNode);
					break;
				}
			}

			if (singleChildNodeView != null) { AddElement(singleChildNodeView); } else if (
				multiChildNodeView  != null) { AddElement(multiChildNodeView); } else if (mapChildNodeView != null) {
				AddElement(mapChildNodeView);
			} else { Debug.LogError($"When creating a node view for {node}, no generic node type was recognized."); }
		}

		public static Optional<TKey> FindKeyInDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict, TValue obj) {
			foreach (KeyValuePair<TKey, TValue> kvp in dict) {
				if (kvp.Value.Equals(obj)) return new Optional<TKey>(kvp.Key, true);
			}

			return new Optional<TKey>();
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
			return ports.ToList()
			            .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node)
			            .ToList();
		}

		public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
			{
				TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<SingleChildNode>();
				foreach (Type type in types) {
					if (type?.BaseType == null) continue;
					evt.menu.AppendAction($"[SingleChildNodes]/{type.Name}", a => CreateNode(type));
				}
			}

			{
				TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<MultiChildNode>();
				foreach (Type type in types) {
					if (type?.BaseType == null) continue;
					evt.menu.AppendAction($"[MultiChildNodes]/{type.Name}", a => CreateNode(type));
				}
			}

			{
				TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<MapChildNode>();
				foreach (Type type in types) {
					if (type?.BaseType == null) continue;
					evt.menu.AppendAction($"[MapChildNodes]/{type.Name}", a => CreateNode(type));
				}
			}
		}
	}
}