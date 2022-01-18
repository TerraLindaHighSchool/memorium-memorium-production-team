namespace NPC_Control.Dialogue {
	public class ResponseDialogueBox : DialogueBox {
		public string[] Choices;

		public ResponseDialogueBox(string message, string[] choices, int charDisplayDelay) : base(
			message, charDisplayDelay) {
			Choices = choices;
		}

		public override void Update() {
			if (!IsMessageFullyDisplayed && CurrentFrameCount % CharDisplayDelay == 0) {
				CurrentDisplayMessage += FullMessage[CurrentCharDisplayCount];
				CurrentCharDisplayCount++;

				if (CurrentCharDisplayCount >= FullMessage.Length) IsMessageFullyDisplayed = true;
			}

			CurrentFrameCount++;
		}
	}
}