using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{

	Animator animator;

	// Amount of time this cloud has been alive
	private float timeAlive = 0;
	private bool dying = false;

	void Start() {
		animator = GetComponent<Animator>();
		
		Birth();
	}

	void Birth() {
		animator.SetTrigger("Birth");
		timeAlive = 0;
	}

	void Death() {
		animator.SetTrigger("KillGracefully");

		// we're gonna use the timeAlive float again to determine when to destroy this object
		timeAlive = 0;
		dying = true;
	}

	void FixedUpdate()
	{
		timeAlive += Time.deltaTime;

		if (dying && timeAlive > 1.0f) {
			Destroy(gameObject);
		}
		if (timeAlive > 5) {
			Death();
		}

		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, 100.0f))
		{
			// if the raycast hits an object with method "TouchRain", call it
			/*
			if (hit.collider.gameObject.GetComponent<ReplaceMe>() != null)
			{
				hit.collider.gameObject.GetComponent<ReplaceMe>().TouchRain();
			}
			*/
		}
	}
}
