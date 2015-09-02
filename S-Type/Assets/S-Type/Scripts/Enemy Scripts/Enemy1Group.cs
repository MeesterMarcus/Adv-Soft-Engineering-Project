using UnityEngine;
using System.Collections;

public class Enemy1Group : EnemyGroup {

	protected ObjectPoolerScript upgradePool;
	protected bool visibilityStart;

	protected override void PostStart() { 
		upgradePool = GameObject.Find ("ObjectPool ItemUpgrades").GetComponent<ObjectPoolerScript> ();
		visibilityStart = false;
	}
	protected override void PostEnemyDeath(GameObject enemy, int enemyID) {
		if (enemies == 0) {
			if(upgradePool != null) {
				GameObject upgradeClone = upgradePool.Spawn();
				upgradeClone.transform.position = enemy.transform.position;
			}
		}
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
