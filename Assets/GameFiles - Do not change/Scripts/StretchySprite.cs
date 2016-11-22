using UnityEngine;
using System.Collections;

//This script stretches its parent object's sprite if it's moving, to accentuate the effect of motion.
//optionally, it also squishes the sprite if it gets touched.

public class StretchySprite : MonoBehaviour {
	public bool stretchOnMove = false;
	public bool stretchOnVerticalTouch = true;
	public bool stretchOnHorizontallTouch = true;
	public float maxStretchTime = 1.0f; 

	Vector2 velocity;
	public AnimationCurve stretchCurve; //for squishing, we can set a curve in the inspector to animate motion using a timer
	float stretchTimer;
	string stretchDirection;

	Transform spriteTransform;

	Vector3 startingScale;

	void Start () {
		stretchTimer = 0;

		//find the sprite on the parent
		foreach (Transform t in transform.parent){
			if (t.name == "Sprite") spriteTransform = t;
		}

		startingScale = spriteTransform.localScale;
	}
	
	void Update () {

		spriteTransform.localScale = startingScale; //reset to 1.0 scale
		spriteTransform.localPosition =  Vector3.zero; //reset to parent position

		//if we are supposed to stretch on move just alter the scale according to the velocity, easy
		if (stretchOnMove) spriteTransform.localScale += new Vector3(Mathf.Abs (velocity.x)*0.05f , Mathf.Abs (velocity.y)*0.05f , 0);

		//if we're supposed to squish on touch...
		if ((stretchOnVerticalTouch)||(stretchOnHorizontallTouch)){
			//...and if the timer has been set by a contact
			if (stretchTimer > 0f){
				//use the curve from the inspector to get the amount we should scale by (using the timer)
				float scaleAmount = stretchCurve.Evaluate(1.0f-(stretchTimer/maxStretchTime));

				if (stretchOnVerticalTouch){
					if (stretchDirection == "Vertical"){ //hit from above
						//we don't want to scale around the middle, we want it to scale from one side, so there is going to be an offset to the position
						spriteTransform.localPosition = new Vector3(0,scaleAmount * 0.5f,0);
						spriteTransform.localScale += new Vector3( 0,scaleAmount,0);
					}
				}

				if (stretchOnHorizontallTouch){

					if (stretchDirection == "Right") {
						//we don't want to scale around the middle, we want it to scale from one side, so there is going to be an offset
						spriteTransform.localPosition = new Vector3(-scaleAmount * 0.5f, 0,0);
						spriteTransform.localScale += new Vector3( scaleAmount,0,0);
					}
					else if (stretchDirection == "Left") {
						//we don't want to scale around the middle, we want it to scale from one side, so there is going to be an offset
						spriteTransform.localPosition = new Vector3(scaleAmount * 0.5f, 0,0);
						spriteTransform.localScale += new Vector3( scaleAmount,0,0);
					}
				}
				stretchTimer -= Time.deltaTime; //decrement the timer
			}
		}

	}


	//This function is called by the parent 
	public void UpdateVelocity(Vector2 newVelocity){
		velocity = newVelocity;
	}

	//This function is called by the parent 
	public void BeginContact(Vector2 point){

		if (stretchTimer > 0)
			return; //don't overwrite previous triggered touch

		stretchTimer = maxStretchTime; //set timer for tween

		Vector2 vectorToPoint = point - (Vector2)spriteTransform.position;
		if (Mathf.Abs (vectorToPoint.x) > Mathf.Abs (vectorToPoint.y)) {
			//a side collision
			if (vectorToPoint.x > 0)
				stretchDirection = "Right";
			else 
				stretchDirection = "Left";
		} else {
			//a top/bottom collision
			stretchDirection = "Vertical";
		}
	}


}
