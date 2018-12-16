using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

	public GameObject enemyObject;
	public GameObject toughEnemy;
	private AudioManager audioManager;
	public Transform spawn;
	private GameFeelManager gfm;

	private void Start() {
		audioManager = FindObjectOfType<AudioManager>();
		gfm = FindObjectOfType<GameFeelManager>();
	}

	//Spawn an enemy
	public void SpawnAnEnemy() {
		GameObject go = Instantiate(enemyObject, spawn.position, Quaternion.identity);
		go.GetComponent<EnemyBehavior>().GetComponent<Rigidbody2D>().velocity += new Vector2(-5, 20);

		if (!gfm.disableSoundEffects) {
			//SFX
			audioManager.PlayWithRandomPitch("PortalSpawn", 0.5f);
		}
	}

	//Spawn an enemy
	public void SpawnToughEnemy() {
		GameObject go = Instantiate(toughEnemy, spawn.position, Quaternion.identity);
		go.GetComponent<EnemyBehavior>().GetComponent<Rigidbody2D>().velocity += new Vector2(-5, 20);

		if (!gfm.disableSoundEffects) {
			//SFX
			audioManager.PlayWithRandomPitch("PortalSpawn", 0.5f);
		}
	}
}