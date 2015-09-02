using UnityEngine;
using System.Collections;

public class Enemy7 : Enemy {

	[SerializeField] protected float defaultFireTime = .33f;
	protected float baseDirection = 0f;
	protected float fireTime = 0f;

	protected override void PostUpdate() {
		fireTime += Time.deltaTime;

		if (fireTime > defaultFireTime) {
			fireTime -= defaultFireTime;
			weapons[0].FireEnemyProjectiles(myTr, 0 + baseDirection);
			weapons[0].FireEnemyProjectiles(myTr, 90 + baseDirection);
			weapons[0].FireEnemyProjectiles(myTr, 180 + baseDirection);
			weapons[0].FireEnemyProjectiles(myTr, 270 + baseDirection);

			if(baseDirection == 0f) baseDirection = 45f;
			else baseDirection = 0f;
		}
	}
}
