using System;
using System.Collections.Generic;
using System.Reflection;
using NPC_Control.Behavior_Tree;
using Other;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	[Serializable]
	public abstract class NodeView<T> : Node where T : BehaviorNode {
		public static Action SaveTree;
		
		public Action<NodeView<T>> OnNodeSelected;

		public T node;

		public Port Input;

		public NodeView(T node) {
			this.node   = node;
			viewDataKey = node.guid;

			title = node.GetType().Name;

			style.left = node.position.x;
			style.top  = node.position.y;
		}

		protected virtual void CreateExtension() {
			VisualElement containerElement =
				new VisualElement {style = {backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f))}};

			VisualElement nodeBody = UIElementsExtensions.CreateUIElementInspector(node);
			containerElement.Add(nodeBody);

			extensionContainer.Add(containerElement);
			RefreshExpandedState();
		}

		protected virtual void CreateInputPort() {
			Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, null);

			if (Input != null) {
				Input.portName = "Input";
				inputContainer.Add(Input);
			}
		}
		
		protected Port CreatePort(Direction dir, string portName = "") {
			Port port = InstantiatePort(Orientation.Horizontal, dir, Port.Capacity.Single, null);

			if (port != null) {
				if (dir == Direction.Input) {
					port.portName = portName == "" ? "Input" : portName;
					inputContainer.Add(port);
				} else {
					port.portName = portName == "" ? "Output" : portName;
					outputContainer.Add(port);
				}
			}

			return port;
		}

		public override void SetPosition(Rect newPos) {
			base.SetPosition(newPos);
			node.position.x = newPos.xMin;
			node.position.y = newPos.yMin;
		}

		public override void OnSelected() {
			base.OnSelected();
			OnNodeSelected?.Invoke(this);
		}
	}
}