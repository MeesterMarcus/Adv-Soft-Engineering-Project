using UnityEngine;
using System.Collections;

public class Enemy4 : Enemy {
	[SerializeField] protected Sprite spriteHorizontal;
	[SerializeField] protected Sprite spriteHorizontalFire;
	[SerializeField] protected Sprite spriteDiagonal;
	[SerializeField] protected Sprite spriteDiagonalFire;
	[SerializeField] protected Sprite spriteVertical;
	[SerializeField] protected Sprite spriteVerticalFire;
	[SerializeField] protected float actionDelayMax;
	[SerializeField] protected float shootDelayMax;

	Vector3 playerTrRelative;
	float actionDelayTimer;
	float shootDelayTimer;
	float direction;

	protected override void PreOnBecameVisible() { actionDelayTimer = 0.0f; shootDelayTimer = 0.0f; }
	protected override void PostUpdate() {
		actionDelayTimer += Time.deltaTime; // Increment timers
		shootDelayTimer += Time.deltaTime;

		if (actionDelayTimer<actionDelayMax) return; // If action delay timer doesn't reach the max delay value, or if enemy is already firing, return the function

		playerTrRelative = myTr.InverseTransformPoint (playerTr.position) * myTr.localScale.x; // compare player position relative to object transform;

		Debug.Log (playerTrRelative);
		if (playerTrRelative.x > - 0.2f && playerTrRelative.x < 0.2f) {
			mySpriteRdr.sprite = spriteVertical; // Compare the player position relative to object, and set the turret sprite accordingly
			direction = 0f;
		}
		else if (playerTrRelative.x < 0.2f) {
			if (playerTrRelative.y <  0.2f) {
				mySpriteRdr.sprite = spriteHorizontal;
				direction = -90;
			}
			else {
				mySpriteRdr.sprite = spriteDiagonal;
				direction = -45f;
			}
			myTr.localScale = new Vector3 (1.0f, myTr.localScale.y, myTr.localScale.z);
		} else { // if (playerTrRelative.x > - 0.2)
			if (playerTrRelative.y <  0.2f) {
				mySpriteRdr.sprite = spriteHorizontal;
				direction = 90f;
			}
			else {
				mySpriteRdr.sprite = spriteDiagonal;
				direction = 45f;
			}
			myTr.localScale = new Vector3 (-1.0f, myTr.localScale.y, myTr.localScale.z);
		}
		
		actionDelayTimer = 0.0f; // Reset the action delay timer
		if (shootDelayTimer > shootDelayMax && hp > 0) {
			weapons [0].FireEnemyProjectiles (myTr, direction);
			shootDelayTimer = 0.0f; // Reset the shoot delay count
		}
	}
}
