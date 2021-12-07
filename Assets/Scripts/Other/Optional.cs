using UnityEngine;

namespace Other {
	/// <summary>
	/// A value that may or may not exist/be relevant. Also an immutable data structure.
	/// </summary>
	/// <typeparam name="T">The type of the contained value.</typeparam>
	public struct Optional<T> {
		[SerializeField] private T _value;
		[SerializeField] private bool _enabled;

		/// <summary>
		/// the contained value.
		/// </summary>
		public T Value => _value;
		
		/// <summary>
		/// if the contained value is relevant.
		/// </summary>
		public bool Enabled => _enabled;

		public Optional(T value, bool enabled) {
			this._value   = value;
			this._enabled = enabled;
		}
	}
}