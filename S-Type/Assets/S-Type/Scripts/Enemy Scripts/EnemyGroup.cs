using UnityEngine;
using System.Collections;

public class EnemyGroup : MonoBehaviour {

	private EnemyGroup parentGroup;
	[SerializeField] protected int enemies = 0;

	public void Start() {
		foreach (Transform child in transform) {
			Enemy childScript = child.GetComponent<Enemy> ();
			if(childScript != null)
				++enemies;
		}

		Transform parentTr = transform.parent;
		if (parentTr != null) {
			parentGroup = transform.parent.GetComponent<EnemyGroup> ();
			if(parentGroup != null) parentGroup.addEnemies(enemies);
		}

		PostStart (); 
	}
	public void EnemyDeath (GameObject enemy, int enemyID) {
		enemies -= 1;
		PostEnemyDeath (enemy, enemyID);

		if (parentGroup != null)
			parentGroup.EnemyDeath (enemy, enemyID);
	}
	private void addEnemies(int childEnemies) { enemies += childEnemies; }

	public virtual void EnemyEvent (GameObject enemy, int enemyID, int eventID) {}
	public virtual void ChildBecameVisible() {}
	
	protected virtual void PostEnemyDeath(GameObject enemy, int enemyID) {}
	protected virtual void PostStart() {}
}
