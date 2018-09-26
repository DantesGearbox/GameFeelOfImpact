using UnityEngine;

public class ScaleWithVelocity : MonoBehaviour {

	private Rigidbody2D rb;
	private CharacterController2D cc;
	private SwordSwing ss;

	//The speeds to reach before maximum scaling is acheived
	private float maxYSpeed = 0.0f;
	private float maxXSpeed = 0.0f;

	private float xScaleSwing1 = 1.2f;
	private float yScaleSwing1 = 0.8f;

	private float xScaleSwing2 = 0.8f;
	private float yScaleSwing2 = 1.2f;

	private bool landingAnimation = false;
	private float downwardMomentum = 0.0f;
	private float landingAnimationLength = 0.1f;
	private float landingAnimationTimer = 0.0f;
	private float xScaleLanding = 1.5f;
	private float yScaleLanding = 0.75f;

	private bool walkingScale = false;

	private void Start() {
		rb = GetComponentInParent<Rigidbody2D>();
		cc = GetComponentInParent<CharacterController2D>();
		ss = GetComponentInParent<SwordSwing>();
	}

	void FixedUpdate() {

		//Going fast
		float fastScaleX = 0.5f;
		float fastScaleY = 2.0f;

		//Going slow
		float slowScaleX = 1.0f;
		float slowScaleY = 1.0f;

		//How lerped will the scale be
		float ySpeedRatio = Mathf.Abs(rb.velocity.y) / maxYSpeed;
		float xSpeedRatio = Mathf.Abs(rb.velocity.x) / maxXSpeed;
		float xLerp = Mathf.Lerp(slowScaleX, fastScaleX, ySpeedRatio);
		float yLerp = Mathf.Lerp(slowScaleY, fastScaleY, ySpeedRatio);
		transform.localScale = new Vector2(xLerp, yLerp);

		//Walking scale
		if(cc.onGround && Mathf.Abs(rb.velocity.x) > 0.1f) {
			walkingScale = true;
			transform.localScale = new Vector2(1.25f, 1.0f);
		} else {
			walkingScale = false;
		}

		//Landing scaling
		if (landingAnimation && !Input.GetKey(cc.jumpKey)) {

			float timeRatio = landingAnimationTimer / landingAnimationLength;
			float xLandLerp = Mathf.Lerp(xScaleLanding, 1.0f, timeRatio);
			float yLandLerp = Mathf.Lerp(yScaleLanding, 1.0f, timeRatio);
			transform.localScale = new Vector2(xLandLerp, yLandLerp);

			landingAnimationTimer += Time.deltaTime;
			if (landingAnimationTimer > landingAnimationLength) {
				landingAnimationTimer = 0.0f;
				landingAnimation = false;
			}
		}
		if (!cc.onGround) {
			landingAnimationTimer = 0.0f;
			landingAnimation = false;
			transform.localScale = new Vector2(xLerp, yLerp);
		}

		//Sword swing scaing
		if (ss.currentSword == 1 && ss.normalAtk) {
			transform.localScale = new Vector2(xScaleSwing1, yScaleSwing1);
		} else if (ss.currentSword == 2 && ss.normalAtk) {
			transform.localScale = new Vector2(xScaleSwing2, yScaleSwing2);
		}

		//Standard scaling
		if (!ss.normalAtk && !landingAnimation && !walkingScale) {
			transform.localScale = new Vector2(xLerp, yLerp);
		}

		//Vector2 dir = new Vector2(rb.velocity.x, rb.velocity.y);
		//float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		//transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public void LandingAnimation() {
		landingAnimation = true;
	}

	public void SetMaxYSpeed(float speed) {
		maxYSpeed = speed;
	}

	public void SetMaxXSpeed(float speed) {
		maxXSpeed = speed;
	}
}
