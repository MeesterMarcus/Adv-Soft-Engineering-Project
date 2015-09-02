using UnityEngine;
using System.Collections;

public class Enemy9 : Enemy {

	public bool homing = false;
	bool fired1 = false;
	bool fired2 = false;
	bool fired3 = false;

	protected override void PostOnBecameVisible() {
		if (homing == true)
			myTr.position = new Vector3(playerTr.position.x, myTr.position.y, myTr.position.z);
	}

	protected override void PostUpdate() {
		myTr.position = new Vector2 (myTr.position.x, myTr.position.y - speed * Time.deltaTime);
		if (myTr.position.y < 0.80f + Camera.main.transform.position.y && fired1 == false) {
			Fire ();
			fired1 = true;
		}
		if (myTr.position.y < playerTr.position.y && fired2 == false) {
			Fire ();
			fired2 = true;
		}
		if (myTr.position.y < -0.80f + Camera.main.transform.position.y && fired3 == false) {
			Fire ();
			fired3 = true;
		}
	}

	protected override void PreApplyDamage() {
		if (fired3 == false && hp <= 0f) {
			Fire ();
			fired3 = true;
		}
	}

	protected void Fire() {
		Vector2 direction = (playerTr.position - myTr.position).normalized;
		Vector3 cross = Vector3.Cross (Vector2.up, direction);
		float angle = Vector2.Angle (Vector2.up, direction);

		if (cross.z > 0)
			angle = 360 - angle;

		weapons [0].FireEnemyProjectiles (myTr, angle);
	}

	protected override void PostOnTriggerEnter2D(Collider2D other) {
		if (asleep == false && other.CompareTag ("CameraEdge")) {
			if(fired1 == false) {
				Fire ();
				fired1 = true;
			} else if(fired3 == false) {
				Fire ();
				fired3 = true;
			}
		}
	}
}
