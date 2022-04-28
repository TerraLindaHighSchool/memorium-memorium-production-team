using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
	void FixedUpdate()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, 100.0f))
		{
			// if the raycast hits an object with method "TouchRain", call it
			if (hit.collider.gameObject.GetComponent<ReplaceMe>() != null)
			{
				hit.collider.gameObject.GetComponent<ReplaceMe>().TouchRain();
			}
		}
	}
}
