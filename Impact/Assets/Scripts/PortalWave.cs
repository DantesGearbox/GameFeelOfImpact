using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalWave : MonoBehaviour {

	float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer = Mathf.Cos(Time.time);
		timer /= 1.0f;

		transform.localScale = new Vector2( 1 + Mathf.Cos(timer), 1 + Mathf.Cos(timer) );
	}
}
