using UnityEngine;
using System.Collections;

public class EnemyWeapon : WeaponScript {

   public void StopFiring() {
      for (int i = 0; i < firing.Length; ++i) {
         firing[i] = false;
      }
   }
   public void StopFiring(int i) { firing [i] = false; }

	public void FireEnemyProjectiles(Transform tr, float baseDirection) { // Fire X projectiles with their own types and stats
		for(int i = 0; i < projectiles.Length; ++i) {

			Vector3  offset = projectiles[i].offset;
			float direction = projectiles[i].direction + baseDirection;
			float rotation = projectiles[i].rotation;
			Color color = projectiles[i].color;
			float speed = projectiles[i].speed;
			int power = projectiles[i].power;
			int type = projectiles[i].type;
			
			GameObject bulletClone = weaponPools[type].Spawn();
			if(bulletClone != null) {
				ProjectileScript projectileScript = bulletClone.GetComponent<ProjectileScript> ();
				if(projectileScript != null) {
               projectileScript.firingWeapon = this;
               projectileScript.transformPtr = tr;
					projectileScript.direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * direction), Mathf.Cos(Mathf.Deg2Rad * direction), 0);
					projectileScript.rotation = rotation;
					projectileScript.damage = power;
					projectileScript.color = color;
					projectileScript.speed = speed;
               projectileScript.id = i;
               firing[i] = true;
				}
				bulletClone.transform.position = 
					new Vector3( tr.position.x + offset.x,  tr.position.y + offset.y,  tr.position.z + offset.z);
			}
		}
	}
}
