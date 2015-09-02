using UnityEngine;
using System.Collections;

public class Opool : MonoBehaviour {

   public GameObject[] objects = null;
   public GameObject objectToInst ;
   public int poolSize = 0;
	// Use this for initialization
	void Start () {
      objects = new GameObject[poolSize];
      for (int i = 0; i < poolSize; i++) {
         objects[i]  = Instantiate(objectToInst) as GameObject;
         objects[i].transform.parent = gameObject.transform; //nest pool under object
         objects[i].SetActive(false);
      }
	
	}


	public void ActivateObject(){
      //cycle thru array, and search for a non activated
      for (int i = 0; i < poolSize; i++) {
         if( objects[i].activeInHierarchy == false){
             objects[i].SetActive(true);
            return; //found one so return
         }

      }
   }

   public void Test(){
      Debug.Log ("HELLLLLLLLLLLLLLLOOOOOOOOOO");
   }

}
