using UnityEngine;
using System.Collections;

public class level2MidBoss : Enemy {
	
	// Sprite references :
	[SerializeField] protected Sprite goForward;
	[SerializeField] protected Sprite goUp;
	[SerializeField] protected Sprite goDown;
	[SerializeField]protected float seekDelayMax = 0.5f;


	float rotationleft=360;
	public float rotationspeed=25;
	int damageValue = 10;

	
	protected Vector2 seekDir; // Direction to seek
	protected float seekDelay; // delay before changing direction ("seekDir")

	public float speed2 = 2f;
	public float range;
	public float minDistance = 0.5f;

	
	public float fireDelay;
	public float fireDelayMax = 1.5f;
	
	protected override void PreOnBecameVisible() { seekDir = new Vector2 (-1.0f, 0.0f); seekDelay = 0.0f; }
	protected override void PostUpdate() {

		fireDelay = fireDelay + 1 * Time.deltaTime;
		if (fireDelay > fireDelayMax) {
			Fire();
		}
		
		float rotation = rotationspeed * Time.deltaTime;
		
		transform.Rotate (0, 0, rotation);

		
		range = Vector2.Distance(transform.position, playerTr.position);
		
		if (range > minDistance)
		{
			Debug.Log(range);
			transform.position = Vector2.MoveTowards(transform.position, playerTr.position, speed2 * Time.deltaTime);
			
		}
	}
	
	protected void Fire() {
		Vector2 direction = (playerTr.position - myTr.position).normalized;
		Vector3 cross = Vector3.Cross (Vector2.up, direction);
		float angle = Vector2.Angle (Vector2.up, direction);
		
		if (cross.z > 0)
			angle = 360 - angle;
		
		weapons [0].FireEnemyProjectiles (myTr, angle);
		fireDelay = 0f;
	}



	
	void OnTriggerEnter2D(Collider2D other)
	{
		
		if (other.CompareTag("Player"))
		{  
			//Debug.Log("Enemy collided with player", gameObject);
			other.SendMessageUpwards ("ApplyDamage", damageValue, SendMessageOptions.DontRequireReceiver);
		}
		
	}








}