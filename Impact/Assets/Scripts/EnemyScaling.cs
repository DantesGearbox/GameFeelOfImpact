using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScaling : MonoBehaviour {

	private Rigidbody2D rb;
	private EnemyBehavior eb;

	private bool landingAnimation = false;
	private float landingAnimationLength = 0.1f;
	private float landingAnimationTimer = 0.0f;
	private float xScaleLanding = 1.5f;
	private float yScaleLanding = 0.75f;

	private float defScaleX = 1.3f;
	private float defScaleY = 0.5f;

	float timer = 0;
	float cosTime = 0;

	private GameFeelManager gfm;

	private void Start() {
		rb = GetComponentInParent<Rigidbody2D>();
		eb = GetComponentInParent<EnemyBehavior>();
		gfm = FindObjectOfType<GameFeelManager>();
	}

	void FixedUpdate() {

		//Landing scaling
		if (landingAnimation && eb.onGround) {

			float timeRatio = landingAnimationTimer / landingAnimationLength;
			float xLandLerp = Mathf.Lerp(xScaleLanding, defScaleX, timeRatio);
			float yLandLerp = Mathf.Lerp(yScaleLanding, defScaleY, timeRatio);
			transform.localScale = new Vector2(xLandLerp, yLandLerp);

			landingAnimationTimer += Time.deltaTime;
			if (landingAnimationTimer > landingAnimationLength) {
				landingAnimationTimer = 0.0f;
				landingAnimation = false;
			}
		}
		if (!eb.onGround) {
			landingAnimationTimer = 0.0f;
			landingAnimation = false;
			transform.localScale = new Vector2(defScaleX, defScaleY);
		}

		//Standard scaling
		if (!landingAnimation) {
			timer += Time.deltaTime;
			cosTime = Mathf.Cos(timer);

			transform.localScale = new Vector2(defScaleX, defScaleY + Mathf.Cos(cosTime));
		}

		if (gfm.disableAnimations) {
			transform.localScale = new Vector2(1.3f, 1);
		}
	}

	public void LandingAnimation() {
		landingAnimation = true;
	}
}
