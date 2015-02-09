using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistantObject : MonoBehaviour
{

	void Start () {
		persistHierarchy(gameObject);
	}

	public void persistHierarchy (GameObject go) {
		DontDestroyOnLoad(go);
		foreach( Transform child in go.transform ){
			persistHierarchy(child.gameObject);
		}
	}
	
}