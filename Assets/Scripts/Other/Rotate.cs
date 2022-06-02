using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private int speed; ///rotation speed
	
	
	///rotates object, used for keys
    void Update()
    {
        transform.eulerAngles += new Vector3(speed, 0, 0);
    }
}
