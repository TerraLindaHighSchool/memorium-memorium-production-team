using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Managing {
	public static class SceneManager /*: Singleton<SceneManager>*/ { //TODO: remove static modifier
		[Flags]
		public enum Scene : byte {
			None               = 0b_0000_0000, //don't actually use this!
			MainMenu           = 0b_0000_0001,
			TutorialIsland     = 0b_0000_0010,
			FlowerIsland       = 0b_0000_0100,
			FeatherIsland      = 0b_0000_1000,
			LibraryIsland      = 0b_0001_0000,
			StainedGlassIsland = 0b_0010_0000,
			Graveyard          = 0b_0100_0000,
		}

		public static string GetPath(Scene scene) {
			throw new NotImplementedException("FILL IN THE SWITCH STATEMENT STRINGS");
			string sceneName = "";
			switch (scene) {
				case Scene.MainMenu:
					sceneName = "";
					break;
				case Scene.TutorialIsland:
					sceneName = "";
					break;
				case Scene.FlowerIsland:
					sceneName = "";
					break;
				case Scene.FeatherIsland:
					sceneName = "";
					break;
				case Scene.LibraryIsland:
					sceneName = "";
					break;
				case Scene.StainedGlassIsland:
					sceneName = "";
					break;
				case Scene.Graveyard:
					sceneName = "";
					break;
			}

			return "Gameplay_Scenes" + sceneName;
		}

		public static string[] GetPaths(Scene scene) {
			List<string> initialList = new List<string>();
			foreach (Scene e in Enum.GetValues(typeof(Scene))) {
				if ((scene & e) == e) initialList.Add(GetPath(e));
			}

			return initialList.ToArray();
		}

		public static void Load(Scene scene) {
			string[] paths = GetPaths(scene);
			int      index = Random.Range(0, paths.Length - 1);
			UnityEngine.SceneManagement.SceneManager.LoadScene(paths[index]);
		}

		public static AsyncOperation LoadAsync(Scene scene) {
			string[] paths = GetPaths(scene);
			int      index = Random.Range(0, paths.Length - 1);
			return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(paths[index]);
		}
	}
}