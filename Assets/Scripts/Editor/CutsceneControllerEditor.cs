using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Game_Managing.Game_Context.Cutscene;
using Other;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
	[CustomEditor(typeof(CutsceneContextController))]
	public class CutsceneControllerEditor : UnityEditor.Editor {
		private CutsceneContextController _cutscene;

		private VisualElement _listView;

		public override VisualElement CreateInspectorGUI() {
			_cutscene = (CutsceneContextController) target;

			//_cutscene.Awake();

			VisualElement rootElement = new VisualElement();

			VisualTreeAsset visualTree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/CutsceneControllerEditor.uxml");
			visualTree.CloneTree(rootElement);

			rootElement.styleSheets.Add(
				AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/CutsceneControllerEditor.uss"));

			VisualElement buttonContainer = rootElement.ElementAt(1);
			_listView = rootElement.ElementAt(2);

			Button addEventButton    = buttonContainer.ElementAt(1) as Button;
			Button removeEventButton = buttonContainer.ElementAt(0) as Button;

			addEventButton.clicked    += AddEvent;
			removeEventButton.clicked += RemoveEvent;

			if (_cutscene.events == null) _cutscene.events = new List<TimedCutsceneEvent>();

			foreach (TimedCutsceneEvent cutsceneEvent in _cutscene.events) {
				_listView.Add(CreateEventElement(cutsceneEvent));
			}

			return rootElement;
		}

		private void AddEvent() {
			TimedCutsceneEvent newEvent        = _cutscene.AddTimedEvent();
			VisualElement      newEventElement = CreateEventElement(newEvent);
			_listView.Add(newEventElement);
		}

		private VisualElement CreateEventElement(TimedCutsceneEvent timedEvent) {
			VisualElement newEventElement = UIElementsExtensions.CreateUIElementInspector(timedEvent);
			newEventElement.viewDataKey = timedEvent.guid;
			StyleColor  borderColor = Color.gray;
			StyleFloat  borderWidth = 5f;
			StyleLength marginWidth = 3f;

			newEventElement.style.borderTopWidth    = borderWidth;
			newEventElement.style.borderBottomWidth = borderWidth;
			newEventElement.style.borderLeftWidth   = borderWidth;
			newEventElement.style.borderRightWidth  = borderWidth;

			newEventElement.style.borderTopColor    = borderColor;
			newEventElement.style.borderBottomColor = borderColor;
			newEventElement.style.borderLeftColor   = borderColor;
			newEventElement.style.borderRightColor  = borderColor;

			newEventElement.style.marginTop    = marginWidth;
			newEventElement.style.marginBottom = marginWidth;
			newEventElement.style.marginLeft   = marginWidth;
			newEventElement.style.marginRight  = marginWidth;

			return newEventElement;
		}

		private void RemoveEvent() {
			if (_listView.childCount <= 0 || _cutscene.events.Count <= 0) return;

			VisualElement lastEventElement = _listView.Children().Last();
			_listView.Remove(lastEventElement);

			_cutscene.RemoveTimedEvent();
		}
	}
}