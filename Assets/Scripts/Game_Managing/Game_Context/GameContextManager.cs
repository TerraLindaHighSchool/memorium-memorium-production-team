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
				try { return _contextStack.Peek(); } catch { return OrbitCameraManager.Instance; }
			}
		}

		private bool _isLeftClickDown;
		private bool _isRightMouseDown;

		public void EnterContext(IGameContext newContext) {
			Debug.Log($"Entering context {newContext}");
			
			ActiveContext.OnExit -= ExitContext;
			_contextStack.Push(newContext);
			newContext.OnExit += ExitContext;
			newContext.GCStart();
		}

		private void ExitContext() {
			Debug.Log($"Exiting context {_contextStack.Peek()}");

			ActiveContext.OnExit -= ExitContext;
			_contextStack.Pop();
			ActiveContext.OnExit += ExitContext;
		}

		public void Start() {
			_contextStack = new Stack<IGameContext>();

			PlayerInputActions playerInputActions = PlayerInputManager.Instance.PlayerInputActions;

			playerInputActions.Player.MouseDelta.performed += OnMouseDelta;
			playerInputActions.Player.MousePos.performed   += OnMousePos;

			playerInputActions.Player.Interact.started  += OnInteractStart;
			playerInputActions.Player.Interact.canceled += OnInteractEnd;

			playerInputActions.Player.Orbit.started  += OnRightClickStart;
			playerInputActions.Player.Orbit.canceled += OnRightClickStop;
      
			ActiveContext.GCStart();
		}

		private void Update() { ActiveContext.GCUpdateDelta(Vector2.zero, false, false); }

		private void OnMouseDelta(InputAction.CallbackContext context) =>
			ActiveContext.GCUpdateDelta(context.ReadValue<Vector2>(), _isLeftClickDown, _isRightMouseDown);
		
		private void OnMousePos(InputAction.CallbackContext context) =>
			ActiveContext.GCUpdatePos(context.ReadValue<Vector2>(), _isLeftClickDown, _isRightMouseDown);

		private void OnInteractStart(InputAction.CallbackContext context) => _isLeftClickDown = true;
		private void OnInteractEnd(InputAction.CallbackContext   context) => _isLeftClickDown = false;

		private void OnRightClickStart(InputAction.CallbackContext context) => _isRightMouseDown = true;

		private void OnRightClickStop(InputAction.CallbackContext context) => _isRightMouseDown = false;
	}
}