using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPoolerScript: MonoBehaviour {
	public string id;
	public GameObject pooledObject;
	public int amount = 3;
	public bool canGrow = true;
	
	public List<GameObject> pooledObjects = new List<GameObject>();
	
	public void Start() {
		for(int i = 0; i < amount; i++)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			obj.transform.parent = transform;
			pooledObjects.Add(obj);
		}
	}

	public bool Check(int needed) {
		if (pooledObject == null)
		{
			Debug.LogWarning("(ObjectPoolerScript " + id + ") pooledObject value is null !", gameObject);
			return false;
		}
		if (amount<=0) // if "amount" was not set in editor or set to incorrect value
		{
			Debug.LogWarning("(ObjectPoolerScript " + id + ") amount value is null or negative !", gameObject);
			return false;
		}
		if (canGrow == true)
			return true;

		for(int i = 0; i < pooledObjects.Count; i++) // Return the first inactive object from the list
		{	
			if (pooledObjects[i].activeInHierarchy == false)
				--needed;
			if(needed <= 0)
				return true;
		}
		return false;
	}
	public GameObject Spawn() {
		if (pooledObject == null)
		{
			Debug.LogWarning("(ObjectPoolerScript " + id + ") pooledObject value is null !", gameObject);
			return null;
		}
		if (amount<=0) // if "amount" was not set in editor or set to incorrect value
		{
			Debug.LogWarning("(ObjectPoolerScript " + id + ") amount value is null or negative !", gameObject);
			return null;
		}

		for(int i = 0; i < pooledObjects.Count; i++) // Return the first inactive object from the list
		{	
			if (pooledObjects[i].activeInHierarchy == false)
			{
				pooledObjects[i].SetActive(true);
				return pooledObjects[i];
			}
		}
		
		if (canGrow == true) // Else, none are found, so if the list allowed to grow then instantiate a new object, and add it to the list
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			pooledObjects.Add(obj);
			obj.transform.parent = transform;
			obj.SetActive(true);
			return obj;
		}
		return null; // Else, no objects are available, so return null
	}	
}