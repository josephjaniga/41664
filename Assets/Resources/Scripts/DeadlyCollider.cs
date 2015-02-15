using UnityEngine;
using System.Collections;

public class DeadlyCollider : MonoBehaviour {

	// Create Event Delegate Type and event Variable
	public delegate void DeadlyCollisionOccured();
	public static event DeadlyCollisionOccured KillPlayer;

	void OnCollisionEnter(){
		if ( KillPlayer != null ){ KillPlayer(); }
	}

}
