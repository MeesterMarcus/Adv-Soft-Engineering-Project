using UnityEngine;
using System;
using System.Collections;

public class PlayerScript:MonoBehaviour {
	public Sprite playerStand; // Sprite references :
	public Sprite playerUp;
	public Sprite playerDown;
	public Sprite playerVertical;

	Transform myTr;				// Target player's transform
	SpriteRenderer mySpriteRdr;	// Target player's sprite renderer
	GameObject myExhaustGo;		// Target exhaust's gameObject
	Transform myExhaustTr;		// Target exhaust's transform
	
	public Vector2 screenLimitsMin; // Leftmost and Bottom x and y positions to move from
	public Vector2 screenLimitsMax; // Rightmost and Top x and y positions to move from
	public Vector3 imageMovement; // The amount of movement for the image

	[HideInInspector] public bool gamePause; // Is the game paused ? (sent by "MainScript()")
	[HideInInspector] public bool canMove = true; // Is player allowed to move ?
	public bool canShoot;	// Is player allowed to shoot ?
	public bool invincible = false; // Is player invincible?
	
	public PlayerWeaponScript[] weapons; // For normal weapons
	float charge = 0.0f;

   [HideInInspector] public bool firingLock = false;
   [HideInInspector] public bool firing = false;
	[HideInInspector] public int speedLevelMax;
	[HideInInspector] public int weaponTypeMax;

	public float[] speeds = null; //The speed levels 
	public int speedLevel = 0;
	public int weaponType = 0;
	float speed = 0.8f;

	Transform chargeBarTr;
	float chargeBarSize = 320;
	float leftBound = 0;
	float rightBound = 0;

	public ObjectPoolerScript explosionPool; // This targets the explosion ObjectPool		
	public AudioClip deathSound; 			 // Player death/explosion :									

	public UiScoreScript uiScoreScript; // "uiScoreScript" targets the script attached to "UI_Score"
	public int livesBase = 3; // Life Base = number of lives at game start
	public int score = 0;
	public int lives;

	public bool camScrollVertical = false;
	public bool camScrollEnabled = true; //Whether we scroll or not
	public float camScrollSpeed = 0.1f; //Scroll speed

	MainScript mainScript; // Target "MainScript" component attached to camera
	Transform camTr; // Target camera's transform
	Camera cam; //The main camera

	public Transform imgTr; //The after image's position
	ImageScript image; //It's script

	Vector2 PrevInputAxis2;
	bool bringToggled;
	bool fireToggled;
	
	public void OnPauseGame() { gamePause = true; }
	public void OnResumeGame() { gamePause = false; }
	public IEnumerator Start()
	{
      speedLevelMax = speeds.Length;
      weaponTypeMax = weapons.Length;

		if (explosionPool == null)   Debug.LogError("You must populate 'explosionPool' with the corresponding object pool located in 'ObjectPools Container'", gameObject);
		if (weapons.Length <= 0) Debug.LogError("You must populate 'weapons'", gameObject);

		myTr = transform;									// We will now use "myTr" instead of "transform"
		mySpriteRdr = myTr.GetComponent<SpriteRenderer>();	// Same as above, we will now use "mySpriteRdr"
		
		// Reference player's exhaust transform. 'GameObject.Find("PlayerExhaust")' will return the game object named 'Exhaust', which is a child of 'Player'
		myExhaustGo = GameObject.Find("PlayerExhaust"); // (cache component)
		myExhaustTr = myExhaustGo.transform; 			// (cache component)

		// Turn on image
		image = GetComponentInChildren<ImageScript> ();
		image.enabled = true;
		image.wakeUpImage ();
		imgTr = image.imgTr;

		cam = Camera.main; //Get the camera
		mainScript = cam.GetComponent<MainScript>(); // Find MainScript from the camer
		
		camTr = cam.transform; //Get the camera's transform;
		imgTr.parent = camTr;
		myTr.localPosition = new Vector3 (-1.0f, myTr.localPosition.y, myTr.localPosition.z);
	
		canMove = true; 			// Can now move
		speed = speeds [speedLevel];

		PrevInputAxis2 = Vector2.zero;
		bringToggled = true;
		fireToggled = true;
      firingLock = false;
		firing = false;

		chargeBarTr = GameObject.Find ("UI_ChargeBar").transform;
		chargeBarSize = Camera.main.aspect * 200f;
		chargeBarSize -= uiScoreScript.pixelLeftSize;
		chargeBarSize -= uiScoreScript.pixelRightSize;
		leftBound = uiScoreScript.screenLeftBound;
		rightBound = uiScoreScript.screenRightBound;

		yield return null;

		if (camScrollVertical == true) {
			image.mySpriteRdr.sprite = playerVertical;
			mySpriteRdr.sprite = playerVertical;
		}
      canShoot = true;
	}

	public void Update()
	{
		Vector2 InputAxis2 = new Vector2 (Input.GetAxisRaw ("Horizontal 2"), Input.GetAxisRaw ("Vertical 2")); // Declare our input axis
		Vector2 InputAxis = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")); // Declare our input axis
		imageMovement = Vector3.zero;
		if (gamePause == true)
			return; // If game is paused abort the function  (sent by "MainScript()")
		if (canMove == false)
			return; // If "canMove" is set to false, then abort the function

		chargeBarTr.localScale = new Vector3(chargeBarSize * (float)charge / (float)weapons[weaponType].maxCharge, chargeBarTr.localScale.y, chargeBarTr.localScale.z);
      chargeBarTr.localPosition = new Vector3(rightBound - leftBound * (float)charge / (float)weapons[weaponType].maxCharge, chargeBarTr.localPosition.y, chargeBarTr.localPosition.z);
		
		if (InputAxis.y > 0) { // Up
			if (mySpriteRdr.sprite != playerUp && camScrollVertical == false)
				mySpriteRdr.sprite = playerUp; // Update the sprite

			if (myTr.position.y < camTr.position.y + screenLimitsMax.y) { // Update position (constrained to limits)
				myTr.localPosition = new Vector3 (myTr.localPosition.x, myTr.localPosition.y + speed * Time.deltaTime, myTr.localPosition.z);
				imageMovement.y = speed;
			}
		} else if (InputAxis.y < 0) { // Down
			if (mySpriteRdr.sprite != playerDown  && camScrollVertical == false)
				mySpriteRdr.sprite = playerDown; // Update the sprite

			if (myTr.position.y > camTr.position.y + screenLimitsMin.y) { // Update position (constrained to limits)
				myTr.localPosition = new Vector3 (myTr.localPosition.x, myTr.localPosition.y - speed * Time.deltaTime, myTr.localPosition.z);
				imageMovement.y = -speed;
			}
		} else if (mySpriteRdr.sprite != playerStand  && camScrollVertical == false)
			mySpriteRdr.sprite = playerStand;

		if (InputAxis.x < 0) { //Left		
			if (myTr.position.x > camTr.position.x + screenLimitsMin.x) { // Update position (constrained to limits)
				myTr.localPosition = new Vector3 (myTr.localPosition.x - speed * Time.deltaTime, myTr.localPosition.y, myTr.localPosition.z);
				imageMovement.x = -speed;
			}
		} else if (InputAxis.x > 0) { //Right
			if (myTr.position.x < camTr.position.x + screenLimitsMax.x) { // Update position (constrained to limits)
				myTr.localPosition = new Vector3 (myTr.localPosition.x + speed * Time.deltaTime, myTr.localPosition.y, myTr.localPosition.z);
				imageMovement.x = speed;
			}
		}

		if (Input.GetButtonDown ("Fire"))
		    fireToggled = !fireToggled;

		if (Input.GetButtonDown ("Bring"))
			bringToggled = !bringToggled;
		if (bringToggled == true)
			image.moveImage (myTr.position, speed * Time.deltaTime);

		if (fireToggled == true && canShoot == true && GetComponent<Renderer> ().isVisible == true) {
			if(camScrollVertical == false) weapons [weaponType].FireBothProjectiles (myTr, imgTr, charge, 90, camScrollVertical);
			else weapons [weaponType].FireBothProjectiles (myTr, imgTr, charge, 0, camScrollVertical);
         if(firingLock == false) firing = true;
		} else
			firing = false;

      if (InputAxis == Vector2.zero) {
         charge += Time.deltaTime;
         if (charge > weapons[weaponType].maxCharge)
            charge = weapons[weaponType].maxCharge;
      } else {
         if(charge != 0f) firing = false;
         charge = 0f;
      }

		if (Input.GetButtonDown ("Switch"))
			image.flipImage (myTr);

		if (InputAxis2.x < 0 && PrevInputAxis2.x == 0)
			speedLevel -= 1;
		if (InputAxis2.x > 0 && PrevInputAxis2.x == 0)
			speedLevel += 1;

      if (canShoot == true) {
         while (speedLevel < 0)
            speedLevel += speedLevelMax;
         while (speedLevel >= speedLevelMax)
            speedLevel -= speedLevelMax;
         speed = speeds [speedLevel];
      }


		PrevInputAxis2 = InputAxis2;
      firingLock = false;
	}

   public void SetWeaponType(int type) {
      weaponType = type;
      charge = weapons [weaponType].maxCharge;

      firingLock = true;
      firing = false;
   }

	public void UpdateScore(int addScore) { // This function is called by killed enemies' scripts
		score = score + addScore; // Update player's score
		uiScoreScript.ProcessScoreEntry(score); // Send the upated score to the UI ("UI_Score")'s score script ("UiScoreScript")
	}

	public IEnumerator ApplyDamage() { // "damage" value (refers to int "hp", health) isn't used for player in the project
		if (invincible == true) yield break; // If the player is "invincible", ignore the damages by aborting the function
	
		myTr.GetComponent<Collider2D>().enabled = false;
		myTr.GetComponent<Renderer>().enabled = false;
		myExhaustTr.GetComponent<Renderer>().enabled = false;
		firing = false;

		image.disableImage ();
		image.enabled = false;

		canMove = false;
		camScrollEnabled = false;

		mainScript.GetComponent<AudioSource>().Stop();
		for(int i = 0; i < 6; i++) // Up to six explosions
		{
			GameObject explosionClone = explosionPool.Spawn();
			float randomXPos = UnityEngine.Random.Range(-0.04f, 0.04f);
			float randomYPos = UnityEngine.Random.Range(-0.04f, 0.04f);
			explosionClone.transform.position = myTr.position;
			myTr.position = new Vector3 (myTr.position.x + randomXPos, myTr.position.y + randomYPos, myTr.position.z);

			GetComponent<AudioSource>().clip = deathSound;
			GetComponent<AudioSource>().Play();
			yield return new WaitForSeconds (GetComponent<AudioSource>().clip.length);
		}

		lives = lives - 1;
		camScrollEnabled = false;
		camScrollSpeed = 0.0f;
		yield return new WaitForSeconds (2.0f);
		StartCoroutine(mainScript.Die()); // Launch "Die()" function located in the main script
	}

	public void LateUpdate() { // We use "LateUpdate()" to move the camera - it is better than to do it in an Update function, as all movements relative to the camera are already processed
		if (camScrollEnabled == true) // If enabled, scroll the camera
		if (camScrollVertical == false) {
			camTr.position = new Vector3 (camTr.position.x + camScrollSpeed * Time.deltaTime, camTr.position.y, camTr.position.z);
		} else {
			camTr.position = new Vector3 (camTr.position.x, camTr.position.y + camScrollSpeed * Time.deltaTime, camTr.position.z);
		}
	}
	
	public void OnCollisionEnter2D(Collision2D coll) {
		if (coll.transform.CompareTag("Ground"))
			StartCoroutine(ApplyDamage());
	}
	public void OnTriggerEnter2D(Collider2D coll) {
		if (coll.CompareTag("Ground"))
			StartCoroutine(ApplyDamage());
	}
}