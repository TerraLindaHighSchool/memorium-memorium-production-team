using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Puzzle_Control {
	/// <summary>
	/// An abstract representation of a puzzle, with the following components:
	/// <list type="definition">
	///		<item>
	///			<term>Guid</term>
	///			<definition>a unique string that identifies this puzzle.</definition>
	///		</item>
	///		<item>
	///			<term>Event Publishers</term>
	///			<definition>
	///				A c# event to notify puzzleSets of this puzzle's completion,
	///				and a unity event to be used in the editor to trigger various things
	///				upon puzzle completion.
	///			</definition>
	///		</item>
	///		<item>
	///			<term><c>StartPuzzle();</c></term>
	///			<definition>
	///				This is the "entrypoint" to puzzle logic, which is to be called (likely by an
	///				Interactable) when the puzzle should begin.
	///			</definition>
	///		</item>
	/// </list>
	/// </summary>
	public abstract class PuzzleController : MonoBehaviour {
		
		/// <summary>
		/// a unique string that identifies this puzzle.
		/// </summary>
		public string Guid { get; private set; }
		
		/// <summary>
		/// a unity event that is called when this puzzle is completed.
		/// </summary>
		[SerializeField] UnityEvent onPuzzleComplete;

		/// <summary>
		/// a c# event that is used to notify containing puzzle sets of this puzzle's completion (puzzle set
		/// not required for puzzle to function)
		/// </summary>
		public event Action<string> NotifyPuzzleSet; //Tell the contained puzzle set the puzzle is complete
		
		/// <summary>
		/// This is the "entrypoint" to puzzle logic, which is to be called (likely by an
		///	Interactable) when the puzzle should begin.
		/// </summary>
		public abstract void StartPuzzle();

		void OnEnable() {
			Guid = System.Guid.NewGuid().ToString();
		}

		/// <summary>
		/// call this to complete the puzzle and end puzzle context.
		/// </summary>
		protected void Complete() {
			onPuzzleComplete.Invoke();
			NotifyPuzzleSet?.Invoke(Guid);
		}

	}
}