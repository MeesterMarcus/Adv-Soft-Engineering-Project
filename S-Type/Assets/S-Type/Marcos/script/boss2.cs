using UnityEngine;
using System.Collections;

public class boss2 : Enemy {
   
   [System.Serializable] public class DelayPhase {
      public float delayMax;
      public int hp;
   }
   public GameObject spawnerHoriz;
	public GameObject spawner;
   [SerializeField] protected DelayPhase[] delayPhases;
   [SerializeField] protected float actionDelayMax;
   [SerializeField] protected float shootDelayMax;
   [SerializeField] protected float smoothTime; // Movement smoothDamp values
   
   float actionDelayTimer;
   float shootDelayTimer;
   float targetPosY; // the target position used for Y movement
   float yVelocity;
   int posState; // targets the state of enemy's position : 1 = center, 2 = up, 3 = center, 4 = down

   /*
   protected override void PreStart(){
      Debug.Log ("got inside");
      spawnerHoriz.SetActive(true);
   }*/

   protected override void PostOnBecameVisible(){
      Debug.Log ("got inside");

		spawner.SetActive (false);
		Debug.Log ("got in side");
      spawnerHoriz.SetActive(true);
	 
      Debug.Log ("got inside2");
   }


   
   protected override void PreOnBecameVisible() { actionDelayTimer = 0.0f; shootDelayTimer = 0.0f; yVelocity = 0.0f; posState = 1; targetPosY = myTr.position.y;
     //spawnerHoriz.GetComponent<SpawnObjectHoriz>().enabled = true;
   
      //spawnerHoriz.GetComponent<Renderer> ().enabled = true;
     // Debug.Log ("just became visable");
   }
   protected override void PostUpdate() {
      
      actionDelayMax = shootDelayMax = delayPhases [0].delayMax;
      for (int i = 1; i < delayPhases.Length; ++i)
         if (hp <= delayPhases [i].hp)
            actionDelayMax = shootDelayMax = delayPhases[i].delayMax;
      
      
      float newPositionY = Mathf.SmoothDamp(transform.position.y, targetPosY, ref yVelocity, smoothTime);
      transform.position = new Vector3 (transform.position.x, newPositionY, transform.position.z);
      
      actionDelayTimer += Time.deltaTime;
      shootDelayTimer += Time.deltaTime;
      
      if (actionDelayTimer > actionDelayMax) {
         if (posState < 4) posState++;
         else posState = 1;
         actionDelayTimer = 0.0f;
         
         if (posState == 1 || posState == 3) targetPosY = 0.0f;
         else if (posState == 2) targetPosY = 0.6f;
         else if (posState == 4) targetPosY = -0.6f;
      }
      
      if (shootDelayTimer > shootDelayMax && hp>0) { //shoot
         Vector3 playerTrRelative = myTr.InverseTransformPoint(playerTr.position);
         float angle = Vector3.Angle(Vector3.up, playerTrRelative);
         if(playerTrRelative.x < 0f) angle = -angle;
         shootDelayTimer = 0.0f;
         weapons[0].FireEnemyProjectiles(myTr, -90f);
        // weapons[1].FireEnemyProjectiles(myTr, angle);
      }
   }
}
