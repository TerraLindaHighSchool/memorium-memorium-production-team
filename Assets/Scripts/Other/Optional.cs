using UnityEngine;

namespace Other {
	public struct Optional<T> {
		[SerializeField] private T _value;
		[SerializeField] private bool _enabled;

		public T Value => _value;
		public bool Enabled => _enabled;

		public Optional(T value, bool enabled) {
			this._value   = value;
			this._enabled = enabled;
		}
	}
}