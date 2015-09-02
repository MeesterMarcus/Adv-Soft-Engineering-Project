using UnityEngine;
using System.Collections;

public class BackgroundSnapScript : MonoBehaviour {

	Camera cam = null;
	
	// Update is called once per frame
	void Update () {
		if (cam == null) {
			cam = Camera.main;
			if(cam != null) {
				transform.parent = cam.transform;
			}
		}
	}
}
