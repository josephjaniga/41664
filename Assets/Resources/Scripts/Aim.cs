using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour {
	
	// enable or disable xbox remote
	public bool useMouse = true;

	void Update () {

		Vector2 mouse;
		Vector2 relmousepos;
		float angle;

		Vector3 objpos = Camera.main.WorldToViewportPoint (transform.position); //Object position on screen
		Vector2 relobjpos = new Vector2(objpos.x - 0.5f,objpos.y - 0.5f); //Set coordinates relative to object

		if ( useMouse ){
			mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition); //Mouse position
			relmousepos = new Vector2 (mouse.x - 0.5f,mouse.y - 0.5f) - relobjpos;
		} else {

			if ( Input.GetAxis("RightJoystickX") == 0f && Input.GetAxis("RightJoystickY") == 0f ){
				if ( transform.parent.transform.localScale.x >=0f ) { // facing right
					mouse = new Vector2( 1f, 0f );
				} else {  // facing left
					mouse = new Vector2( -1f, 0f );
				}

			} else {
				mouse = new Vector2( Input.GetAxis("RightJoystickX"), -Input.GetAxis("RightJoystickY")); //Mouse position
			}
			relmousepos = new Vector2 (mouse.x,mouse.y);
		}
		
		// facing right
		if ( transform.parent.transform.localScale.x >=0f ){
			angle = Vector2.Angle (Vector2.up, relmousepos);
		} else { // facing left
			angle = Vector2.Angle (-Vector2.up, relmousepos);
		}

		if (relmousepos.x > 0)
			angle = 360-angle;
		Quaternion quat = Quaternion.identity;

		// facing right
		if ( transform.parent.transform.localScale.x >=0f ){
			quat.eulerAngles = new Vector3(0,0,angle+90f); //Changing angle
		} else { // facing left
			quat.eulerAngles = new Vector3(0,0,angle+270f); //Changing angle
		}

		transform.rotation = quat;

	}

}
