namespace NPC_Control.Dialogue {
	public class SimpleDialogueBox : DialogueBox {
		public SimpleDialogueBox(string message, int charDisplayDelay) : base(message, charDisplayDelay) { }

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