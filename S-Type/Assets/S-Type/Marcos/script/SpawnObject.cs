using UnityEngine;
using System.Collections;

public class SpawnObject : MonoBehaviour {

   private Vector3 startPos; //x,y,z cords
   public float moveSpeed = 1.0f; //move speed
   public float moveDistance = 4f; // how much it moves
   public GameObject spawnedObj;
   public float timeLeftUntilSpawn = 0;
   public float startTime = 0;
   public float secondsBetweenSpawn = 2.0f;
    //GameObject activate = GameObject.Find("POOL").GetComponent<POOL>();
   public Opool poolScript; 
	public int move = 4;




   private float newXpos;


	// Use this for initialization
	void Start () {
      newXpos = transform.position.x;
      startPos = transform.position; // get start pos

	}

   void SpawnRandom(){
      GameObject myObj = Instantiate (spawnedObj) as GameObject;//gameobject instance
     // GameObject myObj = poolScript.ActivateObject();

      myObj.transform.position = transform.position; //create object at location of spawnder
   }
	
	// Update is called once per frame
	void Update () {
      //newXpos++;
      //no other gameobj specified so main game obj moved. 
      newXpos = Mathf.PingPong (Time.time * moveSpeed, move) - (moveDistance / 2f) ;
      //transform.position = new Vector3 (newXpos, startPos.y, startPos.z);
      transform.position = new Vector3(newXpos, startPos.y, startPos.z); // x between -1 and 3
      //transform.localScale = new Vector3 (Mathf.PingPong (t  , length-1)+1, Mathf.PingPong (t , length-1)+1, 0);
      timeLeftUntilSpawn = Time.time - startTime;
      if (timeLeftUntilSpawn >= secondsBetweenSpawn) {
         startTime = Time.time - Random.Range(0.1f, 0.5f);//random variation between spawn
         timeLeftUntilSpawn = 0;
         //Debug.Log("Spawn a object");
         SpawnRandom();
         //poolScript.Test();
      }
	}
}
