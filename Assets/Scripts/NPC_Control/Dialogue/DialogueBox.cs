namespace NPC_Control.Dialogue {
	public abstract class DialogueBox {
		public string CurrentDisplayMessage;
		
		public bool IsMessageFullyDisplayed;
		
		protected string FullMessage;

		protected int CharDisplayDelay;
		protected int CurrentFrameCount;
		protected int CurrentCharDisplayCount;
		
		protected DialogueBox(string message, int charDisplayDelay) {
			FullMessage           = message;
			CharDisplayDelay = charDisplayDelay;
			CurrentDisplayMessage = "";

			CurrentFrameCount       = 0;
			IsMessageFullyDisplayed = false;
		}

		public abstract void Update();
	}
}