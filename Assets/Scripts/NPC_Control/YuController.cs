using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuController : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private float radius = 2;
    [SerializeField] private float speed = 0.1f;

    private void Update()
    {
        if (Vector3.Distance(Player.position, this.transform.position) >= radius / 2)
        {
            MoveToPlayer();
        }
        else
        {
            MoveAroundPlayer();
        }
    }

    private void MoveToPlayer()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, Player.position, speed);
    }
    private void MoveAroundPlayer()
    {

    }
}
