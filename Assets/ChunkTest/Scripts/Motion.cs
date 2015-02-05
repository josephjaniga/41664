using UnityEngine;
using System.Collections;

public class Motion : MonoBehaviour {

	// debug
	public Vector3 vShow;

	// running
	public float 	speed = 11.0f;

	// jumping
	public bool		isGrounded = false;
	public float 	jumpForce = 1000f;
	public float 	lastJump = 0f;
	public float 	jumpDelay = .25f;
	public GameObject pressurePlate;

	// shooting for momentum
	public bool 	isProjecting = false;
	public bool 	lastGrounded = false;
	public float 	projectingMaxTime = 0.1f;
	public float 	lastProjection = 0f;

	// animating
	public bool isLeft = false;
	public Vector3 playerScale = new Vector3(2f, 2f, 2f);
	public bool isRunning = false;
	public Animator a;


	// Use this for initialization
	void Start () {
		a = gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {

		if ( isProjecting ){
			pressurePlate.SetActive(false);
			if ( rigidbody.velocity == Vector3.zero && lastProjection + projectingMaxTime <= Time.time && lastGrounded == isGrounded ){
				isGrounded = true;
				isProjecting = false;				
			}
		} else {
			pressurePlate.SetActive(true);
		}

		vShow = rigidbody.velocity;

		if ( rigidbody.velocity.y < 0 && isProjecting ){
			isProjecting = false;
		}

		float x = 0.0f;

		// space
		if ( Input.GetKey("space") ) {

			if ( isGrounded ){ // if on the ground
				if ( Time.time > lastJump + jumpDelay ){
					rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
					if ( rigidbody != null )
						rigidbody.AddForce(new Vector3(0f, jumpForce, 0f));
					else 
						rigidbody2D.AddForce(new Vector3(0f, jumpForce));
					isGrounded = false;
					lastJump = Time.time;
				}
			}

		}
	
		if ( isGrounded ){
			if ( Input.GetKey("a") ){	// left
				x = -1f * speed;
				isLeft = true;
				a.SetBool("isRunning", true);
			} else if ( Input.GetKey("d") ){	// right
				x = 1f * speed;
				isLeft = false;
				a.SetBool("isRunning", true);
			} else {
				a.SetBool("isRunning", false);
			}
		}
		/*
		else {
			if ( Input.GetKeyDown(KeyCode.A) ){	// left
				x = -.75f * speed;
				isLeft = true;
				a.SetBool("isRunning", true);
			} else if ( Input.GetKeyDown(KeyCode.D) ){	// right
				x = .75f * speed;
				isLeft = false;
				a.SetBool("isRunning", true);
			}
		}
		*/
		
		if ( rigidbody != null){
			if ( x == 0f && isGrounded ){
				rigidbody.velocity = new Vector3(x, rigidbody.velocity.y, 0f);
			} else if ( x != 0f ){
				rigidbody.velocity = new Vector3(x, rigidbody.velocity.y, 0f);
			}
		}

		if ( isLeft ){
			transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);
		} else {
			transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
		}

	}

	// DELEGATE EVENT SYSTEM FOR IS GROUNDED
	void OnEnable(){
		Ground.HasGrounded 		+= IsNowGrounded;
		Ground.HasNotGrounded 	+= IsNotGrounded;
		Shoot.IsProjecting		+= Projecting;
	}
	void OnDisable(){
		Ground.HasGrounded 		-= IsNowGrounded;
		Ground.HasNotGrounded 	-= IsNotGrounded;
		Shoot.IsProjecting		+= Projecting;
	}

	void IsNowGrounded(){ isGrounded = true; }
	void IsNotGrounded(){ isGrounded = false; }
	void Projecting( bool B ){ 
		IsNotGrounded();
		isProjecting = B; 
		if ( B ){
			lastProjection = Time.time;
			lastGrounded = isGrounded;
		}
	}

}
