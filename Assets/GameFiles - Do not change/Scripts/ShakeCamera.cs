using UnityEngine;
using System.Collections;

//This script simply shakes the camera for a set amount of time by randomizing its position
public class ShakeCamera : MonoBehaviour {
	public bool shakeOnDeath = false;
	public bool shakeOnContact = true;

	//using a public animationcurve to control the shake (set it in the inspector)
	public AnimationCurve shakeCurve; 
	public float shakeTime = 1.0f;
	float timer;

	void Start () {
		timer = shakeTime;
	}
	
	// LateUpdate happens after Update is done on all components
	void LateUpdate () { 
		if (timer < shakeTime) {
			float shakeAmt = shakeCurve.Evaluate (timer); //use the curve to animate the shake amount
			//randomize the position, using the shake amount 
			Camera.main.transform.localPosition += new Vector3 (Random.Range (-shakeAmt, shakeAmt), Random.Range (-shakeAmt, shakeAmt),0);
			timer += Time.deltaTime; //increment the timer
		}
	}

	public void BeginContact(Vector2 point){
		if (shakeOnContact){
			if (timer >= shakeTime){ //don't shake if a shake is currently happening
				timer = 0; //reset the timer
			}
		}
	}

	public void Die(){
		if (shakeOnDeath){
			if (timer >= shakeTime){ //don't shake if a shake is currently happening
				timer = 0; //reset the timer
			}
		}
	}
}
