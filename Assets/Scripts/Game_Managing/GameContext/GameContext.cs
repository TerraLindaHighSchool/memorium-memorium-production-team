using System;
using UnityEngine;
using Cinemachine;

namespace Game_Managing {
	public abstract class GameContext {
		public abstract CinemachineVirtualCamera VCam { get; }

		public abstract void GCUpdate(Vector2 mouseDelta, bool rcDown);

		public event Action onExit;
	}
}