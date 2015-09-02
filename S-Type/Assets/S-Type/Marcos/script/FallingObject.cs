using UnityEngine;
using System.Collections;

public class FallingObject : MonoBehaviour {
   
   public GameObject boulder;
   public Transform myTrans;
   public float fallSpeed = 0.5f;
   
   
   // Use this for initialization
   void Start () {
      enabled = false;
      
   }
   
   // Update is called once per frame
   void Update () {
      transform.Translate (Vector3.down * fallSpeed * Time.deltaTime);
      if (transform.position.y < -8.0f){
         Destroy(gameObject);
         //Debug.Log("destroying Boulder");
      }
      
   }
   
   
   public void OnTriggerEnter2D(Collider2D other)
   {
      
      if (other.CompareTag("Player"))
      {
         enabled = true;
      }
      
   }
   
}
