using UnityEngine;
using System.Collections;

public class SpeedupProjectile : PlayerProjectileScript {
   protected override void PostUpdate() {
      speed += (Time.deltaTime * 4f);
   }
}
