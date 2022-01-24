using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuController : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private float radius = 2;
    [SerializeField] private int circle_steps = 10;
    [SerializeField] private int y;
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
        float circle = 2 * Mathf.PI;
        
        for (int i = 1; i == circle_steps; i ++)
        {
            float angle = i * (circle / circle_steps);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 position = new Vector3(x, y, z);
        }
    }
}
