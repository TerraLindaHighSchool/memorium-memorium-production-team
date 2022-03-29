using Other;

namespace Player_Control {
	/// <summary>
	/// Singleton class for managing the <c>PlayerInputActions</c> object.
	/// </summary>
	public class PlayerInputManager : Singleton<PlayerInputManager> {
		/// <summary>
		/// Public reference to the used <c>PlayerInputActions</c> object.
		/// </summary>
		public PlayerInputActions PlayerInputActions;

		/// <summary>
		/// Creates the <c>PlayerInputActions</c> object and enables it.
		/// </summary>
		void OnEnable() {
			PlayerInputActions = new PlayerInputActions();
			PlayerInputActions.Enable();
		}
	}
}