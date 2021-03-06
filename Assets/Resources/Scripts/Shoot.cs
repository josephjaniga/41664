﻿using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	void Start(){

	}

	// Update is called once per frame
	void Update () {

		// WEAPON SELECTION
		if ( Input.GetKey(KeyCode.Alpha1) ){ // UNARMED
			WeaponManager.instance.type = Weapons.Unarmed;
			WeaponManager.instance.weaponSwitch();
		}

		if ( Input.GetKey(KeyCode.Alpha2) || Input.GetButton("DPadUp") ){ // SHOTGUN
			WeaponManager.instance.type = Weapons.ShotGun;
			WeaponManager.instance.weaponSwitch();
		}	

		if ( Input.GetKey(KeyCode.Alpha3) || Input.GetButton("DPadDown") ){ // ROCKET LAUNCHER
			WeaponManager.instance.type = Weapons.RocketLauncher;
			WeaponManager.instance.weaponSwitch();
		}			
		
		// LEFT MOUSE BUTTON
		if ( Input.GetKey(KeyCode.Mouse0) || Input.GetAxis("RightTrigger") > 0.25f ){
			WeaponManager.instance.activeWeapon.attackOne();
		}

		// RIGHT MOUSE BUTTON
		if ( Input.GetKey(KeyCode.Mouse1) 
			//|| Input.GetAxis("LeftTrigger") > 0.25f 
			){
			WeaponManager.instance.activeWeapon.attackTwo();
		}

    }

}