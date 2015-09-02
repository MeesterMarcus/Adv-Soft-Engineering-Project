using UnityEngine;
using System.Collections;





public class ScrollingMeteor : MonoBehaviour {
	
	
	
	public GameObject boulder;
	public Transform myTrans;
	Transform camTr;
	public float fallSpeed = 0.5f;
	PlayerScript playerScript;
	SpriteRenderer mySpriteRdr;
	Transform myTr;
	
	public int hp = 5;
	public int scoreValue = 1;
	[HideInInspector] int damageValue = 1;
	bool asleep = false;
	public float sec = 5f;
	
	public float boundToCamDelay;
	public float boundToCamDelayMax = 5.0f;
	
	
	[SerializeField] protected int explosions;
	protected ObjectPoolerScript explosionPool; // Explosion object pool
	[SerializeField] protected Vector2 randomMax;
	[SerializeField] protected Vector2 randomMin;
	
	
	
	public void Start()
	{
		myTr = transform;
		mySpriteRdr = myTr.GetComponent<SpriteRenderer>();
		playerScript = GameObject.FindWithTag ("Player").GetComponent<PlayerScript>(); 
		//  if (gameObject.activeInHierarchy)
		//    gameObject.SetActive(false);
		explosionPool = GameObject.Find("ObjectPool EnemyExplosions").GetComponent<ObjectPoolerScript>();  
		//StartCoroutine(LateCall());
		
	} 
	
	
	
	IEnumerator LateCall()
	{
		
		yield return new WaitForSeconds(sec);
		
		Destroy (gameObject);
		boundToCamDelay = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.left * fallSpeed * Time.deltaTime);
		
		if (boundToCamDelay > boundToCamDelayMax) {
			Destroy(gameObject);
			
		}
		
		//if (myTr.position.x <= camTr.position.x + -1.5f) {
		//	Destroy(gameObject);
		//}
		boundToCamDelay = boundToCamDelay + 1 * Time.deltaTime;
	}
	
	

	

	
	public IEnumerator DamageBlink()
	{
		Debug.Log ("Taking Damage");
		mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 0.0f);
		
		yield return new WaitForSeconds (0.02f);
		
		mySpriteRdr.color = new Color (mySpriteRdr.color.r, mySpriteRdr.color.g, mySpriteRdr.color.b, 1.0f);
		
		
	}
	
	public IEnumerator ApplyDamage(int damage)
	{  
		hp = hp-1;
		if (asleep == true) yield break;
		if (hp > 0)
		{
			StartCoroutine(DamageBlink ());
			// ... play an impact sound
		}
		
		else
		{  
			for(int i = 0; i < explosions; i++) {
				GameObject explosionClone = explosionPool.Spawn();
				float randomXPos = UnityEngine.Random.Range(randomMin.x, randomMax.x);
				float randomYPos = UnityEngine.Random.Range(randomMin.y, randomMax.y);
				explosionClone.transform.position = new Vector3(myTr.position.x + randomXPos, myTr.position.y + randomYPos, explosionClone.transform.position.z);
			}
			
			StartCoroutine(DestroyObject());
			//playerScript.UpdateScore (scoreValue);
		}
		
	}
	
	
	
	
	
	public IEnumerator DestroyObject()
	{
		
		yield return new WaitForSeconds (0.1f); // Wait for the end of explosion audio clip
		
		if (gameObject.activeInHierarchy == true) Destroy (gameObject); // Kills the game object
		
	}
	
	
	
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		
		if (other.CompareTag ("Player")) {
			
			other.SendMessageUpwards ("ApplyDamage", 1, SendMessageOptions.DontRequireReceiver);
		}
	}
	
}




