using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

	public delegate void ShootAction(bool B);
	public static event ShootAction IsProjecting;

	public static WeaponManager instance { get; private set; }
	public Transform sparkEffect;
	public Transform explosionEffect;
	public AudioSource source;
	public Transform player;
	public Rigidbody playerRB;
	public IWeapon activeWeapon;
	public Transform firePoint;
	public AudioClip weaponSwap;

	public Weapons type = Weapons.RocketLauncher;

	public float lastShotSound = -5f;
	public float soundCD = 1.75f;
	public float lastShot = -5f;
	public float shotCD = 1.75f;
	public float shotForce = 4000f;

	public float lastSwapSound = -5f;
	public float swapSoundCD = 0.25f;

	public int playerLayerMask = 1 << 8;

	// enforce the singleton
	void Awake(){
		playerLayerMask = ~playerLayerMask;
		instance = this;
		DontDestroyOnLoad(gameObject);
		if ( transform.parent == null ){
			transform.SetParent( GameObject.Find("MainCamera").transform );
		}
	}

	void Start () {
		weaponSwap = Resources.LoadAssetAtPath<AudioClip>("Assets/Resources/Audio/WeaponSwap.wav");
		firePoint	= GameObject.Find("FirePoint").transform;
		player 		= GameObject.Find("Player").transform;
		playerRB 	= GameObject.Find("Player").GetComponent<Rigidbody>();
		source 		= GetComponent<AudioSource>();
		weaponSwitch();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void weaponSwitch(){
		if ( lastSwapSound + swapSoundCD <= Time.time ){
			hideAllWeapons();
			lastSwapSound = Time.time;
			source.PlayOneShot(weaponSwap);
			switch (type){
				default:
				case Weapons.Unarmed:
					activeWeapon = new Unarmed();
				break;
				case Weapons.ShotGun:
					activeWeapon = new ShotGun();
					showWeapon("M16");
				break;
				case Weapons.RocketLauncher:
					activeWeapon = new RocketLauncher();
					showWeapon("RocketLauncher");
				break;
			}
		}
	}

	public void hideAllWeapons(){
		GameObject.Find("M16").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find("RocketLauncher").GetComponent<SpriteRenderer>().enabled = false;
	}

	public void showWeapon(string s){
		GameObject temp = GameObject.Find(s);
		if ( temp != null ){
			temp.GetComponent<SpriteRenderer>().enabled = true;
		}
	}

	public void DelegateIsProjecting(){
		if ( IsProjecting != null ){
			IsProjecting(true);
		}
	}

	public void switchCheck(GameObject go){
		Switch temp = go.GetComponent<Switch>();
		if ( temp != null ){
			temp.activate();
		}
	}

}

public enum Weapons {
	Unarmed,
	ShotGun,
	RocketLauncher
}

public interface IWeapon {
	WeaponManager WM { set; get; }
	void attackOne();
	void attackTwo();
	void reload();
}

public class Unarmed : IWeapon {
	public AudioClip punchSound = Resources.LoadAssetAtPath<AudioClip>("Assets/Resources/Audio/Punch.wav");
	public float unarmedSoundCD = 0.25f;
	public WeaponManager WM { set; get; }
	public Unarmed (){ WM = WeaponManager.instance; }
	public void attackOne(){
		if ( WM.lastShotSound + unarmedSoundCD <= Time.time ){
			WM.lastShotSound = Time.time;
			WM.source.PlayOneShot(punchSound);
		}
	}
	public void attackTwo(){}
	public void reload(){}
}

public class ShotGun : IWeapon {
	public delegate void ShootAction(bool B);
	public static event ShootAction IsProjecting;
	public AudioClip shootSound = Resources.LoadAssetAtPath<AudioClip>("Assets/Resources/Audio/ShotGunShoot_1.wav");
	public WeaponManager WM { set; get; }
	public ShotGun (){ WM = WeaponManager.instance; }

	public void attackOne(){

		if ( WM.lastShotSound + WM.soundCD <= Time.time ){
			WM.lastShotSound = Time.time;
			WM.source.PlayOneShot(shootSound);
		}

		if ( WM.lastShot + WM.shotCD <= Time.time ){

			WM.DelegateIsProjecting();

			Vector3 mp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

	        RaycastHit hit;

	        Vector3 blowBack = WM.firePoint.position-mp;

	        //WM.playerRB.velocity = Vector3.zero;
			WM.playerRB.AddForce( WM.shotForce * blowBack.normalized );

			WM.lastShot = Time.time;

	        if (Physics.Raycast(WM.firePoint.position, mp-WM.firePoint.position, out hit, Mathf.Infinity, WM.playerLayerMask)){
	        	Transform temp =  GameObject.Instantiate(WM.sparkEffect, hit.point, Quaternion.identity) as Transform;
	        	WM.switchCheck(hit.transform.gameObject);
	        }
		}
	}

	public void attackTwo(){}
	public void reload(){}

}

public class RocketLauncher : IWeapon {
	
	public delegate void ShootAction(bool B);
	public static event ShootAction IsProjecting;
	public AudioClip shootSound = Resources.LoadAssetAtPath<AudioClip>("Assets/Resources/Audio/RocketShoot_1.wav");
	public WeaponManager WM { set; get; }
	public RocketLauncher (){ WM = WeaponManager.instance; }

	public float rocketLastShotSound = -5f;
	public float rocketSoundCD = .75f;
	public float rocketLastShot = -5f;
	public float rocketShotCD = .75f;
	public float rocketExplosionForce = 8000f;

	public void attackOne(){

		if ( rocketLastShotSound + rocketSoundCD <= Time.time ){
			rocketLastShotSound = Time.time;
			WM.source.PlayOneShot(shootSound);
		}

		if ( rocketLastShot + rocketShotCD <= Time.time ){

			WM.DelegateIsProjecting();

			Vector3 mp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
	        RaycastHit hit;

			rocketLastShot = Time.time;

	        if (Physics.Raycast(WM.firePoint.position, mp-WM.firePoint.position, out hit, Mathf.Infinity, WM.playerLayerMask)){
	        	Transform temp =  GameObject.Instantiate(WM.explosionEffect, hit.point, Quaternion.identity) as Transform;
	        	WM.switchCheck(hit.transform.gameObject);
	        	Vector3 blowBack = WM.firePoint.position - hit.point;
	        	float dist = Vector3.Distance(hit.point, WM.firePoint.position);

				if ( dist < 25f ){
					Vector3 tempForce = Mathf.Clamp( rocketExplosionForce / (dist/4), -rocketExplosionForce, rocketExplosionForce ) * blowBack.normalized;
					WM.playerRB.AddForce( tempForce );
				}
	        }
		}

	}

	public void attackTwo(){}
	public void reload(){}

}
