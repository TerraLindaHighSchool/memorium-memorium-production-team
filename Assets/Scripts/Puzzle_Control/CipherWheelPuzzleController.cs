using System;
using TMPro;
using UnityEngine;

namespace Puzzle_Control {
	public class CipherWheelPuzzleController : MonoBehaviour {
		[SerializeField] private String word;
		private                  String currentWord;
		
		private void OnValidate() { 
			Debug.Log("hopefully this isn't an infinite loop");
			word = word.ToLower();
		}
	}
}