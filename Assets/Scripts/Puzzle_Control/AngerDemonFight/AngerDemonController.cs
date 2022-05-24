using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerDemonController : MonoBehaviour
{
	/// <summary>
	/// Stores amount of time until demon can be hit again (in seconds)
	/// </summary>
	private double cooldownTime = 0.0f;

	/// <summary>
	/// Stores amount of time to add to cooldownTime each time we get hit (in seconds)
	/// </summary>
	public double cooldownAmount = 2.0f;

	/// <summary>
	/// Stores amount of times hit
	/// </summary>
	public int hitCount = 0;

	/// <summary>
	/// Called when raincloud is above
	/// </summary>
	public void TouchRain() {
		Debug.Log("Rain touched");
		// check if we're still in cooldown
		if (cooldownTime > 0.0f) {
			return;
		}
		// set cooldown
		cooldownTime = cooldownAmount;
		// increment hit count
		hitCount++;

		// if we've been hit enough times, destroy ourselves
		if (hitCount >= 3) {
			// todo: make this animated
			Destroy(GameObject.Find("AngerDemon"));
		}
	}

	// <summary>
	// We use update to manage cooldown time (if we're in cooldown, subtract time from cooldown)
	// </summary>
	void Update() {
		if (cooldownTime > 0.0f) {
			cooldownTime -= Time.deltaTime;
		}
	}

}
