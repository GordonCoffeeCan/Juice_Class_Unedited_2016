using UnityEngine;
using System.Collections;

//This script uses Time.timeScale to freeze the action momentarily when something is touched
public class ContactHitStun : MonoBehaviour {
	public float stunTime = 0.3f;
	float time;
	public bool stunOnGameOver = true;
	public bool stunOnContact = true;
	bool hasBeenStunned;

	void Start(){
		hasBeenStunned = false;
		time = stunTime;
	}

	void Update () {	
		if ((Time.timeScale <0.1f)&&(hasBeenStunned == false)) {
			time += 0.016666f;
			if (time >= stunTime){
				Time.timeScale = 1f;
				hasBeenStunned = true;//only let it do this once
			}
		}
	}
	public void BeginContact(Vector2 point){
		if (stunOnContact) {
			time = 0; //reset the timer variable
			Time.timeScale = 0.01f; //slow time down (not to 0 - that would prevent FixedUpdate from running)
		}
	}
	
	public void Die(){
		if (stunOnGameOver) {
			time = 0; //reset the timer variable
			Time.timeScale = 0.01f; //slow time down (not to 0 - that would prevent FixedUpdate from running)
		}
	}
}
