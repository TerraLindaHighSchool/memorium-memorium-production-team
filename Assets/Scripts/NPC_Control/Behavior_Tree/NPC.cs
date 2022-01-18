using System.Collections.Generic;
using NPC_Control.Dialogue;
using UnityEngine;

namespace NPC_Control.Behavior_Tree {
	[RequireComponent(typeof(EntityController))]
	public class NPC : MonoBehaviour {
		public class NPCDataHelper {
			private readonly NPC _outerObj;

			public readonly EntityController entityController;
			public readonly DialogueManager  dialogueManager;
			public readonly CutsceneManager  cutsceneManager;

			public NPCDataHelper(NPC              outerObj,
			                     EntityController entityController,
			                     DialogueManager  dialogueManager,
			                     CutsceneManager  cutsceneManager) {
				this._outerObj        = outerObj;
				this.entityController = entityController;
				this.dialogueManager  = dialogueManager;
				this.cutsceneManager  = cutsceneManager;
			}

			public void InvokeDialogueEvent(string eventKey) => _outerObj.InvokeEventReceivers(eventKey);
		}

		[SerializeField] private BehaviorTree tree;

		[SerializeField]
		Dictionary<string, IEventReceiver[]> eventReceivers = new Dictionary<string, IEventReceiver[]>();

		private NPCDataHelper _npcDataHelper;

		public bool DialogueActive { get; private set; }

		private void OnEnable() {
			_npcDataHelper = new NPCDataHelper(this, null, DialogueManager.Instance, null); //TODO: NOT THIS
			DialogueActive = false;
		}

		private void OnDisable() { DialogueActive = false; }

		private void InvokeEventReceivers(string eventKey) {
			if (eventReceivers[eventKey] == null) {
				Debug.LogWarning("event key on NPC component is null");
				return;
			}

			foreach (var eventReceiver in eventReceivers[eventKey]) { eventReceiver.OnEventPublish(); }
		}

		public void StartDialogue() {
			if (tree.rootNode == null) {
				Debug.LogWarning("Dialogue tree has no root node. Super Amongus.");
				return;
			}

			DialogueActive = true;
			StepDialogue(null, tree.rootNode);
		}

		public void StepDialogue(BehaviorNode currentNode, BehaviorNode newNode) {
			if (currentNode != null) { currentNode.OnCompleted -= StepDialogue; }

			if (newNode == null) {
				DialogueActive = false;
				return;
			}

			newNode.OnCompleted += StepDialogue;
			newNode.Run(_npcDataHelper); //TODO: make NOT amogus
		}
	}
}