using UnityEngine;
using System.Collections;

public class EnemyX : Enemy {
	float rotationleft=360;
	public float rotationspeed=10;
	int damageValue = 10;

	void Update()
	{
		float rotation = rotationspeed * Time.deltaTime;
		
		transform.Rotate (0, 0, rotation);
	}
}
