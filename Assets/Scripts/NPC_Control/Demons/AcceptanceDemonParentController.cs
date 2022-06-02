using Game_Managing;
using Other;
using UnityEngine;

namespace NPC_Control.Demons {
	public class AcceptanceDemonParentController : MonoBehaviour {
		public GameObject preTransform;
		public GameObject postTransform;
		public GameObject idle;

		private void Start() {
			preTransform.SetActive(true);
			postTransform.SetActive(false);
			idle.SetActive(false);
		}

		public void StartSwitchAnim() {
			preTransform.GetComponent<Animator>().SetTrigger("StartTransform");
			preTransform.GetComponent<Interactable>().isEnabled = false;
		}

		public void Switch() {
			preTransform.SetActive(false);
			postTransform.SetActive(true);
			
			postTransform.GetComponent<Animator>().SetTrigger("StartTransform");
		}

		public void Idle() {
			postTransform.SetActive(false);
			idle.SetActive(true);
		}

		public void GiveShard() {
			SaveManager.Instance.apodochiDefeat = true;
		}
	}
}