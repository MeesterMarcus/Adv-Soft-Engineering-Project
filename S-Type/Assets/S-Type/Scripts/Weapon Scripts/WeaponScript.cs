using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponScript : MonoBehaviour {
	[System.Serializable]
	public class Projectile {
		public Vector3 offset;
		public float direction;
		public float rotation;
		public float charge;
		public Color color;
		public float speed;
		public float delay;
		public bool fired;
		public int power;
		public int type;
	}

	[HideInInspector] public float laserMaxDirection = 0f;
	[HideInInspector] public float laserMinDirection = 0f;
	[HideInInspector] public float laserDirection = 0f;
	[HideInInspector] public float turnSpeed = 0f;
	
	[SerializeField] protected ObjectPoolerScript[] weaponPools;
	[SerializeField] protected Projectile[] projectiles;
	[SerializeField] public bool[] firing;

   private void Start() {
      firing = new bool[projectiles.Length];
      for (int i = 0; i < firing.Length; ++i)
         firing[i] = false;
   }

	public void SetLaserDirection(float laserMaxDirection, float laserMinDirection, float laserDirection, float turnSpeed) {
		this.laserMaxDirection = laserMaxDirection;
		this.laserMinDirection = laserMinDirection;
		this.laserDirection = laserDirection;
		this.turnSpeed = turnSpeed;
	}

	public virtual void FireProjectiles(Transform tr, float charge, float baseDirection) { // Fire X projectiles with their own types and stats
		for(int i = 0; i < projectiles.Length; ++i) {
			if(projectiles[i].fired == true) { // Fire only when available
				projectiles[i].fired = false;
				continue;
			}
			if(projectiles[i].charge > charge) // Only a certain charge is allowed
				continue;

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
					projectileScript.direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * direction), Mathf.Cos(Mathf.Deg2Rad * direction), 0);
					projectileScript.rotation = rotation;
					projectileScript.damage = power;
					projectileScript.color = color;
					projectileScript.speed = speed;
				}
				bulletClone.transform.position = 
					new Vector3( tr.position.x + offset.x,  tr.position.y + offset.y,  tr.position.z + offset.z);

				projectiles[i].fired = true;
				StartCoroutine("DelayProjectile", i);
			}
		}
	}

	public IEnumerator DelayProjectile(int i) {
		yield return new WaitForSeconds(projectiles[i].delay);
		projectiles [i].fired = false;
	}

	public void Update() {
		if (turnSpeed == 0f)
			return;
		for (int i = 0; i < projectiles.Length; ++i) {
			if(firing[i] == true) {
				laserDirection += turnSpeed * Time.deltaTime;
				if((turnSpeed >= 0f && laserDirection >= laserMaxDirection) 
				   || (turnSpeed < 0f && laserDirection <= laserMinDirection)) {
					turnSpeed = -turnSpeed;
				}
			}
		}
	}
}
