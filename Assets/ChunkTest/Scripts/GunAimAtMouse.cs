using UnityEngine;
using System.Collections;

public class GunAimAtMouse : MonoBehaviour {
	
	void Update () {

		float angle;
		Vector2 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition); //Mouse position
		Vector3 objpos = Camera.main.WorldToViewportPoint (transform.position); //Object position on screen
		Vector2 relobjpos = new Vector2(objpos.x - 0.5f,objpos.y - 0.5f); //Set coordinates relative to object
		Vector2 relmousepos = new Vector2 (mouse.x - 0.5f,mouse.y - 0.5f) - relobjpos;
		
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
