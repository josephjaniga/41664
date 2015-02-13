using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	public bool active = false;
	public float switchLastActivated = -5f;
	public float switchCD = 4f;

	public Vector3 offPosition;
	public Vector3 onPosition;

	public Color inactiveColor = Color.red;
	public Color activeColor = Color.green;

	void Start(){
		offPosition = transform.localPosition;
		renderer.material.shader = Shader.Find("Diffuse");
	}

	// Update is called once per frame
	void Update () {
		
		if ( switchLastActivated + switchCD <= Time.time ){
			active = false;
		}

		if ( active ){
        	renderer.material.color = activeColor;
        	if ( transform.localPosition != onPosition )
        		transform.localPosition = Vector3.MoveTowards(transform.localPosition, onPosition, .1f);
		} else {
        	renderer.material.color = inactiveColor;
        	if ( transform.localPosition != offPosition )
        		transform.localPosition = Vector3.MoveTowards(transform.localPosition, offPosition, .1f);
		}

	}

	void OnCollisionEnter(){
		activate();
	}

	public void activate (){
		active = true;
		switchLastActivated = Time.time;
	}

}
