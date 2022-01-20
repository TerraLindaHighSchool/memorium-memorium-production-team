using System;
using Cinemachine;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public interface IGameContext {
		public abstract CinemachineVirtualCamera VCam { get; }

		public abstract void GCStart();
		
		public abstract void GCUpdate(Vector2 mouseDelta, bool rcDown);

		public event Action onExit;
	}
}