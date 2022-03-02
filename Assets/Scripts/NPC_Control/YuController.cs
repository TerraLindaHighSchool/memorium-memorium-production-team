using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC_Control {
    [RequireComponent(typeof(NPC))]
    public class YuController : MonoBehaviour
    {
        public Transform Player;
        public NPC npc;

        [SerializeField] private float radius = 5;
        [SerializeField] private float sine_force = 1;
        [SerializeField] private float speed = 0.01f;
        [SerializeField] private float yOffset = 10;

        private int degree = 0;

        private bool canFloat = true;

        private void Update()
        {
            if (canFloat == true && npc.DialogueActive == false)
            {
                Vector3 position = new Vector3(Player.position.x, Player.position.y + yOffset, Player.position.z);
                if (Vector3.Distance(position, this.transform.position) >= radius / 2)
                {
                    this.transform.position = MoveToPlayer(position);
                }
                else
                {
                    this.transform.position = RotateAroundPlayer(position);
                }
            }
        }

        private Vector3 MoveToPlayer(Vector3 pos)
        {
            return Vector3.Lerp(this.transform.position, pos, speed);
        }
        private Vector3 RotateAroundPlayer(Vector3 pos)
        {
            degree++;
            float rad = degree * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * radius;
            float y = (Mathf.Sin(rad) * sine_force);
            float z = Mathf.Sin(rad) * radius;

            return Vector3.Lerp(this.transform.position, new Vector3(x, y, z) + pos, speed);
        }
    }
}
