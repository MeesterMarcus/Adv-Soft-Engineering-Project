using UnityEngine;
using System.Collections;

public class Enemy2 : Enemy {

	// Sprite references :
	[SerializeField] protected Sprite goForward;
	[SerializeField] protected Sprite goUp;
	[SerializeField] protected Sprite goDown;
	[SerializeField]protected float seekDelayMax = 0.5f;

	protected Vector2 seekDir; // Direction to seek
	protected float seekDelay; // delay before changing direction ("seekDir")

	protected override void PreOnBecameVisible() { seekDir = new Vector2 (-1.0f, 0.0f); seekDelay = 0.0f; }
	protected override void PostUpdate() {

		if (seekDir == new Vector2(-1.0f, 0.0f) || seekDir == new Vector2(0.0f, 0.0f)) { // Direction : Forward
			myTr.position = new Vector3 (myTr.position.x - speed * Time.deltaTime, myTr.position.y, myTr.position.z);
			if (mySpriteRdr.sprite != goForward) mySpriteRdr.sprite = goForward;
		}
		else if (seekDir == new Vector2(-1.0f, 1.0f)) { // Direction : Up
			myTr.position = new Vector3 (myTr.position.x - speed * 0.5f * Time.deltaTime, myTr.position.y + speed * 0.5f * Time.deltaTime, myTr.position.z);
			if (mySpriteRdr.sprite != goUp) mySpriteRdr.sprite = goUp;	
		}
		else if (seekDir == new Vector2(-1.0f, -1.0f)) { // Direction : Down
			myTr.position = new Vector3 (myTr.position.x - speed * 0.5f * Time.deltaTime, myTr.position.y - speed * 0.5f * Time.deltaTime, myTr.position.z);
			
			if (mySpriteRdr.sprite != goDown) mySpriteRdr.sprite = goDown;
		}

		if (seekDelay < seekDelayMax) {seekDelay += Time.deltaTime; return;} // If seek delay doesn't reach max delay, increment it and abort the function
		seekDelay = 0.0f; // (else) Reset the delay counter

		if (myTr.position.x < playerTr.position.x || mySpriteRdr.isVisible == false) seekDir = new Vector2(-1.0f, 0.0f); // if gameObject is behind player, change direction to forward
		else if (myTr.position.y < playerTr.position.y - 0.03f) seekDir = new Vector2(-1.0f, 1.0f); // Else if player is above gameObject, change direction to down
		else if (myTr.position.y > playerTr.position.y + 0.03f) seekDir = new Vector2(-1.0f, -1.0f); // else If player is under gameObject, change direction to up
		else seekDir = new Vector2(-1.0f, 0.0f); // else player is in front of gameObject, change direction to forward		
	}
}