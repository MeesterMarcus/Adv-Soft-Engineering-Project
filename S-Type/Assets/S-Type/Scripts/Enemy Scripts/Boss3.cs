using UnityEngine;
using System.Collections;

public class Boss3 : Enemy {

   [System.Serializable] public class MovePhase {
      public Vector2 imageTarget;
      public Vector2 mainTarget;
      public float time;
      public int next;
      public int next2;
   }
   [SerializeField] protected MovePhase[] movePhases;

   protected Transform imageTr;
   protected Transform bodyTr;
   protected Transform camTr;

   protected Vector2 imageStart;
   protected Vector2 bodyStart;
 
   protected int   movePhaseIndex;
   protected float chargeTime;
   protected float moveTime;
   protected float fireTime;
   protected float spamTime;
   protected bool  switched;

	protected override void PreOnBecameVisible() {  
      if (transform.childCount != 1) { Debug.LogError("Need only one child"); }

      imageTr = myTr;
      bodyTr = transform.GetChild (0).transform;
      camTr = Camera.main.transform;

      imageStart = imageTr.position;
      bodyStart = bodyTr.position;
 
      movePhaseIndex = -1;
      switched = false;
      chargeTime = -1f;
      moveTime = 0f;
      spamTime = -1f;
   }
	protected override void PostUpdate() {
      if (movePhaseIndex >= 0 && movePhaseIndex < movePhases.Length) {
         MovePhase movePhase = movePhases [movePhaseIndex];
         Vector2 camPos = camTr.position;

         Vector2 imageEnd = movePhase.imageTarget;
         Vector2 bodyEnd = movePhase.mainTarget;     
         if(switched == true) {
            imageEnd = movePhase.mainTarget;
            bodyEnd = movePhase.imageTarget;
         }

         if(spamTime <= 0f) {
            imageTr.position = Vector2.Lerp(imageStart, imageEnd + camPos, moveTime / movePhase.time);
            bodyTr.position = Vector2.Lerp(bodyStart, bodyEnd + camPos, moveTime / movePhase.time);
            if((fireTime -= Time.deltaTime) < 0f) {
               fireTime = 99999f;
            }

            if(chargeTime > 0f) {
               chargeTime -= Time.deltaTime;
               if(chargeTime <= 1f) {
                  weapons[3].FireEnemyProjectiles(bodyTr, 0);
                  weapons[3].FireEnemyProjectiles(imageTr, 0);
               }
               if(chargeTime <= 0f) {
                  weapons[3].StopFiring();
                  weapons[2].FireEnemyProjectiles(bodyTr, 0);
                  weapons[2].FireEnemyProjectiles(imageTr, 0);
                  spamTime = .5f;
               }
            }
            if(hp < 250f && chargeTime <= 0f) {
               chargeTime = Random.Range(1f, 3f);
               weapons[1].StopFiring();
            }

            if ((moveTime += Time.deltaTime) >= movePhase.time) {
               if(switched == false) {
                  imageTr.position = movePhase.imageTarget + camPos;
                  bodyTr.position = movePhase.mainTarget + camPos;
               } else {
                  imageTr.position = movePhase.mainTarget + camPos;
                  bodyTr.position = movePhase.imageTarget + camPos;
               }
               fireTime = 99999f;

               int random = Random.Range(0, 2);
               if(random == 0) movePhaseIndex = movePhase.next;
               else            movePhaseIndex = movePhase.next2;
  
               imageStart = imageTr.position;
               bodyStart = bodyTr.position;
               
               movePhase = movePhases[movePhaseIndex];
               fireTime = Random.Range(0f, movePhase.time);
               moveTime = 0f;
            }

         } else {
            spamTime -= Time.deltaTime;
            if(spamTime <= 0f)
               weapons[2].StopFiring();
         }
      } else if (playerScript.camScrollEnabled == false) {
         weapons[1].FireEnemyProjectiles(bodyTr, 0);
         weapons[1].FireEnemyProjectiles(imageTr, 0);
         movePhaseIndex = 0;
         moveTime = 0f;

         fireTime = Random.Range(0f, movePhases[movePhaseIndex].time);
         imageStart = imageTr.position;
         bodyStart = bodyTr.position;
      }
	}

   public void SwitchBodies() {
      if (asleep == false) {
         Vector2 tempPosition = imageTr.position;
         Vector2 tempStart = imageStart;

         imageTr.position = bodyTr.position;
         bodyTr.position = tempPosition;

         imageStart = bodyStart;
         bodyStart = tempStart;
         switched = !switched;
      }
   }

   public override void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag ("Player")) {
         SwitchBodies();
         other.SendMessageUpwards ("ApplyDamage", 1, SendMessageOptions.DontRequireReceiver);
      }
   }

   public override IEnumerator DamageBlink() {
      mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 0.0f);
      yield return new WaitForSeconds (0.05f);
      if(hp > 0f || waitDerender == true) mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 0.5f);
   }

   protected override void PreApplyDamage() {
      if (hp <= 0f) {
         weapons [1].StopFiring();
         weapons [2].StopFiring();
         weapons [3].StopFiring();
         bodyTr.position = imageTr.position;
         mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 1.0f);
      }
   }
}
