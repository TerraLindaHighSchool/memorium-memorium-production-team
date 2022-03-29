using UnityEditor;
using UnityEngine;

namespace Editor.Behavior_Tree {
	public class AddOutputMenu : EditorWindow {
		private string _newOutputKey = "New Output Port Key";

		private static MapChildNodeView _selectedNode;
		
		public static void Init(MapChildNodeView node) {
			_selectedNode = node;
			AddOutputMenu window   = CreateInstance<AddOutputMenu>();
			Vector2       mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			window.position = new Rect(mousePos.x, mousePos.y, 250, 100);
			window.ShowPopup();
		}

		void OnGUI() {
			GUILayout.BeginHorizontal();
			_newOutputKey = GUI.TextField(new Rect(10, 10, 200, 20), _newOutputKey, 25);
			GUILayout.Space(220);
			if (GUILayout.Button("X")) { Close(); }
			GUILayout.EndHorizontal();
			
			GUILayout.Space(50);
			if (GUILayout.Button("Enter")) {
				_selectedNode.AddOutputPort(_newOutputKey);
				Close();
			}
		}
	}
}