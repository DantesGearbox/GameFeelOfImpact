using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour {

	public KeyCode normalAttackKey;
	public AnimationClip normalAttackAnim;

	private float normalAtkTimer = 0.0f;
	private float normalAtkLength = 0.0f;
	public bool normalAtk = false;

	public GameObject attackHitbox;
	public bool isAttacking = false;
	private CharacterController2D cc;

	//public GameObject attackPrefab;
	//public bool isAttacking = false;

	//private float startupTimer = 0.0f;
	//private float startupTime = 10 * (1 / 60.0f);
	//public bool atkStartUp = false;

	//private float activeTimer = 0.0f;
	//private float activeTime = 3 * (1 / 60.0f);
	//public bool atkActive = false;

	//private float recoveryTimer = 0.0f;
	//private float recoveryTime = 25 * (1 / 60.0f);
	//public bool atkRecovery = false;

	//private float hitStunTimer = 0.0f;
	//private float hitStunTime = 30 * (1 / 60.0f);
	//public bool hitStun = false;


	// Use this for initialization
	
	void Start () {
		cc = GetComponent<CharacterController2D>();
		normalAtkLength = normalAttackAnim.length;
	}
	
	// Update is called once per frame
	void Update () {
		
		NormalAttack();
		
	}


	void NormalAttack() {

		//First, check the general "isAttacking" to see if any atk is going
		if (!isAttacking) {
			
			//Check if we should do a normal atk
			if (Input.GetKey(normalAttackKey) && cc.onGround) {
				normalAtk = true;
				isAttacking = true;

				//Instantiate the normalAtk hitbox
				GameObject attackObject = Instantiate(attackHitbox, transform.position, transform.rotation) as GameObject;
				attackObject.GetComponent<AttackHitbox>().DoAttack(normalAtkLength);
				attackObject.transform.SetParent(this.gameObject.transform);
			}
		}

		//All logic that has to do with the normal atk
		if (normalAtk) {
			normalAtkTimer += Time.deltaTime;
			if (normalAtkTimer > normalAtkLength) {
				normalAtkTimer = 0.0f;
				normalAtk = false;
				isAttacking = false;
			}			
		}

	}

	//void HitStun() {

	//	if (hitStun) {

	//		hitStunTimer += Time.deltaTime;
	//		if (hitStunTimer > hitStunTime) {
	//			hitStunTimer = 0.0f;
	//			hitStun = false;
	//		}
	//	}
	//}

	//void NormalAttack() {

	//	if (!isAttacking && !hitStun) {
	//		if (Input.GetKey(normalAttackKey)) {
	//			isAttacking = true;
	//			atkStartUp = true;
	//		}
	//	}


	//	if (isAttacking) {
	//		if (atkStartUp) {

	//			startupTimer += Time.deltaTime;
	//			if (startupTimer > startupTime) {
	//				startupTimer = 0.0f;
	//				atkStartUp = false;
	//				atkActive = true;

	//				GameObject attackObject = Instantiate(attackPrefab, transform.position, transform.rotation) as GameObject;
	//				attackObject.GetComponent<NormalAttackHitbox>().DoAttack(activeTime, hitStunTime);
	//				attackObject.transform.SetParent(this.gameObject.transform);
	//			}

	//		}

	//		if (atkActive) {

	//			activeTimer += Time.deltaTime;
	//			if (activeTimer > activeTime) {
	//				activeTimer = 0.0f;
	//				atkActive = false;
	//				atkRecovery = true;
	//			}

	//		}

	//		if (atkRecovery) {

	//			recoveryTimer += Time.deltaTime;
	//			if (recoveryTimer > recoveryTime) {
	//				recoveryTimer = 0.0f;
	//				atkRecovery = false;
	//				isAttacking = false;
	//			}

	//		}
	//	}
	//}

}
