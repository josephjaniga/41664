using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	// Create Event Delegate Type and event Variable
	public delegate void GroundAction();
	public static event GroundAction HasGrounded;
	public static event GroundAction HasNotGrounded;

	void OnTriggerEnter(){
		// OnTriggerEnter Broadcast the HasGrounded event if it has listeners
		if ( HasGrounded != null ){ HasGrounded(); }
	}

	void OnTriggerExit(){
		// OnTriggerEnter Broadcast the HasNotGrounded event if it has listeners
		if ( HasNotGrounded != null ){ HasNotGrounded(); }
	}

}
