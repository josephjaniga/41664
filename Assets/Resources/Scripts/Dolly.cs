using UnityEngine;
using System.Collections;

public class Dolly : MonoBehaviour {

	public GameObject target;

	public float zoom = -66f;
	public float closest = -66f;
	public float farthest = -500f;

	public bool shouldLerp = false;

	// Use this for initialization
	void Start () {
		if ( target == null ){
			target = GameObject.Find("Player");
		}
	}
	
	// Update is called once per frame
	void Update () {

		if ( Input.GetAxis("Mouse ScrollWheel") > 0) { // forward
			if ( zoom + 14 < closest ){
				zoom += 14;
			} else {
				zoom = closest;
			}
		}

		if ( Input.GetAxis("Mouse ScrollWheel") < 0) { // back
			if ( zoom - 14 > farthest ){
				zoom -= 14;
			} else {
				zoom = farthest;
			}
		}

		if ( shouldLerp ){
			transform.position = new Vector3(target.transform.position.x, Mathf.Lerp(transform.position.y, target.transform.position.y, Time.deltaTime*5f), zoom);		
		} else {
			transform.position = new Vector3(target.transform.position.x, target.transform.position.y, zoom);
		}
		

	}
}
