using UnityEngine;
using System.Collections;

public class Horiz : Enemy {
   [SerializeField] protected LayerMask groundLayerMask;
   [SerializeField] protected float actionDelayMax;
   [SerializeField] protected bool randomFire;
   [SerializeField] protected bool canFire;
   
   Vector3 playerTrRelative;
   float actionDelayTimer;
   float playerAngle;
   bool firing = false;
   bool onAir;

   public GameObject boulder;
   public Transform myTrans;
   public float fallSpeed = 0.5f;
   PlayerScript playerScript;
   SpriteRenderer mySpriteRdr;
   Transform myTr;
   
   //public int hp = 5;
   //public int scoreValue = 100;
 
   bool asleep = false;


  
   protected override void PostUpdate()
   {  transform.Translate (Vector3.left * fallSpeed * Time.deltaTime);
      if (asleep == true) return;   // If our object is sleeping then abort the function
 
     

   }
}