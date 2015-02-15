using UnityEngine;
using System.Collections;

public class LevelProgressCollider : MonoBehaviour {

	public string levelName = "";

	void OnCollisionEnter(){
		if ( levelName != "" ){
			Application.LoadLevel(levelName);
		}
	}

}