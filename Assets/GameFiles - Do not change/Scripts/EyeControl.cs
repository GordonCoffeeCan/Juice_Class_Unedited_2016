using UnityEngine;
using System.Collections;

//This script animates a pair of cartoon eyes on a sprite. If it's the player, it uses the player velocity
//if it's not the player, it looks at the player
public class EyeControl : MonoBehaviour {
	Vector2 velocity;
	Transform playerTransform;
	public Transform leftPupilTransform;
	public Transform rightPupilTransform;
	Vector3 leftPupilStart; //home location for left pupil
	Vector3 rightPupilStart;//home location for right pupil
	float blinkTimer;

	void Start () {
		//position partway up the parent
		transform.localPosition = new Vector3(0,0.25f,0);

		GameObject player = GameObject.Find ("Player"); //get a reference to the player
		if ((player) && (player != transform.parent.gameObject)) //this object isn't the player
			playerTransform = player.transform;
		leftPupilStart = leftPupilTransform.localPosition; //store the home position of the pupils
		rightPupilStart = rightPupilTransform.localPosition;
		blinkTimer = Random.Range (1f, 5f);
	}



	// Update is called once per frame
	void Update () {
		blinkTimer -= Time.deltaTime;
		if (blinkTimer <= 0) {
			transform.localScale = new Vector3 (1, 0, 1); //scale down to blink
			//blink again later
			blinkTimer = Random.Range (1f, 5f);
		} else {
			//scale back up
			transform.localScale = Vector3.Lerp(transform.localScale,Vector3.one,0.2f);
		}

		if (playerTransform) {
			//I am not the player, so follow the player
			Vector3 leftVectorToPlayer = playerTransform.position - leftPupilTransform.position;
			leftPupilTransform.localPosition = leftPupilStart + leftVectorToPlayer.normalized*0.1f;
			Vector3 rightVectorToPlayer = playerTransform.position - rightPupilTransform.position;
			rightPupilTransform.localPosition = rightPupilStart + rightVectorToPlayer.normalized*0.1f;
		} else {
			//I am the player, follow player velocity
			leftPupilTransform.localPosition = leftPupilStart + Vector3.ClampMagnitude(velocity*0.1f, 0.1f);
			rightPupilTransform.localPosition = rightPupilStart +  Vector3.ClampMagnitude(velocity*0.1f, 0.1f);
		}
	}

	//this function gets set by the parent as it moves around
	public void UpdateVelocity(Vector2 newVelocity){
		velocity = newVelocity;
	}

}
