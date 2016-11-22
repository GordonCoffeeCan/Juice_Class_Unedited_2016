using UnityEngine;
using System.Collections;

//This script simply triggers a little sprite animation at the location of a contact
public class ContactClash : MonoBehaviour {
	Animator anim;
	Vector3 playPosition;
	Vector3 baseScale;
	BoxCollider2D col;
	float rayLength = 0.1f;
	public bool clashOnBegin = true;
	public bool clashOnEnd = false;
	public LayerMask collisionMask; //manually choose layers to collide with in the inspector

	void Start () {
		anim = GetComponent<Animator> ();
		col = transform.parent.gameObject.GetComponent<BoxCollider2D> ();

	}

	void Update(){
		transform.position= playPosition; //we need to make sure this sprite doesn't get moved by its parent transform
	}

	void BeginContact (Vector2 point) {

		if (!clashOnBegin)
			return;
			

		//send an imaginary ray in each direction to see if we're touching a surface
		RaycastHit2D rayhitLeft = Physics2D.Raycast (point, new Vector2 (-1, 0), rayLength, collisionMask);
		RaycastHit2D rayhitRight = Physics2D.Raycast (point, new Vector2 (1, 0), rayLength, collisionMask);
		RaycastHit2D rayhitDown = Physics2D.Raycast (point, new Vector2 (0, -1), rayLength, collisionMask);
		RaycastHit2D rayhitUp = Physics2D.Raycast (point, new Vector2 (0, 1), rayLength, collisionMask);
		
		if (rayhitLeft.collider) { //hit something to the left - so shoot out particles up and down
			playPosition = rayhitLeft.point; //this is the position of the contact
			transform.position = playPosition; //move to the contact position
			transform.localEulerAngles = new Vector3 (0, 0, -90f);
			anim.SetTrigger ("Clash"); //start the animation
		}
		if (rayhitRight.collider) {
			playPosition = rayhitRight.point; //this is the position of the contact
			transform.position = playPosition; //move to the contact position
			transform.localEulerAngles = new Vector3 (0, 0, 90f);
			anim.SetTrigger ("Clash"); //start the animation
		}
		if (rayhitUp.collider) {//hit something above this object - so shoot out particles left and right
			playPosition = rayhitUp.point; //this is the position of the contact
			transform.position = playPosition; //move to the contact position
			transform.localEulerAngles = new Vector3 (0, 0, 180f);
			anim.SetTrigger ("Clash"); //start the animation
		}
		if (rayhitDown.collider) {
			playPosition = rayhitDown.point; //this is the position of the contact
			transform.position = playPosition; //move to the contact position
			transform.localEulerAngles = new Vector3 (0, 0, 0f);
			anim.SetTrigger ("Clash"); //start the animation
		}
	}

	void EndContact(Vector2 point){

		if (!clashOnEnd)
			return;

		if (!col)
			return; // we need the collider reference to position the rays


		//send an imaginary ray in each direction to see if we're touching a surface
		//we won't reliably get a point for contacts that are ending, so we use the center instead
		float totalRayLengthX = rayLength + col.bounds.extents.x;
		float totalRayLengthY = rayLength + col.bounds.extents.y;

		//this is gross and scary, but all I'm doing is making a new collision mask that excludes the layer this object is on,
		//so that the ray won't collide with the object it's coming from. It's important in this EndContact case because the ray is starting inside
		//a collider
		LayerMask collisionMaskMinusThis = collisionMask & ~(1<<transform.parent.gameObject.layer);
		RaycastHit2D rayhitLeft = Physics2D.Raycast (transform.parent.position, new Vector2 (-1, 0), totalRayLengthX, collisionMaskMinusThis);
		RaycastHit2D rayhitRight = Physics2D.Raycast (transform.parent.position, new Vector2 (1, 0), totalRayLengthX, collisionMaskMinusThis);
		RaycastHit2D rayhitDown = Physics2D.Raycast (transform.parent.position, new Vector2 (0, -1), totalRayLengthY, collisionMaskMinusThis);
		RaycastHit2D rayhitUp = Physics2D.Raycast (transform.parent.position, new Vector2 (0, 1), totalRayLengthY, collisionMaskMinusThis);
		
		if (rayhitLeft.collider) { //hit something to the left - so shoot out particles up and down
			playPosition = rayhitLeft.point; //this is the position of the contact
			transform.position = playPosition; //move to the contact position
			transform.localEulerAngles = new Vector3 (0, 0, -90f);
			anim.SetTrigger ("Clash"); //start the animation
		}
		if (rayhitRight.collider) {
			playPosition = rayhitRight.point; //this is the position of the contact
			transform.position = playPosition; //move to the contact position
			transform.localEulerAngles = new Vector3 (0, 0, 90f);
			anim.SetTrigger ("Clash"); //start the animation
		}
		if (rayhitUp.collider) {//hit something above this object - so shoot out particles left and right
			playPosition = rayhitUp.point; //this is the position of the contact
			transform.position = playPosition; //move to the contact position
			transform.localEulerAngles = new Vector3 (0, 0, 180f);
			anim.SetTrigger ("Clash"); //start the animation
		}
		if (rayhitDown.collider) {
			playPosition = rayhitDown.point; //this is the position of the contact
			transform.position = playPosition; //move to the contact position
			transform.localEulerAngles = new Vector3 (0, 0, 0f);
			anim.SetTrigger ("Clash"); //start the animation
		}
	}

	void DoClash(Vector2 point){

	}
}
