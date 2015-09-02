using UnityEngine;
using System.Collections;

public class DeactivateByTime : MonoBehaviour {
   public float lifetime = 2;
  private float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
      if (Time.time > timer + lifetime) {
         timer = Time.time;
         Destroy(gameObject);
         transform.position = Vector3.zero;
      }
	
	}
}
