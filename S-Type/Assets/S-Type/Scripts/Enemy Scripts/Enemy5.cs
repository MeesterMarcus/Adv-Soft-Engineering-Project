using UnityEngine;
using System.Collections;

public class Enemy5 : Enemy {
	[SerializeField] protected LayerMask groundLayerMask;
	[SerializeField] protected float actionDelayMax;
	[SerializeField] protected bool randomFire;
	[SerializeField] protected bool canFire;

	Vector3 playerTrRelative;
	float actionDelayTimer;
	float playerAngle;
	bool firing;
	bool onAir;

	protected override void PreOnBecameVisible() { myRb.isKinematic = false; actionDelayTimer = 0.0f; firing = false; onAir = false; }
	protected override void PostUpdate()
	{	
		if (canFire == false || onAir == true) return; // If our object is not allowed to fire, or isn't grounded, then abort the function
		if (gamePause == true) return; // If game is paused abort the function  (sent by "MainScript()")
		if (asleep == true) return;	// If our object is sleeping then abort the function
		
		actionDelayTimer += Time.deltaTime; // Increment action delay timer
		if (actionDelayTimer < actionDelayMax) return; // If action delay timer doesn't reach the max delay value, return the function
		if (randomFire == true && UnityEngine.Random.Range (1,5) < 4) { actionDelayTimer = 0.0f; return; } // Random fire option
		
		playerTrRelative = myTr.InverseTransformPoint(playerTr.position);
		Vector3 playerDist = playerTr.position - transform.position; // distance between object and player
		playerAngle = Vector3.Angle(Vector3.up, playerTrRelative);
		if(playerTrRelative.x < 0f) playerAngle = -playerAngle;
		
		// if player is above object and in a given range and position, then fire - note that "LaunchProjectile()" is called by animation event ('Enemy 5 fire Animation')
		if (playerTrRelative.x > 0.32f && playerTrRelative.x < 1 && playerTrRelative.y > 0.32f && playerDist.sqrMagnitude > 0.2f && playerDist.sqrMagnitude < 1.5f && hp>0)
		{
			firing = true;
			myAnim.SetBool("Firing_Bool", true); // "LaunchProjectile()" is called by 'Enemy 5 fire Animation' !
			myRb.isKinematic = true;
			actionDelayTimer = 0.0f;
		}		
	}
	protected override void PostFixedUpdate() { // Physics update
		if (firing == true) return; // if our object is firing, abort the function

		// Cast a ray under the object
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(myTr.position.x - 0.12f * myTr.right.x, myTr.position.y), -Vector2.up * myTr.up.y,  Mathf.Infinity, groundLayerMask.value);
		float groundDist = Mathf.Abs(hit.point.y - transform.position.y); // Mathf.Abs Returns the absolute value of value.
		
		if (hit /*!= null*/ && groundDist < 0.16f) { // If it hits ground...
			Debug.DrawRay (new Vector2 (myTr.position.x - 0.12f * myTr.right.x, myTr.position.y), -Vector3.up * myTr.up.y, Color.green);
			onAir = false;
		} else {
			Debug.DrawRay (new Vector2 (myTr.position.x - 0.12f * myTr.right.x, myTr.position.y), -Vector3.up * myTr.up.y, Color.red);
			onAir = true;
		}
		
		if (onAir == false) 
			myRb.velocity = new Vector2 (myTr.right.x * speed, myRb.velocity.y);
		else 
			myRb.velocity = new Vector2 (0.0f, myRb.velocity.y);
	}
	public IEnumerator LaunchProjectile() { weapons[0].FireEnemyProjectiles(myTr, playerAngle); canFire = false; yield return null; myRb.isKinematic = false; }
}
