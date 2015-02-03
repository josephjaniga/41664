using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {

	public GameObject polyChunkInstance;
	public int listCount = 0;
	public int poolMax;

	// Use this for initialization
	void Start () {

		if ( poolMax == 0 ){
			poolMax = 128;		
		}

		for ( int i=0; i<poolMax; i++ ){
			GameObject go = Instantiate(polyChunkInstance, new Vector3(-9999f, -9999f, -9999f), Quaternion.identity) as GameObject;
			pushToStack(go);
		}

		listCount = countAvailable();
	}

	public GameObject popFromStack(){
		GameObject temp = getFirstAvailable();
		if ( temp == null ){
			temp = getOldestChunk();
		}
		return temp;
	}

	public void pushToStack(GameObject go){
		go.transform.SetParent( transform );
		go.transform.position = new Vector3(-9999f, -9999f, -9999f);
		go.GetComponent<PolyChunk>().inUse = false;
	}

	public GameObject getFirstAvailable(){
		GameObject temp = null;
		foreach ( Transform child in transform ) {
			if ( !child.gameObject.GetComponent<PolyChunk>().inUse ){
				temp = child.gameObject;
				break;
			}
		}
		return temp;
	}

	public int countAvailable(){
		int temp = 0;
		foreach ( Transform child in transform ) {
			if ( !child.gameObject.activeSelf  ){
				temp++;
			}
		}
		return temp;
	}

	public GameObject getOldestChunk(){
		GameObject temp = null;
		float smallest = Time.time;
		foreach ( Transform child in transform ) {
			if ( child.gameObject.GetComponent<PolyChunk>().startTime < smallest ){
				temp = child.gameObject;
				smallest = child.gameObject.GetComponent<PolyChunk>().startTime;
			}
		}
		if ( temp == null ){
			foreach ( Transform child in transform ) {
				temp = child.gameObject;
			}
		}
		return temp;
	}

	public GameObject getChunkAtPosition(float x, float y){
		GameObject temp = null;
		foreach ( Transform child in transform ) {
			if ( child.position.x == x && child.position.y == y ){
				temp = child.gameObject;
				break;
			}
		}
		return temp;
	}

}
