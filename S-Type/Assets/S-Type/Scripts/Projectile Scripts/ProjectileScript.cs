using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	[SerializeField] protected ObjectPoolerScript impactPool; // This targets the impacts ObjectPool
	[SerializeField] protected bool pierceTerrain; // Pierces through the terrain?
	[SerializeField] protected int layer; // Sorting layer
	
   public WeaponScript firingWeapon;
   public Transform transformPtr;
	public Vector2 direction; // The direction this object is travelling
	public float rotation;
	public Color color;
	public float speed; // Speed of our thing
	public float damage; // How much damage can we do
   public int id;

	protected PlayerScript playerScript;
	protected Transform playerTr;
	protected Transform imageTr;

	protected SpriteRenderer mySpriteRdr; // Renders the sprite
	protected Animator myAnimator; // Animator using Runtime Animator Controllers
	protected GameObject myGo; // This game object
	protected Transform myTr; // This transform
	protected bool ready; // Can we go?


	
	/************************/
	public float boundToCamDelay;
	public float boundToCamDelayMax = 5.0f;
	bool earned = false;
	//the number of reflections  
	public int nReflections = 3;  //not being used atm
	Camera cam;
	Transform camTr;
	public bool Reflect = false;
	/*************************/


	public virtual void OnEnable() { // By object pool's spawn function
		PreOnEnable ();
		if (ready == false) {
			return; 
		}
		StartCoroutine(OnSpawn ()); // Spawn if ready

		PostOnEnable ();
	}

	public virtual IEnumerator OnSpawn() { // Called by "OnEnable() above when ready"
		PreOnSpawn ();
		yield return null; // Wait a frame

		if (mySpriteRdr != null && mySpriteRdr.isVisible == false) {
			StartCoroutine (DestroyObject ()); // Our pooled bullet was enabled but seems to be out of screen bounds, so destroy it.
		}
		else if(myTr.GetComponent<Collider2D>() != null)
         myTr.GetComponent<Collider2D>().enabled = true; // Enable collider

		myTr.eulerAngles = new Vector3 (0, 0, rotation);
		AudioSource mySource = myTr.GetComponent<AudioSource> ();
		if (mySource != null && mySource.clip != null)
			mySource.Play ();

		ready = true;
		PostOnSpawn ();
	}
	public virtual void OnBecameInvisible() {
		PreOnBecameInvisible ();
		if (gameObject.activeInHierarchy == true) 
			StartCoroutine(DestroyObject()); // On became invisible, destroy object.
		PostOnBecameInvisible ();
	}
	
	public virtual IEnumerator Start () {
		PreStart ();

		myGo = gameObject; // cache object gameObject
		myTr = transform; // cache transform


		/***********************/
		cam = Camera.main;
		camTr = cam.transform;
		/***********************/

		GameObject player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<PlayerScript>();
		playerTr =  player.transform;
		imageTr = playerScript.imgTr;

		myAnimator = myTr.GetComponent<Animator> (); // Get animator
		mySpriteRdr = myTr.GetComponent<SpriteRenderer>(); // Get sprite renderer
		if (mySpriteRdr != null) {
			mySpriteRdr.sortingOrder = layer;
			mySpriteRdr.color = color;
		}
		myTr.eulerAngles = new Vector3 (0, 0, rotation);


		GameObject impactPoolGO = GameObject.Find("ObjectPool Impacts");
		impactPool = impactPoolGO.GetComponent<ObjectPoolerScript>() as ObjectPoolerScript;

		AudioSource mySource = myTr.GetComponent<AudioSource> ();
		if (mySource != null && mySource.clip != null)
			mySource.Play ();

		yield return null;
		if (mySpriteRdr != null && mySpriteRdr.isVisible == false) {
         OnBecameInvisible ();
      }
		else
			ready = true;
		PostStart ();
	}
	public virtual void Update() { // Basic movement based on speed and direction
		PreUpdate ();
		
		/***************************/
		
		if ( boundToCamDelay < boundToCamDelayMax && earned == false && Reflect == true)
		{	
			//increment "boundToCamDelay"
			boundToCamDelay = boundToCamDelay + 1 * Time.deltaTime;
			//nReflections--;
			
			if (myTr.position.y >= camTr.position.y + 0.95f)
			{	
				direction.y = -direction.y;
				nReflections = nReflections - 1;
			}
			
			else
				
				if (myTr.position.y <= camTr.position.y - 0.95f)
			{	
				direction.y = -direction.y;
				nReflections = nReflections - 1;
			}
			
			if (myTr.position.x >= camTr.position.x + 1.5f)
			{	
				
				
				direction.x = -direction.x;
				nReflections = nReflections - 1;
			}
			
			else
				
				if (myTr.position.x <= camTr.position.x - 1.5f)
			{	
				direction.x = -direction.x;
				nReflections = nReflections - 1;
			}
		}
		
		/**********************************************/
		myTr.position = myTr.position + new Vector3(direction.x * speed, direction.y * speed, 0.0f)  * Time.deltaTime;
		PostUpdate ();
	}

	public virtual IEnumerator DestroyObject() {
		PreDestroyObject ();
		yield return null;
		myGo.SetActive(false);
		/*************************/
		//reset delay or else next time its setActive timer never resets and wont reflect
		if (Reflect == true) {
			boundToCamDelay = 0f;
			//nReflections ==3 //not being used, my be able to use instead of timer delay for reflections
		}
		/*************************/
		PostDestroyObject ();
	}

	public virtual void OnTriggerEnter2D(Collider2D other) { // Only ground for the base
		PreOnTriggerEnter2D (other);
		if (other.CompareTag("Ground") && pierceTerrain == false) // If we pierce through terrain, we don't have to worry about this
		{
			GameObject impactClone = impactPool.Spawn();
			impactClone.transform.position = myTr.position;
			StartCoroutine(DestroyObject()); // Call the DestroyObject function
			return; // Skip PostOnTriggerEnter
		}
		PostOnTriggerEnter2D (other); // For colliding with enemy/player depending on which type of projectile
	}
	public virtual void OnCollisionEnter2D(Collision2D other) {
		PreOnCollisionEnter2D (other);
		if (other.gameObject.CompareTag("Ground") && pierceTerrain == false) // If we pierce through terrain, we don't have to worry about this
		{
			GameObject impactClone = impactPool.Spawn();
			impactClone.transform.position = myTr.position;
			StartCoroutine(DestroyObject()); // Call the DestroyObject function
			return; // Skip PostOnTriggerEnter
		}
		PostOnCollisionEnter2D (other); // For colliding with enemy/player depending on which type of projectile
	}

	protected virtual void PreOnEnable() {}
	protected virtual void PreOnSpawn() {}
	protected virtual void PreOnBecameInvisible() {}
	protected virtual void PreStart()  {}
	protected virtual void PreUpdate() {}
	protected virtual void PreDestroyObject() {}
	protected virtual void PreOnTriggerEnter2D(Collider2D other) {}
	protected virtual void PreOnCollisionEnter2D(Collision2D other) {}

	protected virtual void PostOnEnable() {}
	protected virtual void PostOnSpawn() {}
	protected virtual void PostOnBecameInvisible() {}
	protected virtual void PostStart()  {}
	protected virtual void PostUpdate() {}
	protected virtual void PostDestroyObject() {}
	protected virtual void PostOnTriggerEnter2D(Collider2D other) {}
	protected virtual void PostOnCollisionEnter2D(Collision2D other) {}
}