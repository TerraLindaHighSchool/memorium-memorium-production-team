using Game_Managing;
using Other;
using UnityEngine;

namespace Object_Control {
	[RequireComponent(typeof(Interactable))]
	public class ShardController : MonoBehaviour {
		// SHARD TYPE AND COLOR DIRECTORY
		// BLUE = FLOWER
		// GREEN = FEATHER
		// PINK = LIBRARY
		
		public Type type;

		public void Pickup() {
			switch (type) {
				case Type.Flower:
					SaveManager.Instance.flowerShard = true;
					break;
				case Type.Feather:
					SaveManager.Instance.featherShard = true;
					break;
				case Type.Library:
					SaveManager.Instance.libraryShard = true;
					break;
			}
		}

		public enum Type {
			Flower,
			Feather,
			Library
		}
	}
}