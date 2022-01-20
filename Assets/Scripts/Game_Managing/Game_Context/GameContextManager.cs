using System;
using System.Collections.Generic;
using Other;
using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Game_Managing.Game_Context {
	public class GameContextManager : Singleton<GameContextManager> {
		private Stack<IGameContext> _contextStack;

		public IGameContext ActiveContext {
			get {
				try {
					return _contextStack.Peek();
				} catch {
					return OrbitCameraManager.Instance;
				}
			}
		}

		private bool _isRightMouseDown;

		public void EnterContext(IGameContext newContext) {
			ActiveContext.onExit -= ExitContext;
			_contextStack.Push(newContext);
			newContext.onExit += ExitContext;
			newContext.GCStart();
		}

		private void ExitContext() {
			ActiveContext.onExit -= ExitContext;
			_contextStack.Pop();
			ActiveContext.onExit += ExitContext;
		}

		private void Start() {
			_contextStack = new Stack<IGameContext>();

			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.MouseDelta.performed += OnMouseDelta;
			playerInputActions.Player.Orbit.started        += OnRightClickStart;
			playerInputActions.Player.Orbit.canceled       += OnRightClickStop;
		}

		private void OnMouseDelta(InputAction.CallbackContext context) =>
			ActiveContext.GCUpdate(context.ReadValue<Vector2>(), _isRightMouseDown);

		private void OnRightClickStart(InputAction.CallbackContext context) =>
			_isRightMouseDown = true;


		private void OnRightClickStop(InputAction.CallbackContext context) =>
			_isRightMouseDown = false;
	}
}