using UnityEngine;
using System.Collections;
using System.Linq;

public class LaserProjectile : PlayerProjectileScript {

   protected Transform[] laserTransforms;
   protected Transform[] branchTransforms;
 
   private LayerMask groundMask;
   private LayerMask enemyMask;
 
   public override void OnBecameInvisible() { }

   protected override void PreStart() {
      laserTransforms = transform.Cast<Transform> ().Where (c => c.gameObject.tag == "Laser").ToArray ();
      branchTransforms = transform.Cast<Transform> ().Where(c => c.gameObject.tag == "Branch").ToArray();
      groundMask = LayerMask.GetMask ("Ground Layer", "CameraEdge Layer");
      enemyMask = LayerMask.GetMask ("Enemy Layer");
      transform.position = new Vector2(-999f, -999f);
      for (int i = 0; i < laserTransforms.Length; ++i) {
         laserTransforms[i].position = new Vector2(-999f, -999f);
      }
      for (int i = 0; i < branchTransforms.Length; ++i) {
         branchTransforms[i].position = new Vector2(-999f, -999f);
      }
   }

   protected override void PreOnSpawn() {
      for (int i = 0; i < laserTransforms.Length; ++i) {
         laserTransforms[i].position = new Vector2(999f, 999f);
      }
      for (int i = 0; i < branchTransforms.Length; ++i) {
         branchTransforms[i].position = new Vector2(999f, 999f);
      }
   }

	public override void Update() {

      if (playerScript.firing == false) {
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

      Vector2 start = transformPtr.position;
      Vector2 rayDir = direction;

      for (int i = 0; i < laserTransforms.Length; ++i) {
         laserTransforms[i].position = new Vector2(-999f, -999f);
      }
      for (int i = 0; i < branchTransforms.Length; ++i) {
         branchTransforms[i].position = new Vector2(-999f, -999f);
      }

      if (branchTransforms.Length > 0)
         branchTransforms [0].position = start;

      for(int i = 0; i < laserTransforms.Length; ++i) {
         Vector3 cross = Vector3.Cross(Vector2.right, rayDir);
         float ang = Vector2.Angle(Vector2.right, rayDir);
         float distance = 1000f;

         if(cross.z < 0)
            ang = 360 - ang;

         laserTransforms[i].eulerAngles = new Vector3 (0, 0, ang);

         RaycastHit2D groundHit = Physics2D.Raycast(start, rayDir, distance, groundMask);
         if(groundHit.collider != null) { distance = groundHit.distance; }

         laserTransforms[i].position = start + new Vector2(rayDir.x * distance / 2f, rayDir.y * distance / 2f);
         laserTransforms[i].localScale = new Vector3(distance * 100f, .20f, 1);

         RaycastHit2D[] enemyHits = Physics2D.RaycastAll(start, rayDir, distance, enemyMask);
         for(int j = 0; j < enemyHits.Length; ++j) {
            RaycastHit2D enemyHit = enemyHits[j];
            if(enemyHit.collider.CompareTag("Enemy"))
               enemyHit.collider.SendMessageUpwards ("ApplyDamage", damage * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
         }

         if(groundHit.collider != null) {
            start += new Vector2(rayDir.x * distance, rayDir.y * distance);
            rayDir = Vector3.Reflect(rayDir, groundHit.normal);
         } else break;

         if(branchTransforms.Length > i+1) branchTransforms[i+1].position = start;
      }
	}

   protected static Vector2 Reflect(Vector2 inDirection, Vector2 inNormal) {
      return  2.0f * Vector2.Dot (inDirection, inNormal) * inNormal - inDirection; 
   }
}
