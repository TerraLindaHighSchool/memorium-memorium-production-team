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
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float yOffset = 10;

        private int degree = 0;

        private bool canFloat = true;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed = 0.1f;

        private Vector3 MoveToPlayer(Vector3 pos)
        {
            return Vector3.Lerp(this.transform.position, pos, speed * Time.deltaTime);
        }
        private Vector3 RotateAroundPlayer(Vector3 pos)
        {
            degree++;
            float rad = degree * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * radius;
            float y = (Mathf.Sin(rad) * sine_force);
            float z = Mathf.Sin(rad) * radius;

    private void MoveAroundPlayer()
    {
        this.transform.position = Vector3.Lerp(this.transform.position + offset, Player.position, speed);
    }
}
