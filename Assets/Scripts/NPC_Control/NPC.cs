using System.Collections.Generic;
using System.Linq;
using Game_Managing.Game_Context;
using NPC_Control.Behavior_Tree;
using NPC_Control.Dialogue;
using Other;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NPC_Control {
	[RequireComponent(typeof(Behavior_Tree.EntityController))]
	public class NPC : MonoBehaviour {
		public class NPCDataHelper {
			private readonly NPC _outerObj;

			public readonly Behavior_Tree.EntityController EntityController;
			public readonly DialogueManager                DialogueManager;
			public readonly CutsceneManager                CutsceneManager;

			public NPCDataHelper(NPC                            outerObj,
			                     Behavior_Tree.EntityController entityController,
			                     DialogueManager                dialogueManager,
			                     CutsceneManager                cutsceneManager) {
				this._outerObj        = outerObj;
				this.EntityController = entityController;
				this.DialogueManager  = dialogueManager;
				this.CutsceneManager  = cutsceneManager;
			}

			public void InvokeDialogueEvent(string eventKey) => _outerObj.InvokeEventReceivers(eventKey);
		}

		[SerializeField] private BehaviorTree tree;

		//public Dictionary<string, EventReceiver> EventReceivers = new Dictionary<string, EventReceiver>();
		public List<EventReceiver> eventReceivers = new List<EventReceiver>();

		private NPCDataHelper _npcDataHelper;

		private DialogueContextController _dialogueContextController;

		public bool DialogueActive { get; private set; }

		private void OnEnable() {
			_npcDataHelper = new NPCDataHelper(this, null, DialogueManager.Instance, null); //TODO: NOT THIS
			DialogueActive = false;
		}

		private void OnDisable() { DialogueActive = false; }

		private void InvokeEventReceivers(string eventKey) {
			foreach (EventReceiver eventReceiver in eventReceivers) {
				if (eventReceiver.key == eventKey) { eventReceiver.@event?.Invoke(); }
			}
		}

		public EventReceiver AddEventReceiver() {
			EventReceiver newEventReceiver = ScriptableObject.CreateInstance<EventReceiver>();

			#if UNITY_EDITOR
			Undo.RecordObject(this, "Added event receiver");
			#endif

			eventReceivers.Add(newEventReceiver);

			return newEventReceiver;
		}

		public void RemoveEventReceiver() {
			#if UNITY_EDITOR
			Undo.RecordObject(this, "Removed event receiver");
			#endif

			eventReceivers.Remove(eventReceivers.Last());
		}

		public void CheckEventReceivers() {
			#if UNITY_EDITOR
			Undo.RecordObject(this, "Sanity check removed event receivers");
			#endif

			List<EventReceiver> receiversToRemove = new List<EventReceiver>();

			foreach (EventReceiver eventReceiver in eventReceivers) {
				if (!eventReceiver) receiversToRemove.Add(eventReceiver);
			}

			foreach (EventReceiver receiverToRemove in receiversToRemove) { eventReceivers.Remove(receiverToRemove); }
		}

		public void StartDialogue() {
			if (GetComponent<Interactable>()) {
				if (!GetComponent<Interactable>().isEnabled) return;
			}

			if (tree.rootNode == null) {
				Debug.LogWarning("Dialogue tree has no root node. Super Amongus.");
				return;
			}

			if (DialogueActive) {
				Debug.LogWarning($"NPC {this} tried to start dialogue while already in some.");
				return;
			}

			DialogueActive             = true;
			_dialogueContextController = new DialogueContextController(this);
			GameContextManager.Instance.EnterContext(_dialogueContextController);
			StepDialogue(null, tree.rootNode);
		}

		private void StepDialogue(BehaviorNode currentNode, BehaviorNode newNode) {
			if (currentNode != null) { currentNode.OnCompleted -= StepDialogue; }

			if (newNode == null) {
				StopDialogue();
				return;
			}

			newNode.OnCompleted += StepDialogue;
			newNode.OnError     += StopDialogue;
			newNode.Run(_npcDataHelper);
		}

		private void StopDialogue() {
			DialogueActive = false;
			_dialogueContextController.GCExit();
			_dialogueContextController = null;
		}
	}
}