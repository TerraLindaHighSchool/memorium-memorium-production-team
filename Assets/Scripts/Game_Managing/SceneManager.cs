using System;
using System.Collections.Generic;
using Audio;
using Game_Managing.Game_Context;
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

		public struct SceneData {
			public string  Path;
			public Vector3 DefaultRespawnPoint;
		}

		public static SceneData GetSceneData(Scene scene) {
			SceneData data = new SceneData {Path = "Scenes/Gameplay_Scenes/"};
			switch (scene) {
				case Scene.MainMenu:
					data.Path += "";
					break;
				case Scene.TutorialIsland:
					data.Path                += "Tutorial Island";
					data.DefaultRespawnPoint =  new Vector3(15, 15, -12);
					break;
				case Scene.FlowerIsland:
					data.Path                += "Flower Island";
					data.DefaultRespawnPoint =  new Vector3(-415, 422, -57);
					break;
				case Scene.FeatherIsland:
					data.Path += "";
					break;
				case Scene.LibraryIsland:
					data.Path += "";
					break;
				case Scene.StainedGlassIsland:
					data.Path                += "Stained Glass Island";
					data.DefaultRespawnPoint =  new Vector3(59, 53, 52);
					break;
				case Scene.Graveyard:
					data.Path += "";
					break;
			}

			return data;
		}

		public static SceneData[] GetSceneDatas(Scene scene) {
			List<SceneData> initialList = new List<SceneData>();
			foreach (Scene e in Enum.GetValues(typeof(Scene))) {
				if ((scene & e) == e) {
					SceneData a = GetSceneData(e);
					initialList.Add(a);
				}
			}

			initialList.RemoveAt(0);

			return initialList.ToArray();
		}

		public static void Load(Scene scene) {
			SceneData[] sceneDatas    = GetSceneDatas(scene);
			int         index         = Random.Range(0, sceneDatas.Length - 1);
			SceneData   selectedScene = sceneDatas[index];
			RespawnManager.Instance.SetRespawnPoint(selectedScene.DefaultRespawnPoint);
			MusicManager.Instance.SetCurrentScene(scene);
			UnityEngine.SceneManagement.SceneManager.LoadScene(selectedScene.Path);
		}

		public static AsyncOperation LoadAsync(Scene scene) {
			SceneData[] sceneDatas    = GetSceneDatas(scene);
			int         index         = Random.Range(0, sceneDatas.Length - 1);
			SceneData   selectedScene = sceneDatas[index];
			RespawnManager.Instance.SetRespawnPoint(selectedScene.DefaultRespawnPoint);
			return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(selectedScene.Path);
		}
	}
}