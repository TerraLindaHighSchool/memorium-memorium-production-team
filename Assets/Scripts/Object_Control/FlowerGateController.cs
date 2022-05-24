using Other;
using UnityEngine;
using Game_Managing.Game_Context;
using System.Collections.Generic;

namespace Puzzle_Control.FlowerGateController
{
	public class FlowerGateController : MonoBehaviour
	{
		public void OpenGate()
		{
			if (Game_Managing.SaveManager.Instance.libraryShard &&
				Game_Managing.SaveManager.Instance.featherShard)
			{
				gameObject.SetActive(false);
			}
		}
	}
}