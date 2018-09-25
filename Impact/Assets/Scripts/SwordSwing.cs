using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour {

	public KeyCode normalAttackKey;

	private float normalAtkTimer = 0.0f;
	private float normalAtkLength = 0.1f;
	public bool normalAtk = false;

	public bool isAttacking = false;
	private CharacterController2D cc;

	private bool swordEnabled = false;

	public SpriteRenderer swordSprite;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController2D>();
	}
	
	// Update is called once per frame
	void Update () {

		NormalAttack();

	}

	void NormalAttack() {

		//First, check the general "isAttacking" to see if any atk is going
		if (!isAttacking) {

			//Check if we should do a normal atk
			if (Input.GetKey(normalAttackKey)) {
				normalAtk = true;
				isAttacking = true;

				swordSprite.enabled = true;
			}
		}

		//All logic that has to do with the normal atk
		if (normalAtk) {
			normalAtkTimer += Time.deltaTime;
			if (normalAtkTimer > normalAtkLength) {
				normalAtkTimer = 0.0f;
				normalAtk = false;
				isAttacking = false;

				swordSprite.enabled = false;
			}
		}
	}
}
