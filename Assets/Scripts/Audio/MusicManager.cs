using System;
using System.Collections.Generic;
using System.Linq;
using Game_Managing;
using Other;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio {
	public class MusicManager : Singleton<MusicManager> {
		private SceneManager.Scene _currentScene;

		private AudioSource _musicSource;

		private Dictionary<Songs, AudioClip> _songs;

		private void OnEnable() {
			_currentScene = SceneManager.Scene.TutorialIsland;
			_musicSource  = Camera.main.GetComponent<AudioSource>();

			_songs = new Dictionary<Songs, AudioClip> {
				                                          {Songs.TutorialIslandMusic, Resources.Load<AudioClip>("")}, {
					                                          Songs.FlowerIslandMusic, Resources.Load<AudioClip>(
						                                          "Audio/Music/FlowerIslandMusicLoop")
				                                          },
				                                          {Songs.FeatherIslandMusic, Resources.Load<AudioClip>("")},
				                                          {Songs.LibraryIslandMusic, Resources.Load<AudioClip>("")},
				                                          {Songs.StainedGlassIslandMusic, Resources.Load<AudioClip>("")}
			                                          };
		}

		private void Update() {
			if (!_musicSource) _musicSource = Camera.main.GetComponent<AudioSource>();

			if (_musicSource.isPlaying) return;
			AudioClip lastSongPlayed = _musicSource.clip;

			AudioClip[] allAvailableSongs;

			switch (_currentScene) {
				case SceneManager.Scene.TutorialIsland:
					allAvailableSongs = new[] {_songs[Songs.TutorialIslandMusic]};
					break;
				case SceneManager.Scene.FlowerIsland:
					allAvailableSongs = new[] {_songs[Songs.FlowerIslandMusic]};
					break;
				case SceneManager.Scene.FeatherIsland:
					allAvailableSongs = new[] {_songs[Songs.FeatherIslandMusic]};
					break;
				case SceneManager.Scene.LibraryIsland:
					allAvailableSongs = new[] {_songs[Songs.LibraryIslandMusic]};
					break;
				case SceneManager.Scene.StainedGlassIsland:
					allAvailableSongs = new[] {_songs[Songs.StainedGlassIslandMusic]};
					break;
				default:
					allAvailableSongs = Array.Empty<AudioClip>();
					break;
			}

			if (allAvailableSongs.Length <= 0) return;

			if (allAvailableSongs.Length <= 1) {
				_musicSource.clip = allAvailableSongs[0];
				_musicSource.Play();
			} else {
				List<AudioClip> songsToPickFrom = allAvailableSongs.ToList();

				if (lastSongPlayed != null) {
					songsToPickFrom.Remove(lastSongPlayed);
				}

				AudioClip randomSong = songsToPickFrom[Random.Range(0, songsToPickFrom.Count)];

				_musicSource.clip = randomSong;
				_musicSource.Play();
			}
		}

		public void SetCurrentScene(SceneManager.Scene scene) {
			_currentScene = scene;

			_musicSource.Stop();
		}

		private enum Songs {
			TutorialIslandMusic,
			FlowerIslandMusic,
			FeatherIslandMusic,
			LibraryIslandMusic,
			StainedGlassIslandMusic
		}
	}

}