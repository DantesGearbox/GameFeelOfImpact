using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	float timer = 25;
	float time = 0;

	private Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {

		rb.velocity = new Vector2(-3, rb.velocity.y);


		time += Time.deltaTime;
		if(time > timer) {
			Destroy(this.gameObject);
		}
	}
}
