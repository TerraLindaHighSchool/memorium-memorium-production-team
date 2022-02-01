using System;
using Cinemachine;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public interface IGameContext {
		void GCStart();

		void GCUpdateDelta(Vector2 mouseDelta, bool lcDown, bool rcDown);
		void GCUpdatePos(Vector2   mousePos,   bool lcDown, bool rcDown);

		float GetYRotForForwards();

		Transform GetPlayerFollowCamTarget();

		event Action onExit;
	}
}