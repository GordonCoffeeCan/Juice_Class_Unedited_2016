using UnityEngine;
using System.Collections;

//This script is used to control the falling blocks and detect collisions
public class BlockControl : MonoBehaviour {
	Vector2 velocity;
	float gravity = -50f;
	float drag_y = 0.15f;
	float skinWidth = 0.1f;
	bool grounded; //set to true if this block lands
	bool freeze; //set to true if the game ends
	Rigidbody2D rb;
	BoxCollider2D col;
	public LayerMask collisionMask; //you can set a custom LayerMask in the inspector to control which layers a raycast hits.

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
		grounded = false; //is this block on the ground?
		freeze = false; //is this block done moving?
		velocity = Vector2.zero;
	}
	
	void FixedUpdate () {

		if ((grounded == false) && (freeze == false)) {

			Vector2 acceleration = new Vector2 (0, 0);
			acceleration.y += gravity; //accelerate with gravity
			acceleration.y += -drag_y * velocity.y * Mathf.Abs (velocity.y); //slow down due to drag if we're moving fast

			velocity += acceleration * Time.fixedDeltaTime; //the velocity increases due to the acceleration

			//Check to see if we're colliding with anything
			UpdateTouches ();

			rb.MovePosition(transform.position + (Vector3)velocity * Time.fixedDeltaTime); //update the position due to the velocity
		} else if (freeze == false){ 
			//snap to an integer position (helps to keep our columns neat)
			rb.MovePosition(new Vector3( Mathf.Round (transform.position.x) , Mathf.Round (transform.position.y)));
		}
	}

	void Update(){
		BroadcastMessage ("UpdateVelocity", velocity,SendMessageOptions.DontRequireReceiver); //let all children know the new velocity
	}

	//This function checks to see if the block should land on the ground.
	void UpdateTouches(){
		int numHits = 0;

		if ( !grounded) { //don't update if this has already landed

			//Send out an imaginary ray toward the ground, starting at the edge of the collider to see if this block will hit anything in the next frame.
			RaycastHit2D hit = Physics2D.Raycast(col.bounds.center+new Vector3(0,-col.bounds.extents.y-skinWidth,0),Vector2.up,velocity.y*Time.fixedDeltaTime,collisionMask);
		
			if (hit.collider){ //we hit an object

				//tell all the children of this object that a collision happened, and the location
				BroadcastMessage ("BeginContact",  hit.point,SendMessageOptions.DontRequireReceiver); 

				//if we're moving down, only move far enough to reach the thing we hit (don't go past it)
				velocity.y = Mathf.Max(velocity.y,-hit.distance - skinWidth)/Time.fixedDeltaTime;

				//if we hit the player, we need to stop and send the message HitPlayer to the block generator, 
				//which will end the game
				if (hit.collider.tag == "Player") {
					transform.parent.gameObject.BroadcastMessage ("HitPlayer");
					freeze = true; //the game ends
				}
				else {
					grounded = true; 
				}
			}
		

		}

		//don't move down if we're on the ground
		if (grounded)
			velocity.y = Mathf.Max (0, velocity.y);

	}
	
}
