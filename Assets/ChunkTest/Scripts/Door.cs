using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Vector3 openPosition;
	public Vector3 closePosition;

	public bool targetPositionOpen = false;

	// Update is called once per frame
	void Update () {
		if ( targetPositionOpen ) {
			transform.position =  Vector3.Lerp(transform.position, openPosition, Time.deltaTime);
		} else {
			transform.position =  Vector3.Lerp(transform.position, closePosition, Time.deltaTime);
		}
	}

	
}
