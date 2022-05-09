using Game_Managing.Game_Context;
using Game_Managing.Game_Context.Cutscene;
using UnityEngine;

namespace NPC_Control.Behavior_Tree.Nodes.SingleChildNodes {
	public class CutsceneNode : SingleChildNode {
		public string key;
		
		public override void Run(NPC.NPCDataHelper npcDataHelper) {
			foreach (CutsceneContextController cutscene in FindObjectsOfType<CutsceneContextController>()) {
				if (cutscene.key.Equals(key)) {
					cutscene.OnExit += () => Complete(child);
					GameContextManager.Instance.EnterContext(cutscene);
					return;
				}
				
				Debug.LogWarning($"{this} could not find a cutscene with key {key}, continuing...");
				Complete(child);
			}
		}
	}
}