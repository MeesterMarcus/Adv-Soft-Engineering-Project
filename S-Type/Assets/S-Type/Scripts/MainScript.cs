
using UnityEngine;
using System;
using System.Collections;


public class MainScript:MonoBehaviour
{	
	private GameStatus gameStatus;
	private int levelSceneBase;

	[HideInInspector] public int level = 0; // The current game level
	[HideInInspector] public bool gamePause; //Is the game paused?
	public AudioClip soundPlayStart;
	public AudioClip soundSpawnIntro;
	public AudioClip soundGameOver;

	public bool lockFramerate = false; // If set to true, framerate will be locked to the target speed (frame per second)
	public int framerate = 60;	

	public GameObject particleStarsFar; // Star particle objects (slow and far)
	public GameObject particleStarsClose; // Star particle objects (long and fast and close)
	
	public GameObject uiMenu;
	public GameObject uiPause;
	public GameObject uiPause_Label;
	public GameObject uiInGame;
	public GameObject uiScore; // the score on top
	public GameObject uilives; // the lives at the beginning of levels
	public GameObject uiText; // the text UI used by Event System
	TextMesh uilivesTextMesh; // lives UI textMesh component (used to displays lives count)
	
	GameObject player; // Player object
	PlayerScript playerScript; // reference to player's script
	public UiScoreScript uiScoreScript; // reference to UI score's script
	[HideInInspector] public float checkpointPos = 0; // Last checkpoint position reached - used by "Spawn()"

	// Camera's layer masks
	public LayerMask regularLayerMask; // Regular camera's layer mask
	public LayerMask excludeEnemiesLayerMask; // This one is used when spawning to a checkpoint to hide 'Enemy Layer'
	[HideInInspector] public bool disableEnemies; // destroy enemies without apply damage to them

	public IEnumerator OnApplicationQuit()
	{
		StartCoroutine(GameDataReset ()); // This function take care of reseting all game data to start a fresh new game when level is reloaded	
		yield return null;
	}
	
	public IEnumerator Start()
	{
		player = GameObject.FindWithTag ("Player"); // Find player gameObject and his script
		playerScript =  player.GetComponent<PlayerScript>();
		yield return null;

		if (lockFramerate == true) Application.targetFrameRate = framerate;
		particleStarsFar.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Default"; // The layer used to define this sprite's overlay priority during rendering.
	    particleStarsFar.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = -3; // The overlay priority of this sprite within its layer. Lower numbers are rendered first and subsequent numbers overlay those below.
		particleStarsClose.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Default";
	    particleStarsClose.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = -2;
	    uilives.GetComponent<Renderer>().sortingLayerName = uiText.GetComponent<Renderer>().sortingLayerName = "UI"; // The layer used to define this sprite's overlay priority during rendering.
		uiPause_Label.GetComponent<Renderer>().sortingLayerName = "UI";  // The layer used to define this sprite's overlay priority during rendering.
		uilivesTextMesh = uilives.GetComponent<TextMesh>(); // Retrieve textMesh component of "uilives" (the lives UI)

		GameObject gameStatusGO = GameObject.Find ("Game Status");
		if (gameStatusGO == null) Debug.LogError ("Can't find 'Game Status' object");
		gameStatus = gameStatusGO.GetComponent<GameStatus> ();
		if (gameStatus == null) Debug.LogError ("Can't find game status script");

		levelSceneBase = Application.loadedLevel;
		checkpointPos = gameStatus.checkpointPos;
		level = gameStatus.level;

		StartCoroutine(InitGame());
	}

	// Pause the game - called by "Update()" on pause key press if "gamePause" is false
	public IEnumerator GamePauseEnable()
	{
		gamePause = true;
		
		Time.timeScale = 0.0f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		yield return null;
		uiPause.SetActive(true); // Activate pause UI elements

		GameObject[] objects = (GameObject[])FindObjectsOfType (typeof(GameObject)); // Send pause message to all objects in the scene
		foreach(GameObject go in objects)
		{
			go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
		}	
	}
	
	// Resume the game - called by "Update()" on pause key press if "gamePause" is true
	public IEnumerator GamePauseResume()
	{
		gamePause = false;
		
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		
		yield return null;

		GameObject[] objects = (GameObject[])FindObjectsOfType (typeof(GameObject)); //UnPause Objects (send unpause message to all objects in the scene)
		foreach(GameObject go in objects)
			go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
	
		uiPause.SetActive(false); // Deactivate pause UI elements
	}
	
	public IEnumerator InitGame()
	{
      playerScript.weaponType = gameStatus.weaponType;
      playerScript.speedLevel = gameStatus.speedLevel;
		yield return null;

		player.SetActive(true);
		player.GetComponent<Renderer>().enabled = false;
		GameObject.Find("Exhaust").GetComponent<Renderer>().enabled = false;

		Application.LoadLevelAdditive (levelSceneBase + level);

		yield return null;
		
		// Switch UI elements
		uiMenu.SetActive(false);
		uiInGame.SetActive(true);
		uilives.SetActive(false);
		
		player.GetComponent<Renderer>().enabled = true;
		GameObject.Find("Exhaust").GetComponent<Renderer>().enabled = true;

		playerScript.score = uiScoreScript.scoreGet = gameStatus.score;
		playerScript.lives = gameStatus.lives;
      playerScript.invincible = gameStatus.invincibility;
		checkpointPos = gameStatus.checkpointPos;

		StartCoroutine(Spawn (checkpointPos)); // Spawn player at position
		uilivesTextMesh.text = "@ x " + playerScript.lives; // Display life count. "@" character has been replaced by a ship symbol in the font texture
		uilives.SetActive(true);
		
		GetComponent<AudioSource>().clip = soundSpawnIntro;
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds (GetComponent<AudioSource>().clip.length);
		uiScoreScript.UpdateScoreEntryDirect();			// Displays directly the score (in the score UI) without visual effect
		yield return new WaitForSeconds (1.0f);
		uilives.SetActive(false);	
	}
	
	public void Update()
	{
		if      (Input.GetKeyDown ("escape")) Application.Quit(); // Exit application on key input if we are in menu
		else if (Input.GetKeyDown("p") || Input.GetKeyDown(KeyCode.Menu) || Input.GetButtonDown ("Pause")) // Switch game pause on key input
		{
			if     (gamePause == false)    StartCoroutine(GamePauseEnable());
			else if(gamePause ==  true)    StartCoroutine(GamePauseResume());
	    }
	}
	
	public IEnumerator Die() // Called by the player ('PlayerScript') when loosing a life
	{
		if (playerScript.lives >= 0) // If player lives > = 0 - We lost a life, restart at last checkpoint
		{
			if (gamePause == true) StartCoroutine(GamePauseResume ()); // Ensure that game is not paused
			
			// We store the game data in PlayerPrefs before reloading the scene
			gameStatus.weaponType = playerScript.weaponType;
			gameStatus.speedLevel = playerScript.speedLevel;
			gameStatus.checkpointPos = checkpointPos;
			gameStatus.lives = playerScript.lives;
			gameStatus.score = playerScript.score;
			gameStatus.level = level;
			
			yield return null;
			Application.LoadLevel (levelSceneBase);
		}
		else // Game Over
		{
			StartCoroutine(GameDataReset ()); // This function take care of reseting all game data to start a fresh new game when level is reloaded
			yield return null;
			StartCoroutine(GameOver());
		}		
	}

	public IEnumerator SetLevel() // Called by the player ('PlayerScript') when losing a life, or called by an event
	{
		if (level > 0) {
			if (gamePause == true)
				StartCoroutine (GamePauseResume ()); // Ensure that game is not paused

			gameStatus.weaponType = playerScript.weaponType;
			gameStatus.speedLevel = playerScript.speedLevel;
			gameStatus.checkpointPos = checkpointPos;
			gameStatus.lives = playerScript.lives;
			gameStatus.score = playerScript.score;
			gameStatus.level = level;

			yield return null;
			Application.LoadLevel (levelSceneBase);
		} else {
			level = 1;
			StartCoroutine(GameDataReset());
			yield return null;
			StatisticsManager.SetFinalScore(playerScript.score);
			Application.LoadLevel("GameOver"); // The final level in the build -- the enter initials screen.
		}
	}

	// Reset Player Prefs - This function take care of reseting all game data to start a fresh new game when level is reloaded
	public IEnumerator GameDataReset()
	{
		gameStatus.checkpointPos = 0f;
		gameStatus.score = 0;

		yield return null;
	}

	// Game Over - Displays "GAME OVER" text, plays sound and restart the application - Game data are already reseted by "Die()" function
	public IEnumerator GameOver()
	{
		if (gamePause == true) StartCoroutine(GamePauseResume ()); // Ensure that game is not paused
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().clip = soundGameOver;
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().Play();
		
		uilivesTextMesh.text = "GAME OVER";
		uilives.SetActive(true);
		yield return new WaitForSeconds (GetComponent<AudioSource>().clip.length + 1);
		
		/* Justin added the following lines. */
		StatisticsManager.SetFinalScore(playerScript.score);
		Application.LoadLevel("GameOver"); // The final level in the build -- the enter initials screen.
	}

	public IEnumerator Spawn(float camXpos) // "Spawn()" place the camera at a given "camXpos" horizontal position, and delete enemies in view (using "OnTriggerStay2D()")
	{	
		playerScript.invincible = true;
		playerScript.camScrollEnabled = false;
		GetComponent<Camera>().farClipPlane = 0.31f; // Limit the camra far clip plane to hide sprites

		if (playerScript.camScrollVertical == true)
			transform.position = new Vector3 (transform.position.x, camXpos, transform.position.z);
		else
			transform.position = new Vector3 (camXpos, transform.position.y, transform.position.z);

		disableEnemies = true; 	                                                                // Set "disableEnemies" to true in order to enable the camera Collider to trigger the enemies in view
		GetComponent<Camera>().cullingMask = excludeEnemiesLayerMask;                           // exclude enemies from the camera view
		yield return new WaitForSeconds (1.5f);
		
		disableEnemies = false;
		GetComponent<Camera>().farClipPlane = 5.0f; // Restore the camra far clip plane to show sprites
		GetComponent<Camera>().cullingMask = regularLayerMask; // Restore regular culling mask, enabling "Enemy Layer" in camera view
		playerScript.invincible = gameStatus.invincibility;
		playerScript.enabled = true;
		playerScript.camScrollEnabled = true;
		playerScript.canShoot = true;
	}

	public IEnumerator MusicPlay(AudioClip music,bool loop,bool waitForClipEnd,float delay) // Play an AudioClip - Usually called by Event System
	{	
		if (GetComponent<AudioSource>().clip == music && GetComponent<AudioSource>().isPlaying == true) 
			yield break;//If music is already playing, abort the funtion
		
		if (waitForClipEnd == true && GetComponent<AudioSource>().isPlaying == true)
			yield return new WaitForSeconds (GetComponent<AudioSource>().clip.length + delay); // Wait for the audio to have finished
		
		else if (delay != 0) yield return new WaitForSeconds (delay);
		
		GetComponent<AudioSource>().clip = music;
		GetComponent<AudioSource>().loop = loop;
		GetComponent<AudioSource>().Play();
	}
	public IEnumerator MusicStop(bool fadeOut) // Stop an AudioClip - Usually called by Event System
	{
		if (fadeOut == true)
		{
			for(int i = 0; GetComponent<AudioSource>().volume > 0; i++)
			{
				GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume - 0.125f * Time.deltaTime;
				yield return null;
			}
			
			GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().volume = 1.0f;
		}
		
		else GetComponent<AudioSource>().Stop();
	}

	public void OnTriggerStay2D(Collider2D other) { // We use this function to trigger enemies in view
		if (other.CompareTag("Enemy"))
			if (disableEnemies == true)
				other.SendMessageUpwards ("OnBecameInvisible", SendMessageOptions.DontRequireReceiver);
	}
}