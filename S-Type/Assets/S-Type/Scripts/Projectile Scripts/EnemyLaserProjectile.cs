using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemyLaserProjectile : EnemyProjectileScript {

   [SerializeField] protected int segments;
   protected Transform[] laserTransforms;
   protected Transform[] branchTransforms;
 
   private LayerMask groundMask;
   private LayerMask playerMask;
   public float maxDistance;
 
   public override void OnBecameInvisible() { }

   protected override void PreStart() {

      laserTransforms = transform.Cast<Transform> ().Where (c => c.gameObject.tag == "Laser").ToArray ();
      branchTransforms = transform.Cast<Transform> ().Where(c => c.gameObject.tag == "Branch").ToArray();
      groundMask = LayerMask.GetMask ("Ground Layer", "CameraEdge Layer");
      playerMask = LayerMask.GetMask ("Player Layer");
      transform.position = new Vector2(-999f, -999f);


      for (int i = 0; i < laserTransforms.Length; ++i) {
         laserTransforms[i].position = new Vector2(999f, 999f);
      }
      for (int i = 0; i < branchTransforms.Length; ++i) {
         branchTransforms[i].position = new Vector2(999f, 999f);
      }


      if (segments > laserTransforms.Length) {
         segments = laserTransforms.Length;
      }
   }

   protected override void PreOnSpawn() {
      for (int i = 0; i < laserTransforms.Length; ++i) {
         laserTransforms[i].position = new Vector2(999f, 999f);
      }
      for (int i = 0; i < branchTransforms.Length; ++i) {
         branchTransforms[i].position = new Vector2(999f, 999f);
      }

      if (segments > laserTransforms.Length) {
         segments = laserTransforms.Length;
      }
   }

	public override void Update() {

      if (firingWeapon.firing [id] == false) {
         for (int i = 0; i < laserTransforms.Length; ++i) {
            laserTransforms [i].position = new Vector2 (999f, 999f);
         }
         for (int i = 0; i < branchTransforms.Length; ++i) {
            branchTransforms [i].position = new Vector2 (999f, 999f);
         }
         transform.position = Vector2.zero;
         StartCoroutine (DestroyObject ());
         return;
      }

		float rayAngle = Vector2.Angle (Vector2.up, direction) + firingWeapon.laserDirection;
		Vector3 cross = Vector3.Cross (Vector2.up, direction);
		if (cross.z < 0)
			rayAngle = 360f - rayAngle;
		Vector2 rayDir = new Vector2(Mathf.Sin(Mathf.Deg2Rad * rayAngle), Mathf.Cos(Mathf.Deg2Rad * rayAngle));

      Vector2 start = transformPtr.position;
      maxDistance += Time.deltaTime;
      if (maxDistance > 1000f)
         maxDistance = 1000f;


      for (int i = 0; i < laserTransforms.Length; ++i) {
         laserTransforms[i].position = new Vector2(999f, 999f);
      }
      for (int i = 0; i < branchTransforms.Length; ++i) {
         branchTransforms[i].position = new Vector2(999f, 999f);
      }


      if (branchTransforms.Length > 0)
         branchTransforms [0].position = start;

      for(int i = 0; i < segments; ++i) {
         cross = Vector3.Cross(Vector2.right, rayDir);
         float ang = Vector2.Angle(Vector2.right, rayDir);
         float distance = maxDistance;

         if(cross.z < 0)
            ang = 360 - ang;

         laserTransforms[i].eulerAngles = new Vector3 (0, 0, ang);

         RaycastHit2D groundHit = Physics2D.Raycast(start, rayDir, distance, groundMask);
         if(groundHit.collider != null) { distance = groundHit.distance * .98f; }

         laserTransforms[i].position = start + new Vector2(rayDir.x * distance / 2f, rayDir.y * distance / 2f);
         laserTransforms[i].localScale = new Vector3(distance * 100f, .20f, 1);

         RaycastHit2D[] enemyHits = Physics2D.RaycastAll(start, rayDir, distance, playerMask);
         for(int j = 0; j < enemyHits.Length; ++j) {
            RaycastHit2D enemyHit = enemyHits[j];
            if(enemyHit.collider.CompareTag("Player"))
               enemyHit.collider.SendMessageUpwards ("ApplyDamage", 1, SendMessageOptions.DontRequireReceiver);
         }

         if(groundHit.collider != null) {
            start += new Vector2(rayDir.x * distance, rayDir.y * distance);
            rayDir = Vector3.Reflect(rayDir, groundHit.normal);
         } else break;

         if(branchTransforms.Length > i+1) branchTransforms[i+1].position = start;
      }
	}
}
