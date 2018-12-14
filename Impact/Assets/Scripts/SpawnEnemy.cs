using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

	public GameObject enemyObject;
	private AudioManager audioManager;

	private void Start() {
		audioManager = FindObjectOfType<AudioManager>();
	}

	//Spawn an enemy
	public void SpawnAnEnemy() {
		GameObject go = Instantiate(enemyObject);
		go.GetComponent<EnemyBehavior>().GetComponent<Rigidbody2D>().velocity += new Vector2(0, 20);

		//SFX
		audioManager.PlayWithRandomPitch("PortalSpawn", 0.5f);
	}
}