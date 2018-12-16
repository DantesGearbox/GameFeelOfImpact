using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithXAndY : MonoBehaviour {

	Rigidbody2D rb;
	AudioManager aud;
	public GameObject ps;
	public GameObject slashParticles;

	float maxVelocity = 10.0f;
	float deccelerationTime = 0.5f;
	float decceleration;

	public Transform pbs;

	private GameFeelManager gfm;

	// Use this for initialization
	void Start() {
		rb = GetComponentInParent<Rigidbody2D>();
		aud = FindObjectOfType<AudioManager>();
		decceleration = maxVelocity / deccelerationTime;
		gfm = FindObjectOfType<GameFeelManager>();
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

	public void ResetPunchingBag() {
		//Move punching bag back to it's spawnpoint
		transform.parent.transform.position = pbs.position;
		rb.velocity = Vector2.zero;
	}

	public void GotHit(float hitForce, Vector3 position) {

		Vector2 hitDir = (transform.position - position).normalized * hitForce * 1.0f;

		Vector2 facingDir = new Vector2(hitDir.x, hitDir.y);
		float angle = Mathf.Atan2(facingDir.y, facingDir.x) * Mathf.Rad2Deg;
		Quaternion quat = Quaternion.AngleAxis(angle, Vector3.forward);
		Quaternion quat2 = Quaternion.AngleAxis(angle + 80, Vector3.forward);

		if (!gfm.disableBlockStun) {
			rb.velocity = hitDir;
		}

		if (!gfm.disableAnimations) {
			transform.rotation = quat;
		}

		if (!gfm.disableSoundEffects) {
			//SFX
			float pitch = (1 / (hitForce / 40)) - 0.5f;
			aud.PlayWithPitch("EnemyHit", pitch);
		}

		if (!gfm.disableParticles) {
			//VFX
			GameObject particles = Instantiate(ps, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
			ParticleSystem hitParticles = particles.GetComponent<ParticleSystem>();
			hitParticles.Play();

			//VFX
			GameObject particle = Instantiate(slashParticles, transform.position + new Vector3(0, 0.5f, 0), quat2);
			ParticleSystem hitParticle = particle.GetComponent<ParticleSystem>();
			hitParticle.Play();

			//VFX
			GameObject slashPart = Instantiate(slashParticles, transform.position + new Vector3(0, 0.5f, 0), quat);
			ParticleSystem hitPart = slashPart.GetComponent<ParticleSystem>();
			hitPart.Play();
		}

		if (!gfm.disableScreenShake) {
			//Camera shake
			CameraShake.AddTrauma((hitForce) / 40);
		}
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

		if (gfm.disableAnimations) {
			transform.localScale = new Vector2(1, 1);
		}
	}
}
