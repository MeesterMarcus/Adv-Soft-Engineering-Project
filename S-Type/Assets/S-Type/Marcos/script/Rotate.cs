using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

   float rotationleft=360;
   public float rotationspeed=10;
   int damageValue = 10;


	// Use this for initialization
	void Start () {
	
	}
	

     
      void Update()
      {
         
      float rotation = rotationspeed * Time.deltaTime;

      transform.Rotate (0, 0, rotation);
   }


       void OnTriggerEnter2D(Collider2D other)
      {
         
         if (other.CompareTag("Player"))
         {  
            //Debug.Log("Enemy collided with player", gameObject);
            other.SendMessageUpwards ("ApplyDamage", damageValue, SendMessageOptions.DontRequireReceiver);
         }
         
      }
	
}
