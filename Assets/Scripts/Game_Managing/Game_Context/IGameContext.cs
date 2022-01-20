using System;
using Cinemachine;
using UnityEngine;

namespace Game_Managing.Game_Context {
	public interface IGameContext {
		void GCStart();

		void GCUpdate(Vector2 mouseDelta, bool rcDown);

		float GetYRotForForwards();

		Transform GetPlayerFollowCamTarget();

		event Action onExit;
	}
}