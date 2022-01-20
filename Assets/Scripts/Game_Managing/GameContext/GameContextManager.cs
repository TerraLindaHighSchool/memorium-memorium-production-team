using System.Collections.Generic;
using Other;
using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Game_Managing {
	public class GameContextManager : Singleton<GameContextManager> {
		private Stack<GameContext> contextStack;
		private GameContext        ActiveContext => contextStack.Peek() ?? null; //todo: replace null with orbitCam

		private bool isRightMouseDown = false;

		void EnterContext(GameContext newContext) {
			contextStack.Peek().onExit -= ExitContext;
			contextStack.Push(newContext);
			newContext.onExit += ExitContext;
		}

		private void ExitContext() {
			ActiveContext.onExit -= ExitContext;
			contextStack.Pop();
			ActiveContext.onExit += ExitContext;
		}

		private void Start() {
			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.Orbit.started  += OnRightClickStart;
			playerInputActions.Player.Orbit.canceled += OnRightClickStop;
		}

		private void OnMouseDelta(InputAction.CallbackContext context) => 
			ActiveContext.GCUpdate(context.ReadValue<Vector2>(), isRightMouseDown);

		private void OnRightClickStart(InputAction.CallbackContext context) =>
			isRightMouseDown = true;
		

		private void OnRightClickStop(InputAction.CallbackContext context) =>
			isRightMouseDown = false;
	}
}
