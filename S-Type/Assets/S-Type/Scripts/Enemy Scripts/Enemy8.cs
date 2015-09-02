using UnityEngine;
using System.Collections;

public class Enemy8 : Enemy {

	public float laserMaxDirection = 225f;
	public float laserMinDirection = 135f;
	public float laserDirection = 180f;
	public float turnSpeed = 10f;
	public float delay = 2f;
	public float time = 20f;

	protected override void PostOnBecameVisible() {
		weapons [0].SetLaserDirection (laserMaxDirection, laserMinDirection, laserDirection, turnSpeed);
		weapons [0].FireEnemyProjectiles (myTr, 0f);
	}

	protected override void PostUpdate() {
		if (delay > 0f) {
			delay -= Time.deltaTime;
			if(delay <= 0f) myTr.parent = Camera.main.transform;
			return;
		}
		if (time > 0f) {
			time -= Time.deltaTime;
			if (time <= 0f) {
				myTr.parent = null;
				weapons[0].StopFiring();
			}
		} else
			return;

		// Movement
	}

	protected override void PreApplyDamage() {
		if (hp <= 0f)
			weapons [0].StopFiring();
	}
}
