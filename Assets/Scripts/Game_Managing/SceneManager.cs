using Other;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
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
			//throw new NotImplementedException("FILL IN THE SWITCH STATEMENT STRINGS");
			string sceneName = "";
			switch (scene) {
				case Scene.MainMenu:
					sceneName = "";
					break;
				case Scene.TutorialIsland:
					sceneName = "Tutorial Island";
					break;
				case Scene.FlowerIsland:
					sceneName = "Flower Island";
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

			return "Scenes/Gameplay_Scenes/" + sceneName;
		}

		public static string[] GetPaths(Scene scene) {
			List<string> initialList = new List<string>();
			foreach (Scene e in Enum.GetValues(typeof(Scene))) {
				if ((scene & e) == e) {
					string a = GetPath(e);
					initialList.Add(a);
				}
			}

			initialList.RemoveAt(0);

			return initialList.ToArray();
		}

		public static void Load(Scene scene) {
			string[] paths = GetPaths(scene);
			int      index = Random.Range(0, paths.Length - 1);
			UnityEngine.SceneManagement.SceneManager.LoadScene(paths[index]);
			foreach (IInterSceneRefresher refresher in Object.FindObjectsOfType<MonoBehaviour>()
			                                                 .OfType<IInterSceneRefresher>()) { refresher.Refresh(); }
		}

		public static AsyncOperation LoadAsync(Scene scene) {
			string[] paths = GetPaths(scene);
			int      index = Random.Range(0, paths.Length - 1);
			return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(paths[index]);
		}
	}
}