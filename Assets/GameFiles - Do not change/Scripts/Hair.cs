using UnityEngine;
using System.Collections;

//This script sets the 'hair' sprites to get crushed and regrow when they are touched

public class Hair : MonoBehaviour {
	public Transform leftHair;
	public Transform rightHair;
	public Transform upHair;
	public Transform downHair;
	public float regrowTime = 1.0f; //hair will 'regrow' in this amount of time

	//timer variables
	float upTime;
	float downTime;
	float leftTime;
	float rightTime;

	void Start () {
		//initialize timer variables
		upTime = regrowTime;
		downTime = regrowTime;
		leftTime = regrowTime;
		rightTime = regrowTime;

		//find the edges of the sprite and put the hair there
		SpriteRenderer rootSprite=null;
		foreach (SpriteRenderer sp in transform.parent.GetComponentsInChildren<SpriteRenderer>()) {//consider parent's children
			if (sp.transform.parent != transform) { //ignore my own children (the grass sprites)
				rootSprite=sp;
			}
		}
		if (rootSprite!=null) {
			Vector3 spriteExtents = rootSprite.bounds.extents;
			leftHair.localPosition = new Vector3 (-spriteExtents.x, 0, 0);
			rightHair.localPosition = new Vector3 (spriteExtents.x, 0, 0);
			downHair.localPosition = new Vector3 (0, -spriteExtents.y, 0);
			upHair.localPosition = new Vector3 (0, spriteExtents.y, 0);
		}
	}	

	void Update () {
		if (upTime < regrowTime) {
			float yScale = Mathf.Lerp(0.25f,1.0f,upTime/regrowTime);
			if (upHair.localScale.y < 1f) upHair.localScale = new Vector3(1f,yScale,1f);
			upTime += Time.deltaTime;
		}
		if (downTime < regrowTime) {
			float yScale = Mathf.Lerp(0.25f,1.0f,downTime/regrowTime);
			if (downHair.localScale.y < 1f) downHair.localScale = new Vector3(1f,yScale,1f);
			downTime += Time.deltaTime;
		}
		if (leftTime < regrowTime) {
			float yScale = Mathf.Lerp(0.25f,1.0f,leftTime/regrowTime);
			if (leftHair.localScale.y < 1f) leftHair.localScale = new Vector3(1f,yScale,1f);
			leftTime += Time.deltaTime;
		}
		if (rightTime < regrowTime) {
			float yScale = Mathf.Lerp(0.25f,1.0f,rightTime/regrowTime);
			if (rightHair.localScale.y < 1f) rightHair.localScale = new Vector3(1f,yScale,1f);
			rightTime += Time.deltaTime;
		}
	}

	public void BeginContact(Vector2 point){
		//get the x and y distance to the contact, so we can determine where it came from
		float xDistance = (point.x - transform.position.x)/transform.lossyScale.x;
		float yDistance = (point.y - transform.position.y)/transform.lossyScale.y;

		if (Mathf.Abs (xDistance) > Mathf.Abs (yDistance)) { //must be a contact to the left or right
			if ((xDistance > 0) && (rightHair)) { //crush the right hair
				rightHair.localScale = new Vector3(1f,0.25f,1f); //crush
				rightTime = 0; //reset timer
			} else if ((xDistance < 0) && (leftHair)) { //crush the left hair
				leftHair.localScale = new Vector3(1f,0.25f,1f);
				leftTime = 0;
			}
		} else { //must be a contact above or below
			if ((yDistance > 0) && (upHair)) { //crush the upper hair
				upHair.localScale = new Vector3(1f,0.25f,1f);
				upTime = 0;
			} else if ((yDistance < 0) && (downHair)) { //crush the lower hair
				downHair.localScale = new Vector3(1f,0.25f,1f);
				downTime = 0;
			}
		}


	}


}
