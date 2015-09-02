using UnityEngine;
using System.Collections;

public class PlayerWeaponScript : WeaponScript {

   public float maxCharge;

	public void FireBothProjectiles(Transform myTr, Transform imgTr, float charge, float baseDirection, bool vertical) {
		for(int i = 0; i < projectiles.Length; ++i) {
			if(projectiles[i].fired == true) // Fire only when available
				continue;
			if(projectiles[i].charge > charge) // Only a certain charge is allowed
				continue;
			
			float   direction = projectiles[i].direction + baseDirection;
			Vector3 offset = projectiles[i].offset;
			float rotation = projectiles[i].rotation;
			Color color = projectiles[i].color;
			float speed = projectiles[i].speed;
			int power = projectiles[i].power;
			int type = projectiles[i].type;

			if(vertical == true) {
				offset = new Vector3(projectiles[i].offset.y * 2, projectiles[i].offset.x * 2, projectiles[i].offset.z);
			}

			if(weaponPools[type].Check(2) == true) {
				GameObject bulletClone = weaponPools[type].Spawn();
				if(bulletClone != null) {
					ProjectileScript projectileScript = bulletClone.GetComponent<ProjectileScript> ();
					if(projectileScript != null) {
						projectileScript.direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * direction), Mathf.Cos(Mathf.Deg2Rad * direction), 0);
                  projectileScript.transformPtr = myTr;
						projectileScript.rotation = rotation - baseDirection + 90;
						projectileScript.damage = power;
						projectileScript.color = color;
						projectileScript.speed = speed;
					}
					bulletClone.transform.position = 
						new Vector3( myTr.position.x + offset.x,  myTr.position.y + offset.y,  myTr.position.z + offset.z);
				}

				bulletClone = weaponPools[type].Spawn();
				if(bulletClone != null) {
					ProjectileScript projectileScript = bulletClone.GetComponent<ProjectileScript> ();
					if(projectileScript != null) {
						projectileScript.direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * direction), Mathf.Cos(Mathf.Deg2Rad * direction), 0);
                  projectileScript.transformPtr = imgTr;
						projectileScript.rotation = rotation - baseDirection + 90;
						projectileScript.damage = power;
						projectileScript.color = color;
						projectileScript.speed = speed;
					}
					bulletClone.transform.position = 
						new Vector3( imgTr.position.x + offset.x,  imgTr.position.y + offset.y,  imgTr.position.z + offset.z);
				}
				projectiles[i].fired = true;
				StartCoroutine("DelayProjectile", i);
			}
		}
	}
}