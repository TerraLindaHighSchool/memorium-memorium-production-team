using Other;
using UnityEngine;

namespace NPC_Control.Demons {
	public class AcceptanceDemonChild1Controller : MonoBehaviour {
		public void Switch() { transform.parent.GetComponent<AcceptanceDemonParentController>().Switch(); }

		public void StartDialogue() {
			if (GetComponent<Interactable>().isEnabled) GetComponent<NPC>().StartDialogue();
		}
	}
}