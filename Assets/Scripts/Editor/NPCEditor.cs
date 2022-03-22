using System.Collections.Generic;
using System.Linq;
using NPC_Control;
using NPC_Control.Behavior_Tree;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	[CustomEditor(typeof(NPC))]
	public class NPCEditor : UnityEditor.Editor {
		private NPC _npc;

		private VisualElement _listView;

		public override VisualElement CreateInspectorGUI() {
			_npc = (NPC) target;

			VisualElement rootElement = new VisualElement();

			VisualTreeAsset visualTree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/NPCEditor.uxml");
			visualTree.CloneTree(rootElement);

			rootElement.styleSheets.Add(
				AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/NPCEditor.uss"));

			VisualElement propertyContainer = rootElement.ElementAt(0);
			ObjectField   treeObjectField   = propertyContainer.ElementAt(0) as ObjectField;
			treeObjectField.objectType = typeof(BehaviorTree);

			VisualElement eventReceiverContainer = rootElement.ElementAt(1);

			VisualElement buttonContainer = eventReceiverContainer.ElementAt(1);
			_listView = eventReceiverContainer.ElementAt(2);

			Button addEventReceiverButton    = buttonContainer.ElementAt(1) as Button;
			Button removeEventReceiverButton = buttonContainer.ElementAt(0) as Button;

			addEventReceiverButton.clicked    += AddEventReceiver;
			removeEventReceiverButton.clicked += RemoveEventReceiver;
			
			Button debugButton = buttonContainer.ElementAt(2) as Button;
			debugButton.clicked += _npc.DebugEventReceivers;
			
			foreach (EventReceiver eventReceiver in _npc.eventReceivers) {
				_listView.Add(CreateEventReceiverElement(eventReceiver));
			}

			return rootElement;
		}

		private void AddEventReceiver() {
			EventReceiver newEventReceiver        = _npc.AddEventReceiver();
			VisualElement newEventReceiverElement = CreateEventReceiverElement(newEventReceiver);
			_listView.Add(newEventReceiverElement);
		}

		private void RemoveEventReceiver() {
			if (_listView.childCount <= 0 || _npc.eventReceivers.Count <= 0) return;

			VisualElement lastEventElement = _listView.Children().Last();
			_listView.Remove(lastEventElement);

			_npc.RemoveEventReceiver();
		}

		private VisualElement CreateEventReceiverElement(EventReceiver eventReceiver) {
			VisualElement newEventReceiverElement = UIElementsExtensions.CreateUIElementInspector(eventReceiver);
			newEventReceiverElement.viewDataKey = eventReceiver.guid;
			StyleColor  borderColor = Color.gray;
			StyleFloat  borderWidth = 5f;
			StyleLength marginWidth = 3f;

			newEventReceiverElement.style.borderBottomWidth = borderWidth;
			newEventReceiverElement.style.borderTopWidth    = borderWidth;
			newEventReceiverElement.style.borderLeftWidth   = borderWidth;
			newEventReceiverElement.style.borderRightWidth  = borderWidth;

			newEventReceiverElement.style.borderTopColor    = borderColor;
			newEventReceiverElement.style.borderBottomColor = borderColor;
			newEventReceiverElement.style.borderLeftColor   = borderColor;
			newEventReceiverElement.style.borderRightColor  = borderColor;

			newEventReceiverElement.style.marginTop    = marginWidth;
			newEventReceiverElement.style.marginBottom = marginWidth;
			newEventReceiverElement.style.marginLeft   = marginWidth;
			newEventReceiverElement.style.marginRight  = marginWidth;

			return newEventReceiverElement;
		}
	}
}