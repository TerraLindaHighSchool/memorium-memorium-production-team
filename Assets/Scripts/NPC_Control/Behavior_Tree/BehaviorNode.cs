using System;
using UnityEngine;

namespace NPC_Control.Behavior_Tree {
	public abstract class BehaviorNode : ScriptableObject {
		[HideInInspector] public string  guid;
		[HideInInspector] public Vector2 position;

		public event Action<BehaviorNode, BehaviorNode> OnCompleted;

		public event Action OnError;

		protected void Complete(BehaviorNode successor) { OnCompleted?.Invoke(this, successor); }

		public void Error(string e) {
			Debug.LogWarning($"Node {this} exited with error: {e}");
			OnError?.Invoke();
		}

		public abstract void Run(NPC.NPCDataHelper npcDataHelper);

		public abstract BehaviorNode[] Children();
	}

	[Serializable]
	public class EntityController { }

	[Serializable]
	public class CutsceneManager { }
}