using UnityEngine;
using System.Collections;

//This script switches out the base sprite based on which key is being pressed.
public class KeyboardSpriteChange : MonoBehaviour {
	
	public Sprite leftSprite;
	public Sprite rightSprite;
	public Sprite jumpSprite;
	public Sprite idleSprite;

	SpriteRenderer baseSpriteRenderer;

	// Use this for initialization
	void Start () {
		foreach (Transform t in transform.parent) { //iterate over the siblings of this object
			if (t.name == "Sprite"){
				baseSpriteRenderer = t.gameObject.GetComponent<SpriteRenderer>();
			}

		}
	}
	
	void Update () {
		if (!baseSpriteRenderer)
			return; //we should abort if baseSpriteRenderer isn't set

		if (Input.GetButton ("Jump")) {
			baseSpriteRenderer.sprite = jumpSprite;
		} else if (Input.GetAxis ("Horizontal") < 0) {
			baseSpriteRenderer.sprite = leftSprite;
		} else if (Input.GetAxis ("Horizontal") > 0) {
			baseSpriteRenderer.sprite = rightSprite;
		} else {
			baseSpriteRenderer.sprite = idleSprite;
		}

	}
}
