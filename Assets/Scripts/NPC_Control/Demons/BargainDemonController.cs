using UnityEngine;
using Game_Managing.Game_Context;

namespace NPC_Control.Demons
{
    public class BargainDemonController : MonoBehaviour
    {
        public void on_grab_shard()
        {
            var save_manager = Game_Managing.SaveManager.Instance.libraryShard = true;
        }
    }
}