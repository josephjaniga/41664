using UnityEngine;
using System.Collections;

public class Motion : MonoBehaviour {

	public float 	speed = 11.0f;
	public float 	jumpForce = 1000f;
	public bool		isGrounded = false;

	public float 	lastJump = 0f;
	public float 	jumpDelay = .25f;

	public Vector3 vShow;

	public bool isLeft = false;
	public Vector3 playerScale = new Vector3(2f, 2f, 2f);

	public bool isRunning = false;

	public Animator a;

	public bool useForce = true;

	// Use this for initialization
	void Start () {
	
		a = gameObject.GetComponent<Animator>();

	}

	// Update is called once per frame
	void Update () {

		vShow = rigidbody.velocity;

		float x = 0.0f;

		// space
		if ( Input.GetKey("space") ) {
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
		
		if ( rigidbody != null && x != 0f ){
			rigidbody.velocity = new Vector3(x, rigidbody.velocity.y, 0f);
		}

		if ( isLeft ){
			transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);
		} else {
			transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
		}

	}

	void OnCollisionEnter(){
		isGrounded = true;
	}
}
