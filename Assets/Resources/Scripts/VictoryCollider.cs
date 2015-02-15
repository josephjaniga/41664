using UnityEngine;
using System.Collections;

public class VictoryCollider : MonoBehaviour {

	// Create Event Delegate Type and event Variable
	public delegate void VictoryCollisionOccured();
	public static event VictoryCollisionOccured Progress;

	void OnCollisionEnter(){
		if ( Progress != null ){ Progress(); }
	}

}
