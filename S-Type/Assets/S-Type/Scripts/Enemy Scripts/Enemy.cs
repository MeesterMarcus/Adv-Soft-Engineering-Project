using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	[SerializeField] protected bool playAudioBossTheme = false;
	[SerializeField] protected bool stopScrolling = false;
	[SerializeField] protected bool useHealthBar = false;
	[SerializeField] protected bool waitDerender = false;
	[SerializeField] protected bool waitExplode = false;
	[SerializeField] protected bool giveUpgrade = false;
	[SerializeField] protected bool gamePause;
	[SerializeField] public bool asleep;

	[SerializeField] protected float stopScrollingDelay;
	[SerializeField] protected Vector2 randomMax;
	[SerializeField] protected Vector2 randomMin;
	[SerializeField] protected int scoreValue;
	[SerializeField] protected int explosions;
	[SerializeField] protected float speed;
	[SerializeField] protected float maxHp;

	[SerializeField] protected List<string> weaponNames;
	protected ObjectPoolerScript explosionPool; // Explosion object pool
	protected ObjectPoolerScript upgradePool;
	protected EnemyWeapon[] weapons;
	
	[SerializeField] protected AudioClip victorySound; // Boss victory music
	[SerializeField] protected AudioClip deathSound; // Death explosion
	[SerializeField] protected AudioClip armorSound; // Plink sound
	[SerializeField] protected AudioClip bossMusic; // Boss music
	
	protected SpriteRenderer mySpriteRdr;
	protected Rigidbody2D myRb;
	protected EnemyGroup group;
	protected Animator myAnim;
	protected Transform myTr;
	protected float hp;
	
	protected SpriteRenderer healthRdr; // Health bar for bosses transform
	protected Transform healthTr; // Health bar for bosses transform

	[SerializeField] protected string barName;
	protected TextMesh nameMesh;

	protected PlayerScript playerScript;
	protected MainScript mainScript;
	protected Transform playerTr;

	protected float healthBarSize;
	protected float rightBound;
	protected float leftBound;

	public void OnBecameVisible() {
		PreOnBecameVisible ();
		if (hp > 0) {
			if(group != null)
				group.ChildBecameVisible();
			PreemptWake(); // Minimum needed to be awake

			myTr.GetComponent<Collider2D> ().enabled = true;
			if(useHealthBar == true) {
				UiScoreScript scoreScript = GameObject.FindWithTag ("Score").GetComponent<UiScoreScript>();
				healthBarSize = Camera.main.aspect * 200f;
				healthBarSize -= scoreScript.pixelLeftSize;
				healthBarSize -= scoreScript.pixelRightSize;
				leftBound = scoreScript.screenLeftBound;
				rightBound = scoreScript.screenRightBound;
				
				healthRdr = GameObject.FindWithTag ("BossHealth").GetComponent<SpriteRenderer> ();
				healthTr = GameObject.FindWithTag ("BossHealth").transform;
				healthTr.localPosition = new Vector3(rightBound - leftBound, healthTr.localPosition.y, healthTr.localPosition.z);
				healthTr.localScale = new Vector3(healthBarSize, healthTr.localScale.y, healthTr.localScale.z);
				healthRdr.enabled = true;
				
				nameMesh = GameObject.FindWithTag ("BossName").GetComponent<TextMesh> ();
				nameMesh.text = barName;
			}

			if (playAudioBossTheme == true) StartCoroutine (LaunchMusic ()); // Play special music theme?
			if (stopScrolling == true) StartCoroutine (StopScrolling ()); // Stop scrolling?
		}
		PostOnBecameVisible ();
	}
	public virtual void OnBecameInvisible()
	{
		PreOnBecameInvisible ();
		if (gameObject.activeInHierarchy == true) Destroy (gameObject); // Kills the game object
		PostOnBecameInvisible ();
	}

	public virtual void OnPauseGame()  { PreOnPauseGame () ; gamePause =  true; PostOnPauseGame  (); } // Called by 'MainScript'
	public virtual void OnResumeGame() { PreOnResumeGame (); gamePause = false; PostOnResumeGame (); } // Called by 'MainScript'

	public virtual IEnumerator DisableCollider() 
	{
		PreDisableCollider ();
		yield return new WaitForSeconds(0.3f); myTr.GetComponent<Collider2D>().enabled = false;
		PostDisableCollider ();
	}
	public virtual void Start () {
		PreStart ();
		explosionPool = GameObject.Find("ObjectPool EnemyExplosions").GetComponent<ObjectPoolerScript>();  
		upgradePool = GameObject.Find("ObjectPool ItemUpgrades").GetComponent<ObjectPoolerScript>();

		weapons = new EnemyWeapon[weaponNames.Count];
		for (int i = 0; i < weaponNames.Count; ++i) {
			GameObject   weapon = GameObject.Find ("EnemyWeapon Container/EnemyWeapon " + weaponNames [i]);
			EnemyWeapon script = null;

			if(weapon == null)
				Debug.LogError("Can't find EnemyWeapon Container/EnemyWeapon " + weaponNames [i]);
			else
				script = weapon.GetComponent<EnemyWeapon>();
			if(script == null)
				Debug.LogError("Can't get weapon for index " + i);

			weapons[i] = script;
		}

		mySpriteRdr = GetComponent<SpriteRenderer>();
		myAnim = GetComponent<Animator> ();
		myTr = transform;

		if (myTr.parent != null && myTr.parent.GetComponent<EnemyGroup> () != null)
			group = myTr.parent.GetComponent<EnemyGroup> ();

		myRb = GetComponent<Rigidbody2D>();
		if(myRb != null) 
			myRb.gravityScale *= myTr.up.y;

		GameObject player = GameObject.FindWithTag ("Player");
		playerScript =  player.GetComponent<PlayerScript>();
		playerTr = player.transform;

		gamePause = false;
		asleep = true;
		hp = maxHp;
		
		StartCoroutine(DisableCollider());
		PostStart ();
	}

	public virtual void Update () {
		PreUpdate ();
		if (gamePause == true) return; // If game is paused abort the function (sent by "MainScript()")
		if (asleep == true || hp <=0) return; // If our object is sleeping then abort the function
		PostUpdate ();
	}

	public virtual void FixedUpdate() {	
		PreFixedUpdate ();
		if (gamePause == true) return; // If game is paused abort the function  (sent by "MainScript()")
		if (asleep == true) return; // If our object is sleeping then abort the function
		PostFixedUpdate ();
	}

	public virtual IEnumerator StopScrolling() 
	{
		PreStopScrolling ();
		yield return new WaitForSeconds (stopScrollingDelay); 
		playerScript.camScrollEnabled = false;
		PostStopScrolling ();
	}
	public virtual IEnumerator LaunchMusic() {	
		PreLaunchMusic ();
		Camera cam = Camera.main; // Find MainScript
		mainScript = cam.GetComponent<MainScript>();
		
		mainScript.StopCoroutine("MusicStop"); // Stop all coroutines relative to the audio in "MainScript"
		mainScript.StopCoroutine("MusicPlay");
		
		mainScript.GetComponent<AudioSource>().clip = bossMusic;
		mainScript.GetComponent<AudioSource>().Stop();
		playAudioBossTheme = false;
		yield return null;
		
		mainScript.GetComponent<AudioSource>().loop = true;
		mainScript.GetComponent<AudioSource>().Play();
		PostLaunchMusic ();
	}

	public virtual IEnumerator ApplyDamage(float damage) {
		if (asleep == true) yield break; // Ensure that object receiving damage is not sleeping (and therefore out of screen)
		hp = hp-damage;
		PreApplyDamage ();
		
		if (hp > 0f) {
			StartCoroutine(DamageBlink ());
			GetComponent<AudioSource>().clip = armorSound;
			GetComponent<AudioSource>().Play();

			if(useHealthBar == true) {
				healthTr.localScale = new Vector3(healthBarSize * (float)hp / (float)maxHp, healthTr.localScale.y, healthTr.localScale.z);
				healthTr.localPosition = new Vector3(rightBound - leftBound * (float)hp / (float)maxHp, healthTr.localPosition.y, healthTr.localPosition.z);
				healthRdr.enabled = true;
			}
			PostApplyDamage ();
		}
		else 
		{	
         if(waitDerender == false) 
            mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 0.0f);

			if(group != null)
				group.EnemyDeath(gameObject, 0);

			myTr.GetComponent<Collider2D>().enabled = false;
			asleep = true;

			if(useHealthBar == true) {
				healthTr.localScale = new Vector3(healthBarSize, healthTr.localScale.y, healthTr.localScale.z);
				healthTr.localPosition = new Vector3(rightBound - leftBound, healthTr.localPosition.y, healthTr.localPosition.z);
				healthRdr.enabled = false;
				nameMesh.text = "";
				nameMesh = null;
			}
			yield return null;

			GetComponent<AudioSource>().clip = deathSound;
			GetComponent<AudioSource>().Play();

			if(victorySound != null) {
				mainScript.StopCoroutine("MusicPlay"); // Stop all coroutines relative to the audio in "MainScript"
				mainScript.StopCoroutine("MusicStop");
				yield return null;

				mainScript.GetComponent<AudioSource>().Stop();
				StartCoroutine(mainScript.MusicPlay(victorySound, false, false, 0.0f)); // music, musicLoop, musicWaitForClipEnd, musicDelay
			}

			playerScript.UpdateScore(scoreValue);
			StatisticsManager.IncrementKills(gameObject.name); // Justin added this line.

			for(int i = 0; i < explosions; i++) {
				GameObject explosionClone = explosionPool.Spawn();
				float randomXPos = UnityEngine.Random.Range(randomMin.x, randomMax.x);
				float randomYPos = UnityEngine.Random.Range(randomMin.y, randomMax.y);
				explosionClone.transform.position = new Vector3(myTr.position.x + randomXPos, myTr.position.y + randomYPos, explosionClone.transform.position.z);

				if(deathSound != null) {
					GetComponent<AudioSource>().clip = deathSound;
					GetComponent<AudioSource>().Play();
					if(waitExplode == true) yield return new WaitForSeconds (GetComponent<AudioSource>().clip.length);
				}
			}
			if(GetComponent<AudioSource>().clip != null)
				yield return new WaitForSeconds (GetComponent<AudioSource>().clip.length);

			
			if (giveUpgrade == true) {
				GameObject upgradeClone = upgradePool.Spawn();
				upgradeClone.transform.position = myTr.position;
			}

			myTr.GetComponent<Renderer>().enabled = false;
			PostApplyDamage ();

			if (stopScrolling == true) playerScript.camScrollEnabled = true;
			if (gameObject.activeInHierarchy == true) Destroy (gameObject); // Kills the game object
		}
	}

	public virtual IEnumerator DamageBlink() {
		PreDamageBlink ();
		mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 0.0f);
		yield return new WaitForSeconds (0.05f);
		if(hp > 0f || waitDerender == true) mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 1.0f);
		PostDamageBlink ();
	}
	
	public virtual void OnTriggerEnter2D(Collider2D other) { 
		PreOnTriggerEnter2D (other);
		if (other.CompareTag("Player")) other.SendMessageUpwards ("ApplyDamage", 1, SendMessageOptions.DontRequireReceiver); 
		PostOnTriggerEnter2D (other);
	}
	public void PreemptWake() { asleep = false; PostPreemptWake (); }

	protected virtual void PreOnBecameVisible() {}
	protected virtual void PreOnBecameInvisible() {}
	protected virtual void PreOnPauseGame() {}
	protected virtual void PreOnResumeGame() {}
	protected virtual void PreDisableCollider() {}
	protected virtual void PreStart() {}
	protected virtual void PreUpdate() {}
	protected virtual void PreFixedUpdate() {}
	protected virtual void PreStopScrolling() {}
	protected virtual void PreLaunchMusic() {}
	protected virtual void PreApplyDamage() {}
	protected virtual void PreDamageBlink() {}
	protected virtual void PreOnTriggerEnter2D(Collider2D other) {}

	protected virtual void PostOnBecameVisible() {}
	protected virtual void PostOnBecameInvisible() {}
	protected virtual void PostOnPauseGame() {}
	protected virtual void PostOnResumeGame() {}
	protected virtual void PostDisableCollider() {}
	protected virtual void PostStart() {}
	protected virtual void PostUpdate() {}
	protected virtual void PostFixedUpdate() {}
	protected virtual void PostStopScrolling() {}
	protected virtual void PostLaunchMusic() {}
	protected virtual void PostApplyDamage() {}
	protected virtual void PostDamageBlink() {}
	protected virtual void PostOnTriggerEnter2D(Collider2D other) {}
	protected virtual void PostPreemptWake() {}
}