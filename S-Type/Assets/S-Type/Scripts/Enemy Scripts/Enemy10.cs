using UnityEngine;
using System.Collections;

public class Enemy10 : Enemy {

	public float direction = 90f;

	protected override void PostOnBecameVisible() {
		weapons [0].FireEnemyProjectiles (myTr, direction);
	}

	protected override void PreApplyDamage() {
		if (hp <= 0f)
			weapons [0].StopFiring();
	}
	protected override void PreOnBecameInvisible() {
		weapons [0].StopFiring ();
	}

	protected override void PostUpdate() {
		myTr.position = new Vector2 (myTr.position.x, myTr.position.y - speed * Time.deltaTime);
		if (myTr.position.y < -0.80f) {
			weapons [0].StopFiring ();
		}
	}
}
