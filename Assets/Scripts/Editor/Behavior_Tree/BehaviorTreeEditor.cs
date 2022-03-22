using NPC_Control.Behavior_Tree;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Behavior_Tree {
	public class BehaviorTreeEditor : EditorWindow {
		private BehaviorTreeView _treeView;

		[MenuItem("Window/BehaviorTreeEditor")]
		public static void OpenWindow() {
			BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
			wnd.titleContent = new GUIContent("BehaviorTreeEditor");
			wnd.Show();
		}

		[OnOpenAsset]
		public static bool OnOpenAsset(int instanceID, int line) {
			Object openedAsset = EditorUtility.InstanceIDToObject(instanceID);

			if (!(openedAsset is BehaviorTree)) return false;

			OpenWindow();
			return true;
		}

		public void CreateGUI() {
			// Each editor window contains a root VisualElement object
			VisualElement root = rootVisualElement;

			// Import UXML
			var visualTree =
				AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
					"Assets/Scripts/Editor/Behavior_Tree/BehaviorTreeEditor.uxml");
			visualTree.CloneTree(root);

			// A stylesheet can be added to a VisualElement.
			// The style will be applied to the VisualElement and all of its children.
			var styleSheet =
				AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Behavior_Tree/BehaviorTreeEditor.uss");
			root.styleSheets.Add(styleSheet);

			_treeView = root.Q<BehaviorTreeView>();

			_treeView.RefreshEditorWindow       += Repaint;
			MultiChildNodeView.RegenerateEditor += OnSelectionChange;
			MapChildNodeView.RegenerateEditor   += OnSelectionChange;

			OnSelectionChange();
		}

		private void OnSelectionChange() {
			Repaint();
			BehaviorTree tree = Selection.activeObject as BehaviorTree;
			_treeView.PopulateView(tree);
		}
	}
}