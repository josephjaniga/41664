﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public string name = "";
	public Transform startingPosition;

	public GameObject player;
	public bool isDead;

	public GameObject camera;

	public string nextLevelName = "";

	// Use this for initialization
	void Start () {

		if ( startingPosition == null ){
			startingPosition = GameObject.Find("StartingPosition").transform;
		}

		camera = _.c;
		player = _.p;

		player.transform.position = startingPosition.position;

	}
	
	// Update is called once per frame
	void Update () {

		if ( isDead ){
			isDead = false;
			player.rigidbody.velocity = Vector3.zero;
			player.transform.position = startingPosition.position;
		}
	
	}

	// DELEGATE EVENT SYSTEM FOR IS GROUNDED
	void OnEnable(){
		DeadlyCollider.KillPlayer += kill;
		VictoryCollider.Progress += nextLevel;
	}
	void OnDisable(){
		DeadlyCollider.KillPlayer -= kill;
		VictoryCollider.Progress -= nextLevel;
	}

	public void kill(){
		isDead = true;
	}

	public void nextLevel(){
		if ( nextLevelName != "" ){
			Application.LoadLevel(nextLevelName);
		}
	}

}
