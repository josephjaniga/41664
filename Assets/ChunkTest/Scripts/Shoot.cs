using UnityEngine;
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

		if ( Input.GetKey(KeyCode.Alpha2) ){ // SHOTGUN
			WeaponManager.instance.type = Weapons.ShotGun;
			WeaponManager.instance.weaponSwitch();
		}	

		if ( Input.GetKey(KeyCode.Alpha3) ){ // ROCKET LAUNCHER
			WeaponManager.instance.type = Weapons.RocketLauncher;
			WeaponManager.instance.weaponSwitch();
		}			
		
		// LEFT BUTTON
		if ( Input.GetKey(KeyCode.Mouse0) ){
			WeaponManager.instance.activeWeapon.attackOne();
		}

		// RIGHT BUTTON
		if ( Input.GetKey(KeyCode.Mouse1) ){
			WeaponManager.instance.activeWeapon.attackTwo();
		}

    }

}
