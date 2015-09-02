using UnityEngine;
using System;
using System.Collections;

public class ItemUpgrade:MonoBehaviour {	
	int type; // type 2 = Speed upgrade, else = score bonus

	float randomX;
	float randomY;
	float smoothLerp = 0.01f;
	Vector3 basePosition;
	Vector3 newPosition;
	public float playerAttractionRange = 0.1f;

	bool asleep = true;
	bool ready = false; 
	bool attracted = false; 
	bool earned = false;
	float distanceFromPlayer;
	
	public float boundToCamDelay;
	public float boundToCamDelayMax = 600.0f;
	public float defaultTypeChangeTime = .3f;
	public float typeChangeMinDistance = .2f;
	float typeChangeTime = 0f;

	Transform myTr;
	GameObject myGo;
	SpriteRenderer mySpriteRdr;

	GameObject player;
	Transform playerTr;
	PlayerScript playerScript;
	Camera cam;
	Transform camTr;

	public Sprite[] powerupSprites;
	public Sprite notificationWeaponUp;

	public void OnEnable() { 
		if (ready == true) {
			type = UnityEngine.Random.Range (0, powerupSprites.Length);
			mySpriteRdr.sprite = powerupSprites [type];
		}
	}
	public void OnBecameVisible() { asleep = false; }
	public void OnBecameInvisible() { if (gameObject.activeInHierarchy == true && ready == true) StartCoroutine(DestroyObject()); }
	
	public IEnumerator DestroyObject() {	
		if (GetComponent<AudioSource>().isPlaying) yield return new WaitForSeconds (GetComponent<AudioSource>().clip.length);  // Wait for the end of explosion audio clip
		yield return null; // yield function is needed because an animation is playing

		if (gameObject.activeInHierarchy == true) {	// Restore the Animator and the sorting order of the sprite, that were possibly changed to display upgrade notification (in "Update()")
			mySpriteRdr.sortingOrder = 2;
			
			myGo.SetActive(false);
			earned = attracted = false;
			asleep = true;
			boundToCamDelay = 0.0f;
			mySpriteRdr.color = Color.white;
		}	
	}
	
	public void Start() {
		myGo = gameObject;
		myTr = transform;
		mySpriteRdr = myTr.GetComponent<SpriteRenderer>();
		
		player = GameObject.FindWithTag ("Player");
		playerScript =  player.GetComponent<PlayerScript>();
		playerTr = player.transform;
		
		cam = Camera.main;
		camTr = cam.transform;
		StartCoroutine(Prepare ());
	}
	
	public IEnumerator Prepare() {
		float rand = UnityEngine.Random.Range(-1.0f, 1.0f);
		float randAdd = UnityEngine.Random.Range(-0.25f, 0.25f);
		if (rand <= 0)	randomX = -1 + randAdd;
		else randomX = 1 + randAdd;
		
		
		rand = UnityEngine.Random.Range(-1.0f, 1.0f);
		randAdd = UnityEngine.Random.Range(-0.25f, 0.25f);
		if (rand <= 0)	randomY = -1 + randAdd;
		else randomY = 1 + randAdd;
		smoothLerp = .4f;//smoothLerp = Random.Range(1.0, 2.0);
	
		yield return null;
		ready = true;
		type = UnityEngine.Random.Range (0, powerupSprites.Length);
		mySpriteRdr.sprite = powerupSprites [type];
	}

	public void Update() { // For some time, upgrade will bump when it hits the camera boundaries
		if (ready == false || asleep == true) return; // If isn't ready or is sleeping, abort the function
		if (boundToCamDelay < boundToCamDelayMax && earned == false) {	
			boundToCamDelay = boundToCamDelay + 1 * Time.deltaTime; //increment "boundToCamDelay"

			distanceFromPlayer = Vector2.Distance(new Vector2(playerTr.position.x,playerTr.position.y), new Vector2(basePosition.x, basePosition.y)); // Get the distance between upgrade and player
			if(distanceFromPlayer > typeChangeMinDistance) typeChangeTime += Time.deltaTime / 1f;
			else                                           typeChangeTime += Time.deltaTime / 2f;
			if(typeChangeTime >= defaultTypeChangeTime) {
				type = (type + 1) % powerupSprites.Length;
				mySpriteRdr.sprite = powerupSprites [type];
				typeChangeTime -= defaultTypeChangeTime;
			}
			if(type == playerScript.weaponType) {
				type = (type + 1) % powerupSprites.Length;
				mySpriteRdr.sprite = powerupSprites [type];
			}

			if (myTr.position.y >= camTr.position.y + 0.95f) {	
				myTr.position = new Vector3 (myTr.position.x, camTr.position.y + 0.95f, myTr.position.z); // Bound to cam limits
				randomY = -randomY; // Inverse Y direction
			}
			else if (myTr.position.y <= camTr.position.y - 0.95f) {	
				myTr.position = new Vector3 (myTr.position.x, camTr.position.y - 0.95f, myTr.position.z);
				randomY = -randomY;
			}
			
			if (myTr.position.x >= camTr.position.x + 1.5f) {	
				myTr.position = new Vector3 (camTr.position.x + 1.5f, myTr.position.y, myTr.position.z);
				randomX = -randomX;
			} else if (myTr.position.x <= camTr.position.x - 1.5f) {
				myTr.position = new Vector3 (camTr.position.x - 1.5f, myTr.position.y, myTr.position.z);
				randomX = -randomX;
			}
		}

		basePosition = myTr.position; // Reference actual position
		newPosition = new Vector3(myTr.position.x+randomX, myTr.position.y+randomY, myTr.position.z); // Wanted position
		distanceFromPlayer = Vector2.Distance(new Vector2(playerTr.position.x,playerTr.position.y), new Vector2(basePosition.x, basePosition.y)); // Get the distance between upgrade and player

		// If earned, sprite has been set to 'notification' sprite ('speed up !', 'weapon up !', etc)
		// make the notification go up and fade out 
		if (earned == true) {
			myTr.position = new Vector3 (myTr.position.x, myTr.position.y + 0.25f * Time.deltaTime, myTr.position.z);
			mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, mySpriteRdr.color.a -0.6f * Time.deltaTime);
		}
		
		// If close enough to the player (based on "playerAttractionRange" value), set it "attracted"
		// (by default attraction is disabled for upgrade, by setting "playerAttractionRange" to 0)	
		else if (earned == false && attracted == false && distanceFromPlayer <= playerAttractionRange && playerScript.canMove == true)
			attracted = true; // Attracted by player
		// If not earned by player yet, and is close enough, then set it "earned" (this act like a collision with the player)		
		else if (earned == false && distanceFromPlayer < 0.16f && playerScript.canMove == true) {
			earned = true;
	
			GetComponent<AudioSource>().Play();
			mySpriteRdr.sortingOrder = -2; // the notification will be drawn behind player sprite

         playerScript.SetWeaponType(type);
			mySpriteRdr.sprite = notificationWeaponUp;

			Invoke("DestroyObject", 2.0f); //Invokes "DestroyObject()" in 2 seconds, so the notification sprite has some time to display
		}	
		else if (attracted == true && playerScript.canMove == true) myTr.position = Vector3.MoveTowards(basePosition, playerTr.position, Time.deltaTime); // Else if upgrade is attracted and not close enough to be grabbed, move it towards player transform
		else myTr.position = Vector3.Lerp(basePosition, newPosition, smoothLerp * Time.deltaTime);	 // Else if it is not attracted, simply update is position	
	}
}