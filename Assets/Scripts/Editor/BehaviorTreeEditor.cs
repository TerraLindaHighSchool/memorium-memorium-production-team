using NPC_Control.Behavior_Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeView _treeView;
    
        [MenuItem("Window/BehaviorTreeEditor")]
        public static void OpenWindow()
        {
            BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviorTreeEditor");
            wnd.Show();
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/BehaviorTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/BehaviorTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            _treeView = root.Q<BehaviorTreeView>();

            _treeView.RefreshEditorWindow += Repaint;

            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            Repaint();
            BehaviorTree tree = Selection.activeObject as BehaviorTree;
            if (tree)
            {
                _treeView.PopulateView(tree);
            }
        }
    }
}