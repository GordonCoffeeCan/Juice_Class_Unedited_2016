using UnityEngine;
using System.Collections;

//This script animates the background color when the parent object touches something
//It works the same as ContactColor so look at that if you need comments
public class ContactBackgroundFlash : MonoBehaviour {
	SpriteRenderer backgroundRenderer;
	float time;

	[Range(0, 4)]
	public float changeTime = 1.0f;
	public Gradient colorGradient;
	public bool changeColorOnGameOver = true;

	// Use this for initialization
	void Start () {
		time = changeTime; //don't want to change color at the beginning of the game.
		GameObject background = GameObject.Find ("Backdrop");
		if (background) {
			backgroundRenderer = background.GetComponent<SpriteRenderer> ();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (backgroundRenderer) {
			if (time < changeTime) {
				backgroundRenderer.color = colorGradient.Evaluate (time / changeTime);
				time += Time.deltaTime;
			} else {
				backgroundRenderer.color = colorGradient.Evaluate (1);
			}
		}
	}

	public void BeginContact(Vector2 point){
		time = 0;
	}

	public void Die(){
		if (changeColorOnGameOver) time = 0;
	}
}
