using UnityEngine;
using System;
using System.Collections.Generic;

public class ImageScript : MonoBehaviour {

	[HideInInspector] public Transform imgTr;
	public SpriteRenderer mySpriteRdr;

	// Use this for initialization
	void Start () {
		mySpriteRdr = GetComponent<SpriteRenderer>();
		imgTr = transform;
		mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 0.5f);
	}

	public void wakeUpImage() {
		mySpriteRdr.enabled = true;
	}
	public void disableImage() {
		mySpriteRdr.enabled = false;
	}

	public void flipImage(Transform other) {
		Vector3 temp = other.position;
		other.position = new Vector3 
			(imgTr.position.x,
			 imgTr.position.y,
			 imgTr.position.z);
		imgTr.position = new Vector3 (temp.x, temp.y, temp.z);
	}

	public void moveImage(Vector3 playerPosition, float distance) {
		Vector3 localPosition = playerPosition - imgTr.position;
		Vector3 direction = localPosition.normalized;
		float magnitude = localPosition.magnitude;
		if (magnitude <= distance)
			imgTr.position = playerPosition;
		else
			imgTr.localPosition = imgTr.localPosition + (direction * distance);
	}
}
