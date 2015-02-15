using UnityEngine;
using System.Collections;

public class Expiration : MonoBehaviour {

	public float lifeTime = 0.1f;
	private float deathTime;

	// Use this for initialization
	void Start () {
		deathTime = Time.time + lifeTime;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Time.time >= deathTime )
			Destroy(gameObject);
	}

}
