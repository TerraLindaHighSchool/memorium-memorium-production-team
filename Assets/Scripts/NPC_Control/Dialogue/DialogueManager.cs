using NPC_Control.Behavior_Tree;
using NPC_Control.Behavior_Tree.Nodes.MapChildNodes;
using NPC_Control.Behavior_Tree.Nodes.SingleChildNodes;
using Other;
using Player_Control;
using TMPro;
using UnityEngine;
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

		public void Awake() {
			dialogueBoxPanel = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Image>();
			_buttonPrefab    = Resources.Load<GameObject>("Prefabs/Interface/UI/Button");
			_dialogueText    = dialogueBoxPanel.GetComponentInChildren<TextMeshProUGUI>();
			_playerInput     = PlayerInputManager.Instance;

			_playerInput.PlayerInputActions.Player.Interact.performed += OnAttemptNext;
			_playerInput.PlayerInputActions.Player.Jump.performed     += OnAttemptNext;

			_isDialogueShowing = false;
			dialogueBoxPanel.CrossFadeAlpha(0, 0.0f, false);
		}

		private void FixedUpdate() {
			if (!dialogueBoxPanel) Awake();

			if (_isDialogueShowing) {
				_currentDialogueBox.Update();
				_dialogueText.text = _currentDialogueBox.CurrentDisplayMessage;
			}
		}

		private void OnAttemptNext(InputAction.CallbackContext context) {
			if (_currentDialogueBox == null) return;

			if (_currentDialogueBox.IsMessageFullyDisplayed && _currentDialogueNode is DialogueNode) { EndDialogue(); }
		}

		private void OnChoiceButtonClicked(string key) {
			if (_currentDialogueBox == null) return;

			if (_currentDialogueBox.IsMessageFullyDisplayed && _currentDialogueNode is DialogueWithResponseNode) {
				GameObject.Find("Player").GetComponent<PlayerController>().OnDialogueOption();
				EndDialogue(key);
			}
		}

		private void EndDialogue(string key = "", bool isError = false) {
			dialogueBoxPanel.CrossFadeAlpha(0, 1.0f, false);
			_currentDialogueBox = null;
			_dialogueText.text  = "";

			_isDialogueShowing = false;

			if (isError) {
				_currentDialogueNode.Error("Attempted dialogue start when dialogue is already showing");
				return;
			}

			if (_currentDialogueNode is DialogueNode dialogueNode && key == "") {
				dialogueNode.OnDialogueComplete();
			} else if (_currentDialogueNode is DialogueWithResponseNode dialogueWithResponseNode && key != "") {
				ClearDialogueChoices();
				dialogueWithResponseNode.OnDialogueComplete(key);
			}
		}

		private void ClearDialogueChoices() {
			foreach (Transform child in dialogueBoxPanel.transform) {
				if (child.GetComponent<Button>()) Destroy(child.gameObject);
			}
		}

		/// <summary>
		/// Shows a dialogue box, either for standard dialogue or dialogue with choices.
		/// </summary>
		/// <param name="node">The node that launched this dialogue box.</param>
		/// <param name="message">The text to be shown in the dialogue box.</param>
		public void ShowDialogue(BehaviorNode node, string message) {
			if (_isDialogueShowing) {
				EndDialogue("", true);
				return;
			}

			if (message == null || message.Length <= 0) {
				Debug.LogError(
					$"Node {node} launched a dialogue box with no message. Please check your dialogue trees.");
				return;
			}

			dialogueBoxPanel.CrossFadeAlpha(1, 1.0f, false);
			_currentDialogueNode = node;

			switch (node) {
				case DialogueNode _:
					_currentDialogueBox = new SimpleDialogueBox(message, 3);
					break;
				case DialogueWithResponseNode dialogueWithResponseNode:
					_currentDialogueBox =
						new ResponseDialogueBox(message, dialogueWithResponseNode.GetChildrenKeys().ToArray(),
						                        3);
					CreateChoiceButtons(_currentDialogueBox as ResponseDialogueBox);
					break;
			}

			_isDialogueShowing = true;
		}

		private void CreateChoiceButtons(ResponseDialogueBox responseDialogueBox) {
			float maxWidth     = dialogueBoxPanel.rectTransform.rect.width  - 64;
			float maxHeight    = dialogueBoxPanel.rectTransform.rect.height - 64;
			float choiceBuffer = 16;
			int   numOfChoices = responseDialogueBox.Choices.Length;

			float perChoiceWidth = 32;
			float perChoiceHeight = (maxHeight + choiceBuffer) / numOfChoices
			                      - choiceBuffer;

			for (int i = 0; i < numOfChoices; i++) {
				string choice = responseDialogueBox.Choices[i];
				GameObject newButton = Instantiate(_buttonPrefab, dialogueBoxPanel.transform);
				RectTransform newButtonTransform = newButton.GetComponent<RectTransform>();
				Vector2 newButtonPos = new Vector2(maxWidth / 2 - perChoiceWidth,
				                                   (perChoiceHeight + choiceBuffer) * i - maxHeight / 2 + 64);
				newButtonTransform.anchoredPosition = newButtonPos;
				newButtonTransform.rect.Set(newButtonPos.x, newButtonPos.y, perChoiceWidth, perChoiceHeight);
				newButton.GetComponentInChildren<TextMeshProUGUI>().text = choice;

				newButton.GetComponent<Button>()
				         .onClick.AddListener(delegate { OnChoiceButtonClicked(choice); });
			}
		}
	}
}