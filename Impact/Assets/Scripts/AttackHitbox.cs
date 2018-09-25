using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour {

	private bool active = false;

	private float timer = 0.0f;
	private float activeTime = 3.0f;

	public void DoAttack(float activeTime){
		this.activeTime = activeTime;
		active = true;
	}

	void Update () {
		
		if(active){
			timer += Time.deltaTime;
			if (timer > activeTime) {
				Destroy(gameObject);
			}
		}
	}
}
