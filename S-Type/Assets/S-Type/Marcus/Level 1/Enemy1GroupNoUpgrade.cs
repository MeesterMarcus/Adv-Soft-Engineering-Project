using UnityEngine;
using System.Collections;

public class Enemy1GroupNoUpgrade : EnemyGroup {

	protected bool visibilityStart;
	
	protected override void PostStart() { 
		visibilityStart = false;
	}

	public override void ChildBecameVisible() {
		if (visibilityStart == false) {
			visibilityStart = true;
			foreach (Transform child in transform) {
				Enemy1 childScript = child.GetComponent<Enemy1> ();
				if (childScript != null) childScript.PreemptWake();
			}
		}
	}
}
