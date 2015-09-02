using UnityEngine;
using System.Collections;

public class ElipticalPath : MonoBehaviour {
   
   public Vector3 center = new Vector3(0, 1, 0);
   public float radiusA = 5;
   public float radiusB = 10;
   public float speed = 1;
   
   private float angle;
   
   // Use this for initialization
   void Start () {
      
   }
   
   // Update is called once per frame
   void Update () {
      angle += speed * Time.deltaTime;
      
      // Calculates position with parametric form, explanation
      float x = radiusA * Mathf.Cos(angle); //rotate around and object
      float y = radiusB * Mathf.Sin(angle); //rotate around and object

      
      transform.position = center + new Vector3(y, 0, x);
   }
}
