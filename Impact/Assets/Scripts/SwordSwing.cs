using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour {

	public KeyCode normalAttackKey;

	private float normalAtkTimer = 0.0f;
	private float normalAtkLength = 0.125f;
	public bool normalAtk = false;

	private float attackCooldownTimer = 0.0f;
	private float attackCooldownLength = 0.15f;
	private bool attackCooldown = false;

	public bool isAttacking = false;
	private CharacterController2D cc;
	private AudioManager audioManager;
	private ScreenFreeze sf;

	public SpriteRenderer swordSprite;
	public Sprite sword1;
	public Sprite sword2;
	public int currentSword = 1;

	public BoxCollider2D normalAttackHitbox;
	public float hitForce = 10.0f;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController2D>();
		audioManager = FindObjectOfType<AudioManager>();
		sf = GetComponent<ScreenFreeze>();
	}
	
	// Update is called once per frame
	void Update () {

		if (attackCooldown) {
			attackCooldownTimer += Time.deltaTime;
			if(attackCooldownTimer > attackCooldownLength) {
				attackCooldown = false;
				attackCooldownTimer = 0.0f;
			}
		}

		if (!attackCooldown) {
			NormalAttack();
		}

	}

	private void OnTriggerEnter2D(Collider2D collision) {
		
		if(collision.CompareTag("PunchingBag") && normalAtk){
			sf.FreezeForHitPower(hitForce);
			collision.GetComponentInChildren<ScaleWithXAndY>().GotHit(hitForce, transform.position);
		}

		if (collision.CompareTag("Enemy") && normalAtk) {
			sf.FreezeForHitPower(hitForce);
			collision.GetComponentInChildren<EnemyBehavior>().GotHit(hitForce, transform.position);
		}

		if (collision.CompareTag("ToughEnemy") && normalAtk) {
			sf.FreezeForHitPower(hitForce*2);
			collision.GetComponentInChildren<EnemyBehavior>().GotHit(hitForce, transform.position);

			cc.blockStun = true;
		}
	}

	void NormalAttack() {

		//First, check the general "isAttacking" to see if any atk is going
		if (!isAttacking && !attackCooldown && !cc.blockStun) {

			//Check if we should do a normal atk
			if (Input.GetKey(normalAttackKey)) {
				normalAtk = true;
				isAttacking = true;

				swordSprite.enabled = true;
				normalAttackHitbox.enabled = true;

				//SFX
				audioManager.PlayWithRandomPitch("SwordHit", 1.5f);
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
				normalAttackHitbox.enabled = false;

				//Switch the two sword sprites up
				if(currentSword == 1) {
					currentSword = 2;
					swordSprite.sprite = sword2;
				} else if (currentSword == 2) {
					currentSword = 1;
					swordSprite.sprite = sword1;
				}

				//Set attacking on a general cooldown
				attackCooldown = true;
			}
		}
	}
}
