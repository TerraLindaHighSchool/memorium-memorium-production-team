using UnityEngine;

namespace Other {
	public class DummyComponent : MonoBehaviour {
		[SerializeField] private Optional<int> test = new Optional<int>(123, true);
		[SerializeField] private int           amogus;
	}
}