using System;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public interface IGameContext {
		void GCStart();

		void GCUpdateDelta(Vector2 mouseDelta, bool lcDown, bool rcDown);
		void GCUpdatePos(Vector2   mousePos,   bool lcDown, bool rcDown);
		void GCExit();

		void GCUpdate(Vector2 mouseDelta = new Vector2(), bool rcDown = false);

		float GetYRotForForwards();

		Transform GetPlayerFollowCamTarget();

		event Action OnExit;
	}
}