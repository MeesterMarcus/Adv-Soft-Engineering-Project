using UnityEngine;
using System.Collections;

public class Enemy1 : Enemy {
	[SerializeField] protected float targetXPos;
	[SerializeField] protected float targetYPos;
	[SerializeField] protected bool goUp;

	protected bool targetXPosReached;
	protected bool targetYPosReached;

	protected Vector2 basePosition;
	protected override void PostStart() {
		if (group != null)
			basePosition = myTr.parent.transform.position;
		else
			basePosition = Camera.main.transform.position;
	}

	protected override void PostUpdate() {
		if (targetXPosReached == false && myTr.position.x >  basePosition.x + targetXPos) { // Step 1 - Begin - reach a particular horizontal distance
			myTr.position = new Vector3 (myTr.position.x - speed * Time.deltaTime, myTr.position.y, myTr.position.z);
			return;
		}
		else if (targetXPosReached == false && myTr.position.x <= basePosition.x + targetXPos) { // Step 1 - End - horizontal distance reached
			myTr.position = new Vector3 (basePosition.x + targetXPos, myTr.position.y, myTr.position.z);
			targetXPosReached = true;
			return;
		}
		// Step 2 - Begin - reach a particular vertical distance, while executing diagonal step back movement
		else if ((goUp == false && targetXPosReached == true && targetYPosReached == false && myTr.position.y > basePosition.y + targetYPos)
		         || (goUp == true && targetXPosReached == true && targetYPosReached == false && myTr.position.y  < basePosition.y + targetYPos))
		{	
			myTr.position = new Vector3 (myTr.position.x + speed * 0.75f * Time.deltaTime, myTr.position.y, myTr.position.z);
			
			if (goUp == false)	myTr.position = new Vector3 (myTr.position.x, myTr.position.y - speed * 0.75f * Time.deltaTime, myTr.position.z);      
			else				myTr.position = new Vector3 (myTr.position.x, myTr.position.y + speed * 0.75f * Time.deltaTime, myTr.position.z);	
		}
		// Step 2 - End - vertical distance reached
		else if  ((goUp == false && targetXPosReached == true && targetYPosReached == false && myTr.position.y <= basePosition.y + targetYPos)
		          ||(goUp ==  true && targetXPosReached == true && targetYPosReached == false && myTr.position.y >= basePosition.y + targetYPos))
		{	
			myTr.position = new Vector3 (myTr.position.x, basePosition.y + targetYPos, myTr.position.z);
			targetYPosReached = true;
			return;
		}
		// Step 3 - Step back
		else if (targetXPosReached == true && targetYPosReached == true) myTr.position = new Vector3 (myTr.position.x + speed * Time.deltaTime, myTr.position.y, myTr.position.z);
	}
	protected override void PostPreemptWake() { targetXPosReached = false; targetYPosReached = false; }
}
