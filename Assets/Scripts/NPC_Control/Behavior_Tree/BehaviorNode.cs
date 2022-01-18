using System;
using UnityEngine;

namespace NPC_Control.Behavior_Tree {
	public abstract class BehaviorNode : ScriptableObject {
		[HideInInspector] public string guid;
		[HideInInspector] public Vector2 position;

		public event Action<BehaviorNode, BehaviorNode> OnCompleted;

		protected void Complete(BehaviorNode successor) { OnCompleted?.Invoke(this, successor); }

		public abstract void Run(NPC.NPCDataHelper npcDataHelper);

		public abstract BehaviorNode[] Children();
	}

	[Serializable]
	public class EntityController { }

	[Serializable]
	public class CutsceneManager { }
}