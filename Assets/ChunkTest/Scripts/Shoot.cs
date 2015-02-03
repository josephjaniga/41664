using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public Transform effect;
	public AudioClip shootSound;
	public AudioSource source;

	public float lastShot = 0f;
	public float cd = 0.75f;

	public Transform player;
	public Rigidbody playerRB;

		
	void Start(){
		player = GameObject.Find("Player").transform;
		playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		
		if ( Input.GetKey(KeyCode.Mouse0) && lastShot + cd <= Time.time ){
			lastShot = Time.time;
			source.PlayOneShot(shootSound);
		}

		if ( Input.GetKey(KeyCode.Mouse0) ){

			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
			mousePosition.z = 0f;
	        RaycastHit hit;

	        Vector3 blowBack = mousePosition-transform.position;
	        //print(blowBack);
			playerRB.AddForce( 15 * blowBack );

	        //Debug.DrawRay(transform.position, transform.position-mousePosition, Color.red);
	        if (Physics.Raycast(transform.position, transform.position-mousePosition, out hit)){
	        	//print(hit.point);
	        	Transform temp = Instantiate(effect, hit.point, Quaternion.identity) as Transform;
	        	//Destroy(temp.gameObject);
	        }
		}

    }



}
