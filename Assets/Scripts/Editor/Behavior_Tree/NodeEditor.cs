using NPC_Control.Behavior_Tree;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.Behavior_Tree {
    [CustomEditor(typeof(BehaviorNode))]
    public class NodeEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            return new VisualElement();
        }
    }
}
