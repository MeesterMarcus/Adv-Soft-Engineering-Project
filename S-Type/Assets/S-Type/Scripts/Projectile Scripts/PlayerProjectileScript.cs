using UnityEngine;
using System.Collections;

public class PlayerProjectileScript : ProjectileScript {

	[SerializeField] protected bool pierceEnemies; // Pierces through enemiess
	[SerializeField] protected bool incrementDamage; // Increments damage

	protected virtual void OnTriggerStay2D(Collider2D other) {
		PreOnTriggerStay2D (other);
		if (incrementDamage == true) {
			if (other.CompareTag("Enemy"))
				other.SendMessageUpwards ("ApplyDamage", damage * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
		}
		PostOnTriggerStay2D (other);
	}
	protected override void PostOnTriggerEnter2D(Collider2D other) {
		if (incrementDamage == false) {
			if (other.CompareTag ("Enemy")) {
				other.SendMessageUpwards ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);

				if (pierceEnemies != true) {
					myTr.GetComponent<Collider2D> ().enabled = false; 
					StartCoroutine (DestroyObject ());
				}
			}
		}
	}

	protected virtual void PreOnTriggerStay2D(Collider2D other) {}
	protected virtual void PostOnTriggerStay2D(Collider2D other) {}
}