using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public Transform effect;
	public AudioClip shootSound;
	public AudioSource source;

	public float lastShotSound = 0f;
	public float soundCD = 1.75f;
	public float lastShot = 0f;
	public float shotCD = 1.75f;

	public float shotForce = 750f;

	public Transform player;
	public Rigidbody playerRB;

	public delegate void ShootAction(bool B);
	public static event ShootAction IsProjecting;
		
	void Start(){
		player = GameObject.Find("Player").transform;
		playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		
		if ( Input.GetKey(KeyCode.Mouse0) && lastShotSound + soundCD <= Time.time ){
			lastShotSound = Time.time;
			source.PlayOneShot(shootSound);
		}

		if ( Input.GetKey(KeyCode.Mouse0) && lastShot + shotCD <= Time.time ){

			if ( IsProjecting != null ){
				IsProjecting(true);
			}

			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
			mousePosition.z = 0f;
	        RaycastHit hit;

	        Vector3 blowBack = mousePosition-transform.position;
			playerRB.AddForce( shotForce * blowBack );

			lastShot = Time.time;

	        //Debug.DrawRay(transform.position, transform.position-mousePosition, Color.red);
	        if (Physics.Raycast(transform.position, transform.position-mousePosition, out hit)){
	        	Transform temp = Instantiate(effect, hit.point, Quaternion.identity) as Transform;
	        }
		}

    }



}
