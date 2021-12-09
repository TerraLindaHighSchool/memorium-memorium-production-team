using System;
using System.Collections.Generic;
using System.Linq;
using NPC_Control.Behavior_Tree;
using NPC_Control.Behavior_Tree.Nodes;
using NPC_Control.Behavior_Tree.Nodes.SingleChildNodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	public class BehaviorTreeView : GraphView {
		public Action<NodeView> OnNodeSelected;

		public Action RefreshEditorWindow;

		public class UXMLFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }

		public BehaviorTree tree;

		public BehaviorTreeView() {
			Insert(0, new GridBackground());

			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/BehaviorTreeEditor.uss");
			styleSheets.Add(styleSheet);
		}

		private NodeView FindNodeView(BehaviorNode node) { return GetNodeByGuid(node.guid) as NodeView; }

		public void PopulateView(BehaviorTree tree) {
			this.tree = tree;

			graphViewChanged -= OnGraphViewChanged;
			DeleteElements(graphElements.ToList());
			graphViewChanged += OnGraphViewChanged;

			foreach (BehaviorNode node in tree.nodes) {
				//Creates node view
				CreateNodeView(node);
			}

			for (int i = 0; i < tree.nodes.Count; i++) {
				BehaviorNode treeNode = tree.nodes[i];

				//Create edges
				List<BehaviorNode> children = tree.GetChildren(treeNode);
				foreach (BehaviorNode child in children) {
					if (child == null || child == treeNode) continue;
					NodeView parentView = FindNodeView(treeNode);
					NodeView childView  = FindNodeView(child);

					AddElement(parentView.output.ConnectTo(childView.input));
				}
			}
		}

		private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange) {
			if (graphviewchange.elementsToRemove != null) {
				foreach (GraphElement graphElement in graphviewchange.elementsToRemove) {
					switch (graphElement) {
						case NodeView nodeView: {
							if (nodeView.node == null) {
								Debug.LogWarning("Please select a tree before trying to delete a node.");
								RefreshEditorWindow?.Invoke();
								continue;
							}

							if (nodeView.node is RootNode) {
								Debug.LogWarning("Attempted to delete the root node of a tree. Don\'t do that.");
								Vector2 nodePos = nodeView.node.position;
								tree.nodes.Remove(nodeView.node);
								tree.rootNode = null;
								CreateNode(typeof(RootNode), nodePos);
								RefreshEditorWindow?.Invoke();
							} else { tree.DeleteNode(nodeView.node); }

							break;
						}
						case Edge edge: {
							NodeView parentView = edge.output.node as NodeView;
							NodeView childView  = edge.input.node as NodeView;
							tree.RemoveChild(parentView?.node, childView?.node);
							break;
						}
					}
				}
			}

			if (graphviewchange.edgesToCreate != null) {
				foreach (Edge edge in graphviewchange.edgesToCreate) {
					NodeView parentView = edge.output.node as NodeView;
					NodeView childView  = edge.input.node as NodeView;
					tree.AddChild(parentView?.node, childView?.node);
				}
			}

			return graphviewchange;
		}

		private void CreateNode(Type type, Vector2 position = new Vector2()) {
			if (!tree) {
				Debug.LogWarning("Please select a tree before trying to create a node.");
				return;
			}

			if (type != typeof(RootNode) && !tree.rootNode) {
				Debug.LogWarning("Tried to add to a tree without a root node. Adding a new root node.");
				CreateNode(typeof(RootNode));
			}

			BehaviorNode node = tree.CreateNode(type);
			if (node == null) return;
			node.position = position;
			CreateNodeView(node);
		}

		private void CreateNodeView(BehaviorNode node) {
			NodeView nodeView = new NodeView(node) {OnNodeSelected = this.OnNodeSelected};
			AddElement(nodeView);
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
				TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<MapChildNode<string>>();
				foreach (Type type in types) {
					if (type?.BaseType == null) continue;
					evt.menu.AppendAction($"[MapChildNodes]/{type.Name}", a => CreateNode(type));
				}
			}
		}
	}
}