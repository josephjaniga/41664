using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject target;

	public float zoom = -55f;
	public float closest = -15f;
	public float farthest = -500f;


	// Use this for initialization
	void Start () {
	
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

		transform.position = new Vector3(target.transform.position.x, Mathf.Lerp(transform.position.y, target.transform.position.y, Time.deltaTime*5f), zoom);

	}
}
