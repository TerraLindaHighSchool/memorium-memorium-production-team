using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private int speed; ///rotation speed
	
	
	///rotates object, used for key
    void Update()
    {
        transform.Rotate(0, speed, 0);
    }
}
