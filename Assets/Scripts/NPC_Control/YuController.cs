using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuController : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private float radius = 5;
    [SerializeField] private float sine_force = 1;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private float yOffset = 10;

    private int degree = 0;

    private bool canFloat = true;

    private void Update()
    {
        
        if (Vector3.Distance(Player.position, this.transform.position) >= radius / 2)
        {
            this.transform.position = MoveToPlayer();
        }
        else
        {
            this.transform.position = RotateAroundPlayer();
        }
    }

    private Vector3 MoveToPlayer()
    {
        return Vector3.Lerp(this.transform.position, Player.position, speed);
    }
    private Vector3 RotateAroundPlayer()
    {
        degree++;
        float rad = degree * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * radius;
        float y = (Mathf.Sin(rad) * sine_force);
        float z = Mathf.Sin(rad) * radius;

        return Vector3.Lerp(this.transform.position, new Vector3(x, y, z) + Player.position, speed);
    }
}
