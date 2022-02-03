namespace Other {
	/// <summary>
	/// An interface for all the objects that need to run some code to "refresh" themselves between scenes.
	/// </summary>
	public interface IInterSceneRefresher {
		void Refresh();
	}
}