using System;
using System.Collections.Generic;
using System.Linq;
using Game_Managing.Game_Context;
using NPC_Control.Behavior_Tree;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	[CustomEditor(typeof(CutsceneContextController))]
	public class CutsceneControllerEditor : UnityEditor.Editor {
		private CutsceneContextController _obj;

		private VisualTreeAsset _eventTemplate;

		private VisualElement _listView;

		public override VisualElement CreateInspectorGUI() {
			_obj = target as CutsceneContextController;

			_eventTemplate =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/TimedCutsceneEvent.uxml");

			VisualElement rootElement = new VisualElement();

			VisualTreeAsset visualTree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/CutsceneControllerEditor.uxml");
			visualTree.CloneTree(rootElement);

			rootElement.styleSheets.Add(
				AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/CutsceneControllerEditor.uss"));

			VisualElement buttonContainer = rootElement.ElementAt(0);
			_listView        = rootElement.ElementAt(1);

			Button addEventButton    = buttonContainer.ElementAt(1) as Button;
			Button removeEventButton = buttonContainer.ElementAt(0) as Button;

			addEventButton.clicked    += AddEvent;
			removeEventButton.clicked += RemoveEvent;

			if (_obj.events == null) _obj.events = new Dictionary<string, TimedCutsceneEvent>();
			
			foreach (KeyValuePair<string, TimedCutsceneEvent> kvp in _obj.events) {
				_listView.Add(CreateEventElement(kvp.Value));
			}

			return rootElement;
		}

		private void AddEvent() {
			VisualElement newEventElement = CreateEventElement(_obj.AddTimedEvent());
			_listView.Add(newEventElement);
		}

		private VisualElement CreateEventElement(TimedCutsceneEvent timedEvent) {
			VisualElement      newEventElement = NodeView<BehaviorNode>.CreateUIElementInspector(timedEvent, null);
			newEventElement.viewDataKey = timedEvent.guid;
			return newEventElement;
		}

		private void RemoveEvent() {
			if (_listView.childCount <= 0 || _obj.events.Count <= 0) return;

			VisualElement lastEventElement = _listView.Children().Last();
			_listView.Remove(lastEventElement);
			
			_obj.RemoveTimedEvent();
		}
	}
}