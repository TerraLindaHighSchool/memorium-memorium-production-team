﻿using System;
using System.Collections.Generic;
using Other;
using Player_Control;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputManager = Player_Control.PlayerInputManager;

namespace Game_Managing.Game_Context {
	public class GameContextManager : Singleton<GameContextManager> {
		private Stack<IGameContext> _contextStack;

		private IGameContext ActiveContext {
			get {
				try { return _contextStack.Peek(); } catch (InvalidOperationException e) {
					return OrbitCameraManager.Instance;
				}
			}
		}

		private bool _isRightMouseDown;

		void EnterContext(IGameContext newContext) {
			_contextStack.Peek().onExit -= ExitContext;
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

			playerInputActions.Player.Orbit.started  += OnRightClickStart;
			playerInputActions.Player.Orbit.canceled += OnRightClickStop;
		}

		private void OnMouseDelta(InputAction.CallbackContext context) =>
			ActiveContext.GCUpdate(context.ReadValue<Vector2>(), _isRightMouseDown);

		private void OnRightClickStart(InputAction.CallbackContext context) =>
			_isRightMouseDown = true;


		private void OnRightClickStop(InputAction.CallbackContext context) =>
			_isRightMouseDown = false;
	}
}