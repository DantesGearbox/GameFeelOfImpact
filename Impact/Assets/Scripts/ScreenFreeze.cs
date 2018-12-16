using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFreeze : MonoBehaviour {

	private float duration = 0.1f;
	bool isFrozen = false;

	float pendingFreezeDuration = 0.0f;

	private GameFeelManager gfm;

	private void Awake() {
		gfm = FindObjectOfType<GameFeelManager>();
	}

	// Update is called once per frame
	void Update() {
		if (pendingFreezeDuration > 0 && !isFrozen) {
			if (!gfm.disableScreenFreeze) {
				StartCoroutine(DoFreeze());
			} else {
				pendingFreezeDuration = 0;
			}
		}
	}

	public void Freeze() {
		pendingFreezeDuration = duration;
	}

	public void FreezeForTime(float time) {
		pendingFreezeDuration = time;
		duration = time;
	}

	public void FreezeForHitPower(float hitPower) {

		float freezeTime = (hitPower - 10) / 400;

		Debug.Log("FreezeTime: " + freezeTime);

		pendingFreezeDuration = freezeTime;
		duration = freezeTime;
	}

	IEnumerator DoFreeze() {
		isFrozen = true;
		float original = Time.timeScale;
		Time.timeScale = 0.0f;

		yield return new WaitForSecondsRealtime(duration);

		Time.timeScale = original;
		pendingFreezeDuration = 0.0f;
		isFrozen = false;
	}
}
