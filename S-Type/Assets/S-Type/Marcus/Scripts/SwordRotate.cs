using UnityEngine;
using System.Collections;

public class SwordRotate : MonoBehaviour {

	float rotationRight = -360; 
	public float rotationspeed = 10; 
	int damageValue = 10; 
	
	// Update is called once per frame
	void Update () {
		float rotation = rotationspeed * Time.deltaTime; 
		transform.Rotate (0, 0, rotation); 
	
	}

	//void OnTriggerEnter2D(Collision2D other) {
	//	if (other.CompareTag ("Player")) {
	//		other.SendMessageUpwards("ApplyDamage",damageValue, SendMessageOptions.DontRequireReceiver); 
	//	}
	//}
}
