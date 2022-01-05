using System;
using System.Collections.Generic;
using Other;

namespace Game_Managing {
	public class SceneManager : Singleton<SceneManager> {
		[Flags]
		public enum Scene : byte {
			MainMenu           = 0b_0000_0001,
			TutorialIsland     = 0b_0000_0010,
			FlowerIsland       = 0b_0000_0100,
			FeatherIsland      = 0b_0000_1000,
			LibraryIsland      = 0b_0001_0000,
			StainedGlassIsland = 0b_0010_0000,
			Graveyard          = 0b_0100_0000,

			MainGameplay = TutorialIsland
			             | FlowerIsland
			             | FeatherIsland
			             | LibraryIsland
			             | StainedGlassIsland
		}

		public static class SceneExtensions {
			public static string GetPath(Scene scene) {
				string sceneName =
					scene switch {
						Scene.MainMenu           => "",
						Scene.TutorialIsland     => "",
						Scene.FlowerIsland       => "",
						Scene.FeatherIsland      => "",
						Scene.LibraryIsland      => "",
						Scene.StainedGlassIsland => "",
						Scene.Graveyard          => "",
						_                        => throw new ArgumentException("Invalid Scene ID")
					};
				return "Gameplay_Scenes" + sceneName;
			}

			public static string[] GetPathsSafe(Scene scene) {
				List<string> initialList = new List<string>();
				if ((scene & Scene.MainMenu) == Scene.MainMenu) initialList.Add(GetPath(Scene.MainMenu));
				if ((scene & Scene.TutorialIsland) == Scene.TutorialIsland) initialList.Add(GetPath(Scene.TutorialIsland));
				if ((scene & Scene.FlowerIsland)  == Scene.FlowerIsland) initialList.Add(GetPath(Scene.FlowerIsland));
				if ((scene & Scene.FeatherIsland) == Scene.FeatherIsland) initialList.Add(GetPath(Scene.FeatherIsland));
				if ((scene & Scene.LibraryIsland) == Scene.LibraryIsland) initialList.Add(GetPath(Scene.LibraryIsland));
				if ((scene & Scene.StainedGlassIsland) == Scene.StainedGlassIsland) initialList.Add(GetPath(Scene.StainedGlassIsland));
				if ((scene & Scene.Graveyard) == Scene.Graveyard) initialList.Add(GetPath(Scene.Graveyard));
				return initialList.ToArray();
			}
		}

		public void Load(Scene scene) {
			string[] paths = scene.GetPathsSafe()
		}
	}
}