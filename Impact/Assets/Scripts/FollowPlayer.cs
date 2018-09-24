using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public Transform player;
		
	void FixedUpdate () {
		transform.position = Vector3.Lerp (transform.position, new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z), 0.15f);
	}
}
