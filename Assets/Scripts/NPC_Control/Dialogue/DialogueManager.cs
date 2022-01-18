using System.Linq;
using NPC_Control.Behavior_Tree;
using NPC_Control.Behavior_Tree.Nodes.MapChildNodes;
using NPC_Control.Behavior_Tree.Nodes.SingleChildNodes;
using Other;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace NPC_Control.Dialogue {
	public class DialogueManager : Singleton<DialogueManager> {
		private GameObject _buttonPrefab;

		[SerializeField] private Image dialogueBoxPanel;

		private PlayerInputManager _playerInput;

		private TextMeshProUGUI _dialogueText;

		private BehaviorNode _currentDialogueNode;
		private DialogueBox  _currentDialogueBox;

		private bool _isDialogueShowing;

		private void Awake() {
			dialogueBoxPanel = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Image>();
			_buttonPrefab =
				AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Interface/UI/Button.prefab");
			_dialogueText = dialogueBoxPanel.GetComponentInChildren<TextMeshProUGUI>();
			_playerInput  = PlayerInputManager.Instance;

			_playerInput.PlayerInputActions.Player.Interact.performed += OnAttemptNext;
			_playerInput.PlayerInputActions.Player.Jump.performed     += OnAttemptNext;

			_isDialogueShowing = false;
			dialogueBoxPanel.CrossFadeAlpha(0, 0.0f, false);
		}

		private void FixedUpdate() {
			if (_isDialogueShowing) {
				_currentDialogueBox.Update();
				_dialogueText.text = _currentDialogueBox.CurrentDisplayMessage;

				if (_currentDialogueBox is ResponseDialogueBox responseDialogueBox) {
					float maxWidth     = dialogueBoxPanel.flexibleWidth  - 64;
					float maxHeight    = dialogueBoxPanel.flexibleHeight - 64;
					float choiceBuffer = 16;
					int   numOfChoices = responseDialogueBox.Choices.Length;

					float perChoiceWidth = 32;
					float perChoiceHeight = (maxHeight + choiceBuffer) / numOfChoices
					                      - choiceBuffer;

					for (int i = 0; i < numOfChoices; i++) {
						string        choice             = responseDialogueBox.Choices[i];
						GameObject    newButton          = Instantiate(_buttonPrefab, dialogueBoxPanel.transform);
						RectTransform newButtonTransform = newButton.GetComponent<RectTransform>();
						newButtonTransform.anchoredPosition =
							new Vector2(maxWidth / 2 - perChoiceWidth, (perChoiceHeight + choiceBuffer) * numOfChoices);
						newButton.GetComponentInChildren<TextMeshProUGUI>().text = choice;

						newButton.GetComponent<Button>()
						         .onClick.AddListener(delegate { OnChoiceButtonClicked(choice); });
					}
				}
			}
		}

		private void OnAttemptNext(InputAction.CallbackContext context) {
			if (_currentDialogueBox == null) return;

			if (_currentDialogueBox.IsMessageFullyDisplayed) {
				dialogueBoxPanel.CrossFadeAlpha(0, 1.0f, false);
				_currentDialogueBox = null;
				_dialogueText.text  = "";

				_isDialogueShowing = false;

				if (_currentDialogueNode is DialogueNode dialogueNode) dialogueNode.OnDialogueComplete();
			}
		}

		private void OnChoiceButtonClicked(string key) {
			if (_currentDialogueBox == null) return;

			if (_currentDialogueBox.IsMessageFullyDisplayed) {
				dialogueBoxPanel.CrossFadeAlpha(0, 1.0f, false);
				_currentDialogueBox = null;
				_dialogueText.text  = "";

				_isDialogueShowing = false;

				if (_currentDialogueNode is DialogueWithResponseNode dialogueWithResponseNode) {
					dialogueWithResponseNode.OnDialogueComplete(key);
				} else {
					Debug.LogError(
						$"A button was clicked for the dialogue spawned from {_currentDialogueNode}, but it is not a DialogueWithResponseNode.");
				}
			}
		}

		/// <summary>
		/// Shows a standard dialogue box, with just a continue arrow and the specified message.
		/// </summary>
		/// <param name="node">The node that launched this dialogue box.</param>
		/// <param name="message">The text to be shown in the dialogue box.</param>
		public void ShowDialogue(BehaviorNode node, string message) {
			if (_isDialogueShowing) {
				Debug.LogWarning($"Node {node} attempted to start dialogue while some was already going, ignoring...");
				return;
			}

			dialogueBoxPanel.CrossFadeAlpha(1, 1.0f, false);
			_currentDialogueNode = node;
			int charDisplaySpeed = message.Length >= 600 ? 100 : message.Length / 6;
			int charDisplayDelay = 100 / charDisplaySpeed;
			// _currentDialogueBox = new SimpleDialogueBox(message, charDisplayDelay);

			switch (node) {
				case DialogueNode dialogueNode:
					_currentDialogueBox = new SimpleDialogueBox(message, charDisplayDelay);
					break;
				case DialogueWithResponseNode dialogueWithResponseNode:
					_currentDialogueBox =
						new ResponseDialogueBox(message, dialogueWithResponseNode.children.Keys.ToArray(),
						                        charDisplayDelay);
					break;
			}

			_isDialogueShowing = true;
		}
	}
}