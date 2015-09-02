using UnityEngine;
using System.Collections;

public class Oscillator : MonoBehaviour {

   float timeCounter;
   public float speed;
   public float width;
   public float height;
   float z;
   float startX;
   float startY;
   int damageValue = 10;


	// Use this for initialization
	void Start () {
      //timeCounter = 10;
     // z = transform.position.z;
      startX = transform.position.x;
      startY = transform.position.y;
	
	}
	
	// Update is called once per frame
	void Update () {

      timeCounter += Time.deltaTime * speed;
      float x = Mathf.Cos (timeCounter) * width + startX;
      float y = Mathf.Sin (timeCounter) * height + startY;
      //float z = 0;
      transform.position = new Vector3 (x, y, transform.position.z);
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
