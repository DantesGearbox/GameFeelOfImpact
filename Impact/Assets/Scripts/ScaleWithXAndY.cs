using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithXAndY : MonoBehaviour {

	Rigidbody2D rb;
	AudioManager aud;
	public GameObject ps;

	float maxVelocity = 10.0f;
	float deccelerationTime = 0.5f;
	float decceleration;

	public Transform pbs;

	// Use this for initialization
	void Start() {
		rb = GetComponentInParent<Rigidbody2D>();
		aud = FindObjectOfType<AudioManager>();
		decceleration = maxVelocity / deccelerationTime;
	}
	


	private void Update() {

		if (rb.velocity.magnitude > 0.1f) {
			rb.velocity += rb.velocity.normalized * -decceleration * Time.deltaTime;
		} else {
			rb.velocity = Vector2.zero;
		}

		//Move punching bag back to it's spawnpoint
		if (Input.GetKeyDown(KeyCode.R)) {
			transform.parent.transform.position = pbs.position;
			rb.velocity = Vector2.zero;
		}
	}

	public void GotHit(float hitForce, Vector3 position) {

		Vector2 hitDir = (transform.position - position).normalized * hitForce * 1.2f;
		rb.velocity = hitDir;

		Vector2 facingDir = new Vector2(hitDir.x, hitDir.y);
		float angle = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		//SFX
		float pitch = (1/(hitForce / 40))-0.5f;
		aud.PlayWithPitch("EnemyHit", pitch);

		//VFX
		GameObject particles = Instantiate(ps, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
		ParticleSystem hitParticles = particles.GetComponent<ParticleSystem>();
		hitParticles.Play();

		//Camera shake
		CameraShake.AddTrauma((hitForce)/40);
	}

	void FixedUpdate() {

		//Going fast
		float fastScaleX = 2.0f;
		float fastScaleY = 0.5f;

		//Going slow
		float slowScaleX = 1.0f;
		float slowScaleY = 1.0f;

		//How lerped will the scale be
		float ySpeedRatio = Mathf.Abs(rb.velocity.magnitude) / 40;
		float xLerp = Mathf.Lerp(slowScaleX, fastScaleX, ySpeedRatio);
		float yLerp = Mathf.Lerp(slowScaleY, fastScaleY, ySpeedRatio);
		transform.localScale = new Vector2(xLerp, yLerp);
	}
}
