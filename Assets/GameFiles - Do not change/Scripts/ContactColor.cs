using UnityEngine;
using System.Collections;

//This script changes the color of the sprite according to a gradient, when something touches it.
public class ContactColor : MonoBehaviour {
	SpriteRenderer sprite;
	float time; //timer variable for tweening colors

	//Did you know you can set the inspector to use a limited range rather than an unlimited float?
	[Range(0, 4)]
	public float changeTime = 1.0f;

	//Did you know you can get colors from an editable gradient?
	public Gradient colorGradient;
	
	public bool changeColorOnGameOver = true; //should the game do this effect when a block hits the player?

	void Start () {

		//find the parent's sprite
		foreach (Transform t in transform.parent){
			if (t.name == "Sprite") sprite = t.gameObject.GetComponent<SpriteRenderer>();
		}
		time = changeTime;
	}
	
	void Update () {
		if (sprite) { 
			if (time < changeTime) {
				sprite.color = colorGradient.Evaluate (time / changeTime); //as time increases, we move along the gradient
				time += Time.deltaTime;
			} else {
				sprite.color = colorGradient.Evaluate (1); //get the color from the far right of the gradient
			}
		}
	}

	public void BeginContact(Vector2 point){
		time = 0; //this starts the timer
	}

	public void Die(){
		if (changeColorOnGameOver) time = 0; //this starts the timer
	}
}
