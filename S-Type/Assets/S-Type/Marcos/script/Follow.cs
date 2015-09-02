using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	//public Transform target;
	public float speed = 2f;
	public float minDistance = 1f;
	public float range;

	protected PlayerScript playerScript;
	protected MainScript mainScript;
	protected Transform playerTr;

	// Use this for initialization
	void Start () {
		GameObject target = GameObject.FindWithTag ("Player");
		GameObject player = GameObject.FindWithTag ("Player");
		playerScript =  player.GetComponent<PlayerScript>();
		playerTr = player.transform;
	}
	
	
	void Update ()
	{
		range = Vector2.Distance(transform.position, playerTr.position);
		
		if (range > minDistance)
		{

			transform.position = Vector2.MoveTowards(transform.position, playerTr.position, speed * Time.deltaTime);
			
		}
	}
}