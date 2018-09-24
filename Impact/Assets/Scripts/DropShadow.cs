using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour {

	public Color shadowColor;
	public float layerOrder = 0.0f;

	private Vector2 offset = new Vector2(-0.8f, -0.4f);

	private SpriteRenderer shadowCaster;
	private SpriteRenderer shadow;

	private Transform shadowTransform;
	private Transform casterTransform;

	// Use this for initialization
	void Start () {

		casterTransform = transform;
		shadowTransform = new GameObject().transform;
		shadowTransform.parent = casterTransform;
		shadowTransform.gameObject.name = "DropShadow";
		shadowTransform.localRotation = Quaternion.identity;
		shadowTransform.localScale = Vector3.one;

		shadowCaster = GetComponent<SpriteRenderer>();
		shadow = shadowTransform.gameObject.AddComponent<SpriteRenderer>();
		
		shadow.sortingOrder = shadowCaster.sortingOrder - 1;
		shadow.color = shadowColor;

		shadowTransform.position = new Vector2(casterTransform.position.x + offset.x, casterTransform.position.y + offset.y);
		shadow.sprite = shadowCaster.sprite;
	}
}
