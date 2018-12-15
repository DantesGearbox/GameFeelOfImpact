using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	[HideInInspector] public Rigidbody2D rb;
	EnemyScaling scale;
	BoxCollider2D boxColl;
	AudioManager audioManager;
	public GameObject landingParticles;
	public GameObject slashParticles;
	public GameObject gotHitParticles2;
	public GameObject gotHitParticles3;

	//Collision variables
	public bool onGround = false;

	private float gravity = 100;
	private Vector2 launchSpeed = new Vector2(-10, 10);
	private float moveSpeed = -3;
	private float toughMoveSpeed = -2;
	private float acceleration = 0.1f;

	public bool toughEnemy = false;

	private bool blockStun = false;
	private float blockTime = 0.3f;
	private float blockTimer = 0.0f;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		boxColl = GetComponent<BoxCollider2D>();
		scale = GetComponentInChildren<EnemyScaling>();
		audioManager = FindObjectOfType<AudioManager>();
	}

	public void GotHit(float hitForce, Vector3 position) {

		if (toughEnemy) {
			Vector2 hitDir = (transform.position - position).normalized;

			Vector2 facingDir = new Vector2(hitDir.x, hitDir.y);
			float angle = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;
			Quaternion quat = Quaternion.AngleAxis(angle, Vector3.forward);
			Quaternion quat2 = Quaternion.AngleAxis(angle + 80, Vector3.forward);

			blockStun = true;
			rb.velocity = new Vector2(0, rb.velocity.y);

			//SFX
			audioManager.Play("ToughEnemyHit");

			//VFX
			GameObject particles = Instantiate(slashParticles, transform.position + new Vector3(0, 0.5f, 0), quat);
			ParticleSystem hitParticles = particles.GetComponent<ParticleSystem>();
			hitParticles.Play();

			//VFX
			GameObject particle = Instantiate(slashParticles, transform.position + new Vector3(0, 0.5f, 0), quat2);
			ParticleSystem hitParticle = particle.GetComponent<ParticleSystem>();
			//hitParticle.Play();

			//VFX
			GameObject particles3 = Instantiate(gotHitParticles3, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
			ParticleSystem hitParticles3 = particles3.GetComponent<ParticleSystem>();
			hitParticles3.Play();

			//VFX
			GameObject particles2 = Instantiate(gotHitParticles2, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
			particles2.transform.parent = this.transform;
			ParticleSystem hitParticles2 = particles2.GetComponent<ParticleSystem>();
			hitParticles2.Play();

			//Camera shake
			CameraShake.AddTrauma((hitForce) / 40);
		}

		if (!toughEnemy){
			Vector2 hitDir = (transform.position - position).normalized;

			Vector2 facingDir = new Vector2(hitDir.x, hitDir.y);
			float angle = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;
			Quaternion quat = Quaternion.AngleAxis(angle, Vector3.forward);
			Quaternion quat2 = Quaternion.AngleAxis(angle + 80, Vector3.forward);

			rb.velocity += new Vector2(10, 10);

			//SFX
			audioManager.Play("EnemySwordHit");

			//VFX
			GameObject particles = Instantiate(slashParticles, transform.position + new Vector3(0, 0.5f, 0), quat);
			ParticleSystem hitParticles = particles.GetComponent<ParticleSystem>();
			hitParticles.Play();

			//VFX
			GameObject particle = Instantiate(slashParticles, transform.position + new Vector3(0, 0.5f, 0), quat2);
			ParticleSystem hitParticle = particle.GetComponent<ParticleSystem>();
			hitParticle.Play();

			//VFX
			GameObject particles3 = Instantiate(gotHitParticles3, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
			ParticleSystem hitParticles3 = particles3.GetComponent<ParticleSystem>();
			hitParticles3.Play();

			//VFX
			GameObject particles2 = Instantiate(gotHitParticles2, transform.position + new Vector3(0, 0.0f, 0), Quaternion.identity);
			particles2.transform.parent = this.transform;
			ParticleSystem hitParticles2 = particles2.GetComponent<ParticleSystem>();
			hitParticles2.Play();

			//Camera shake
			CameraShake.AddTrauma((hitForce) / 40);
		}
	}

	private void CheckGroundCollision() {
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
			}
		}

		//Is only true the first frame that we are one the ground
		if (onGround) {
			//SFX
			audioManager.PlayWithRandomPitch("Landing");

			//VFX
			GameObject particles = Instantiate(landingParticles, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
			particles.GetComponent<ParticleSystem>().Play();

			if (!toughEnemy) {
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
	void Update () {

		Gravity();
		HorizontalMovement();

		if (blockStun) {
			blockTimer += Time.deltaTime;
			if(blockTimer > blockTime) {
				blockStun = false;
				blockTimer = 0.0f;
			}
		}

	}

	void Gravity() {

		if (!onGround) {
			if (toughEnemy) {
				rb.velocity -= new Vector2(0, gravity * Time.deltaTime * 2);
			}
			if (!toughEnemy) {
				rb.velocity -= new Vector2(0, gravity * Time.deltaTime);
			}
		}
	}

	void HorizontalMovement() {

		if (onGround && !blockStun) {
			if (toughEnemy) {
				if (rb.velocity.x > toughMoveSpeed) {
					rb.velocity -= new Vector2(acceleration, 0);
				}
				if (rb.velocity.x < toughMoveSpeed) {
					rb.velocity += new Vector2(acceleration, 0);
				}
			}
			if (!toughEnemy) {
				if (rb.velocity.x > moveSpeed) {
					rb.velocity -= new Vector2(acceleration, 0);
				}
				if (rb.velocity.x < moveSpeed) {
					rb.velocity += new Vector2(acceleration, 0);
				}
			}
		}
	}
}
