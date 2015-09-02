using UnityEngine;
using System.Collections;

public class OptionScript : MonoBehaviour {

	public Vector2 horizontalPosition = new Vector2 (.0f, .1f);
	public Vector2 verticalPosition = new Vector2 (.2f, .0f);
	PlayerScript player;

	// Use this for initialization
	void Start () {
		GameObject playerGO = GameObject.FindWithTag ("Player");
		if (playerGO != null) {
			player = playerGO.GetComponent<PlayerScript>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			if(player.camScrollVertical == false) { transform.localPosition = horizontalPosition; } 
			else { transform.localPosition = verticalPosition; }

			if(player.canMove == false || player.canShoot == false)
				transform.localPosition = new Vector2(-999f, -999f);
		}
	}
}
