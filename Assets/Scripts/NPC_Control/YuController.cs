using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuController : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private float radius = 5;
    [SerializeField] private float speed = 0.1f;

    private bool canFloat = true;

    private void Update()
    {
        if (Vector3.Distance(Player.position, this.transform.position) >= radius / 2)
        {
            MoveToPlayer();
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position + new Vector3(0,0,radius/2), Player.position, speed);
        }
    }

    private void MoveToPlayer()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, Player.position, speed);
    }
}
