using Other;
using UnityEngine.Events;

namespace Game_Managing {
	public class SaveManager : Singleton<SaveManager> {
		// Puzzle completion booleans
		public bool flowerGate;
		public bool cipherPuzzle1; //TODO: add cipher puzzles :)
		public bool swordAssembly;
		public bool apodochiDefeat;
		
		// Shard obtained booleans
		public bool featherShard;
		public bool libraryShard;
		public bool flowerShard;
		
	}
}