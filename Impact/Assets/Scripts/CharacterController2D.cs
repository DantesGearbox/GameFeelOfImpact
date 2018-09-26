using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {

	//Key inputs
	public KeyCode jumpKey;
	public KeyCode leftKey;
	public KeyCode rightKey;

	//Unity components
	Rigidbody2D rb;
	BoxCollider2D boxColl;
	ScaleWithVelocity scale;
	SwordSwing swordSwing;
	AudioManager audioManager;

	//Physics variables - We set these
	private float maxJumpHeight = 3.5f;                 // If this could be in actual unity units somehow, that would be great
	private float minJumpHeight = 0.5f;                 // If this could be in actual unity units somehow, that would be great
	private float timeToJumpApex = 0.25f;               // This is in actual seconds
	private float maxMovespeed = 10;					// If this could be in actual unity units per second somehow, that would be great
	private float accelerationTime = 0.1f;              // This is in actual seconds
	private float deccelerationTime = 0.1f;             // This is in actual seconds

	//Physics variables - These get set for us
	private float maxFallingSpeed;
	private float gravity;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private float acceleration;
	private float decceleration;

	//Physics variables - State variables
	float leftSpeed = 0.0f;
	float rightSpeed = 0.0f;
	Vector2 previousVelocity;

	//Sprite settings variables
	private float inputDirection = 1.0f;

	//Collision variables
	public bool onGround = false;

	// Use this for initialization
	void Start() {
		rb = GetComponent<Rigidbody2D>();
		boxColl = GetComponent<BoxCollider2D>();
		scale = GetComponentInChildren<ScaleWithVelocity>();
		swordSwing = GetComponent<SwordSwing>();
		audioManager = FindObjectOfType<AudioManager>();
		SetupMoveAndJumpSpeed();
	}

	private void CheckGroundCollision(){
		//Get the 10 first contacts of the box collider
		ContactPoint2D[] contacts = new ContactPoint2D[10];
		int count = boxColl.GetContacts(contacts);

		//If we find any horizontal surfaces, we are on the ground
		onGround = false;
		for (int i = 0; i < count; i++) {

			//If the angle between the normal and up is less than 5, we are on the ground
			if (Vector2.Angle(contacts[i].normal, Vector2.up) < 5.0f) {
				onGround = true;
				rb.velocity = new Vector2(rb.velocity.x, 0);

				//SFX
				audioManager.PlayWithRandomPitch("Landing");

				//Landing animation
				scale.LandingAnimation();
			}
		}
	}

	//Whenever we collide, check if we are touching the ground
	private void OnCollisionEnter2D(Collision2D collision) {
		CheckGroundCollision();
	}

	private void OnCollisionExit2D(Collision2D collision) {
		CheckGroundCollision();
	}

	// Update is called once per frame
	void Update() {

		if (inputDirection == -1.0f && !swordSwing.isAttacking) {
			transform.localRotation = Quaternion.Euler(0, 180, 0);
		} else if(inputDirection == 1.0f && !swordSwing.isAttacking) {
			transform.localRotation = Quaternion.Euler(0, 0, 0);
		}

		Jumping();
		Gravity();
		HorizontalMovement();
		previousVelocity = rb.velocity;

	}

	public float GetInputDirection() {
		return inputDirection;
	}

	public float GetMinJumpVelocity(){
		return minJumpVelocity;
	}

	public float GetMaxJumpVelocity() {
		return maxJumpVelocity;
	}

	void Jumping() {
		//Setting the initial jump velocity

		if (Input.GetKey(jumpKey)) {
			if (onGround) {
				rb.velocity = new Vector2(rb.velocity.x, 0);
				rb.velocity += new Vector2(0, maxJumpVelocity);

				//SFX
				audioManager.PlayWithRandomPitch("Jump");

			}
		} else {
			if (rb.velocity.y > minJumpVelocity) {
				rb.velocity = new Vector2(rb.velocity.x, minJumpVelocity);
			}
		}
	}

	void Gravity(){
		//Gravity

		if(!onGround){
			rb.velocity -= new Vector2(0, gravity * Time.deltaTime);

			float temp = Mathf.Clamp(rb.velocity.y, -maxFallingSpeed, maxJumpVelocity);
			rb.velocity = new Vector2(rb.velocity.x, temp);

		}
		
	}

	void HorizontalMovement(){

		//If the rigid body velocity is ever set to zero, set the speed variables to zero as well
		if(rb.velocity.x == 0){
			leftSpeed = 0.0f;
			rightSpeed = 0.0f;
		}

		//Accelerate left
		if (Input.GetKey(leftKey)) {
			inputDirection = -1.0f;
			leftSpeed += acceleration * -1.0f * Time.deltaTime;
		} else {
			leftSpeed += decceleration * 1.0f * Time.deltaTime;
		}
		leftSpeed = Mathf.Clamp(leftSpeed, -maxMovespeed, 0.0f);

		//Accelerate right
		if (Input.GetKey(rightKey)) {
			inputDirection = 1.0f;
			rightSpeed += acceleration * 1.0f * Time.deltaTime;
		} else {
			rightSpeed += decceleration * -1.0f * Time.deltaTime;
		}
		rightSpeed = Mathf.Clamp(rightSpeed, 0.0f, maxMovespeed);

		//Set rigidbody velocity
		rb.velocity = new Vector2(rightSpeed + leftSpeed, rb.velocity.y);
	}

	void SetupMoveAndJumpSpeed() {
		//Scale gravity and jump velocity to jumpHeights and timeToJumpApex
		gravity = (2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = gravity * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * gravity * minJumpHeight);
		maxFallingSpeed = maxJumpVelocity * 1.25f;

		//Scale acceleration values to the movespeed and wanted acceleration times
		acceleration = maxMovespeed / accelerationTime;
		decceleration = maxMovespeed / deccelerationTime;

		//Set variables for the velocity scaling
		scale.SetMaxYSpeed(maxJumpVelocity);
	}
}
