using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	private static float trauma = 0.0f;
	private float traumaRemoval = 1.0f; //Per second
	private float maxOffset = 1.0f;
	private Vector3 notShakenCam;

	private void Start() {
		notShakenCam = transform.position;
	}

	public static void AddTrauma(float addedTrauma) {
		
		trauma += Mathf.Clamp01(addedTrauma);
	}

	void FixedUpdate() {
		//Camera shake effect
		trauma = Mathf.Clamp01(trauma);
		float xOffset = maxOffset * Mathf.Pow(trauma, 2) * Random.Range(-1.0f, 1.0f);
		float yOffset = maxOffset * Mathf.Pow(trauma, 2) * Random.Range(-1.0f, 1.0f);
		trauma -= traumaRemoval * Time.deltaTime;

		//Apply camera shake and follow effects
		transform.position = notShakenCam + new Vector3(xOffset, yOffset, 0);
	}
}
