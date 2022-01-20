using System;
using Cinemachine;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public class DialogueContextController : IGameContext {
		public CinemachineVirtualCamera VCam { get; }

		public void GCStart() { throw new NotImplementedException(); }

		public void GCUpdate(Vector2 mouseDelta, bool rcDown) { }

		public event Action onExit;
	}
}