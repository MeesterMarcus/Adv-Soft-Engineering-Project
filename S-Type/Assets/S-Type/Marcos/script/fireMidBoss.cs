using UnityEngine;
using System.Collections;

public class fireMidBoss : Enemy {
	
	float rotationleft=360;
	public float rotationspeed=10;
	int damageValue = 10;
	public float speed2 = 2f;
	public float range;
	public float minDistance = 1f;
	
	
	protected Transform playerTr;
	
	
	
	// Use this for initialization
	void Start () {
		
		GameObject player = GameObject.FindWithTag ("Player");
		
		playerTr = player.transform;
		
	}
	
	
	
	void LateUpdate()
	{
		
		float rotation = rotationspeed * Time.deltaTime;
		
		transform.Rotate (0, 0, rotation);
		
		range = Vector2.Distance(transform.position, playerTr.position);
		
		if (range > minDistance)
		{
			Debug.Log(range);
			transform.position = Vector2.MoveTowards(transform.position, playerTr.position, speed2 * Time.deltaTime);
			
		}
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