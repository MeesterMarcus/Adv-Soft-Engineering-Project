using UnityEngine;
using System.Collections;

public class EnemyProjectileScript : ProjectileScript {
	protected override void PostOnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player")) {
			other.SendMessageUpwards ("ApplyDamage", 1, SendMessageOptions.DontRequireReceiver);
			DestroyObject();
		}
	}
}