using UnityEngine;
using System.Collections;

public class Enemy3 : Enemy {
	float basePosY; // the base position used for Y movement
	protected override void PreOnBecameVisible() { basePosY = myTr.position.y; } // Set "basePosY" value to object Y position
	protected override void PostUpdate() { myTr.position = new Vector3 (myTr.position.x - speed * Time.deltaTime, basePosY + 0.25f * Mathf.Sin (Time.time * speed * Mathf.PI), myTr.position.z); }
}
