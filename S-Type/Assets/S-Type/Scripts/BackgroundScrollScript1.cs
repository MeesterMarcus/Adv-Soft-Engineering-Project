

using UnityEngine;
using System;

public class BackgroundScrollScript1: MonoBehaviour
{
	PlayerScript player;
	float speed = 0.3f;
	
	public void Update()
	{
		if (player == null) {
			GameObject playerGO = GameObject.FindWithTag ("Player");
			if (playerGO != null) {
				player = playerGO.GetComponent<PlayerScript> ();
			}
		}
		if (player != null && player.canShoot == true) {
			speed = player.camScrollSpeed;
		}
		if (player != null && player.camScrollEnabled == true) {
			transform.Translate (new Vector3 (0, -speed * Time.deltaTime, 0));
			if (transform.localPosition.y < -1.3f)
				transform.localPosition = new Vector3 (transform.localPosition.x, -.8f, transform.localPosition.z);
		}
	}
}