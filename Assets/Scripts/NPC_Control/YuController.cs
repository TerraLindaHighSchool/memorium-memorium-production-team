using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuController : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed = 0.1f;

    private void Update()
    {
        MoveAroundPlayer();
    }

    private void MoveAroundPlayer()
    {
        this.transform.position = Vector3.Lerp(this.transform.position + offset, Player.position, speed);
    }
}
