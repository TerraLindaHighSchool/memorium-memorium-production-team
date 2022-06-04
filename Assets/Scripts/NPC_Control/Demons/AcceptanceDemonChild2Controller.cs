using UnityEngine;

namespace NPC_Control.Demons {
	public class AcceptanceDemonChild2Controller : MonoBehaviour {
		public void Switch() { transform.parent.GetComponent<AcceptanceDemonParentController>().Idle(); }
	}
}