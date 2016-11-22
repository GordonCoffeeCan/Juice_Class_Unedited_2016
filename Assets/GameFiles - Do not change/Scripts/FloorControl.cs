using UnityEngine;
using System.Collections;

//This script handles collisions for the floor blocks.

public class FloorControl : MonoBehaviour {
	BoxCollider2D col;
	public LayerMask collisionMask; //you can use a LayerMask object to filter out layers when raycasting
	float rayLength= 0.2f; //maximum distance to check if an object is touching this one

	//these boolean variables will be set if we are touching anything
	bool touchingUp;
	bool touchingDown;
	bool touchingLeft;
	bool touchingRight;

	void Start () {
		col = GetComponent<BoxCollider2D> ();
		touchingLeft = false;
		touchingRight = false;
		touchingUp = false;
		touchingDown = false;
	}
	
	void FixedUpdate () {
		// send an imaginary box in each direction to see if it touches another collider
		RaycastHit2D rayhitLeft = Physics2D.BoxCast (col.bounds.center, col.bounds.size, 0, Vector2.right,rayLength, collisionMask);
		RaycastHit2D rayhitRight = Physics2D.BoxCast (col.bounds.center, col.bounds.size, 0, Vector2.right, rayLength, collisionMask);
		RaycastHit2D rayhitDown = Physics2D.BoxCast (col.bounds.center, col.bounds.size, 0, Vector2.up, rayLength, collisionMask);
		RaycastHit2D rayhitUp = Physics2D.BoxCast (col.bounds.center, col.bounds.size, 0, Vector2.up, rayLength, collisionMask);

		if (rayhitRight.collider) { //something is to our right
			if (!touchingRight) {
				BroadcastMessage ("BeginContact", rayhitRight.point,SendMessageOptions.DontRequireReceiver); //tell any children that this contact happened
			}
			touchingRight = true;
		}
		else {
			if (touchingRight) BroadcastMessage ("EndContact",  rayhitRight.point,SendMessageOptions.DontRequireReceiver );//tell any children that this contact ended
			touchingRight = false;
		}
		if (rayhitLeft.collider){//something is to our left
			
			if (!touchingLeft){
				BroadcastMessage ("BeginContact",  rayhitLeft.point,SendMessageOptions.DontRequireReceiver);//tell any children that this contact happened
			}
			touchingLeft = true; 
			
		}
		else {
			if (touchingLeft) BroadcastMessage ("EndContact",  rayhitLeft.point,SendMessageOptions.DontRequireReceiver);//tell any children that this contact ended
			touchingLeft = false;
		}
		if (rayhitUp.collider) {//something is above
			if (!touchingUp) {
				BroadcastMessage ("BeginContact",  rayhitUp.point,SendMessageOptions.DontRequireReceiver);//tell any children that this contact happened
			}
			touchingUp = true; 
		}
		else {
			if (touchingUp) BroadcastMessage ("EndContact",  rayhitUp.point,SendMessageOptions.DontRequireReceiver);//tell any children that this contact ended
			touchingUp = false;
		}
		if (rayhitDown.collider) { //something is below
			
			if (!touchingDown){
				BroadcastMessage ("BeginContact",  rayhitDown.point,SendMessageOptions.DontRequireReceiver);//tell any children that this contact happened
			}
			touchingDown = true; 
		}
		else {
			if (touchingDown) BroadcastMessage ("EndContact",  rayhitDown.point,SendMessageOptions.DontRequireReceiver);//tell any children that this contact ended
			touchingDown = false;
		}
	}
}
