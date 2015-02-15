using UnityEngine;
using System.Collections;

public static class _
{

	private static GameObject 	_player;
	private static GameObject 	_camera;
	private static AudioSource 	_source;

	// player
	public static GameObject p 
	{
		get {
			GameObject temp = GameObject.Find("Player");
			if ( _player == null ){
				if ( temp == null ){
					temp = GameObject.Instantiate(Resources.Load("Prefabs/Player"), Vector3.zero, Quaternion.identity) as GameObject;
					temp.name = "Player";
					_player = temp;
				}
			}
			return _player;
		}
		set { _player = value; }
	}

	// camera
	public static GameObject c 	
	{
		get {
			GameObject temp = GameObject.Find("MainCamera");
			if ( _camera == null  ){
				if ( temp == null  ){
					temp = GameObject.Instantiate(Resources.Load("Prefabs/MainCamera"), Vector3.zero, Quaternion.identity) as GameObject;
					temp.name = "MainCamera";
					_camera = temp;
				}
			}
			return _camera;
		}
		set { _camera = value; }
	}

	public static AudioSource	s; // audio source

}