using Game_Managing;
using UnityEngine;

public class CompleteBargain : MonoBehaviour {
    public GameObject teleporter;

    public void DoStuff() {
        teleporter.SetActive(true);
        SaveManager.Instance.libraryShard = true;
    }
}
