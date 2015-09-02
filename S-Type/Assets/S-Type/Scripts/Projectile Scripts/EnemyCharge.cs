using UnityEngine;
using System.Collections;

public class EnemyCharge : EnemyProjectileScript {

   public float chargeTime = 0f;

   protected override void PreStart() { chargeTime = 0f; transform.localScale = new Vector3 (0f, 0f, 0f); }
   protected override void PreOnSpawn() { chargeTime = 0f; transform.localScale = new Vector3 (0f, 0f, 0f); }

   public override void Update() {
      myTr.position = transformPtr.position;
      chargeTime += Time.deltaTime;

      myTr.localScale = new Vector3 (chargeTime, chargeTime, 1f);
      if (chargeTime >= 1f)
         myTr.localScale = new Vector3 (1f, 1f, 1f);

      if (firingWeapon.firing [id] == false)
         StartCoroutine (DestroyObject ());
   }
}
