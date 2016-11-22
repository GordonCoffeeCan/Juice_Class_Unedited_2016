using UnityEngine;
using System.Collections;

//This script controls the player movement. We use a simple fake physics system for platforming,
//and use the raycasting system to detect collisions.

public class PlayerControl : MonoBehaviour {

	Vector2 velocity;
	float gravity = -50f;
	float walkdrag = 4.95f;
	float airdrag = 0.20f;
	float walkspeed = 280f; //walks speed
	float airspeed = 10f; //air control speed
	float jumpspeed = 5000f; 
	float skinWidth = 0.1f; //this prevents raycasts from hitting the wrong objects
	float jumpBuffer = 0.1f; //this allows the player to press jump just before touching the ground

	Rigidbody2D rb;
	BoxCollider2D col;

	//these variables store information about if we're touching other objects
	bool touchingUp;
	bool touchingDown;
	bool touchingLeft;
	bool touchingRight;

	//keep track of how long since the jump was pressed, for buffering
	float timeSinceJumpPressed;

	//use a public Layermask object so we can control which layers this player can collide with in the inspector
	public LayerMask collisionMask;

	//keep a record of where the player started so we can restart there
	Vector3 startingPos;
	
	bool dead;

	void Start () {
		//some of the colliders are set as triggers, and we want the rays to hit them
		//(you can also set this property in the physics2d settings)
		Physics2D.queriesHitTriggers = true; 

		startingPos = transform.position; //store for later

		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
		Reset ();
	}
	
	// This is our fake 'physics' so it makes sense for it to be in FixedUpdate
	void FixedUpdate () { 
		if (dead) return; //don't do any movement if the player is dead, abort this function

		Vector2 acceleration = new Vector2 (0, 0); //start from zero every frame
		acceleration.y += gravity; //add gravity

		timeSinceJumpPressed += Time.fixedDeltaTime; //how much time since we pressed jump?
		if (Input.GetButtonDown ("Jump")) {
			timeSinceJumpPressed = 0f; //set to zero if we just pressed it
		}

		//jump if we recently touched the jump button and if we are touching the ground
		//(the player can hit jump a bit before touching the ground)
		if ((timeSinceJumpPressed<jumpBuffer)&&(!touchingUp)) { 
			if (touchingLeft) { //wall jump
				acceleration.x += jumpspeed * 0.3f;
				acceleration.y += jumpspeed * 0.3f;
				touchingLeft = false; 
				touchingDown = false;

				timeSinceJumpPressed = jumpBuffer;//don't allow double jump
				BroadcastMessage ("EndContact",  Vector2.zero, SendMessageOptions.DontRequireReceiver);//let the children know we stopped touching something
				BroadcastMessage("Jump", SendMessageOptions.DontRequireReceiver); //tell all children that we jumped

			} else if (touchingRight) { //wall jump
				acceleration.x -= jumpspeed * 0.3f;
				acceleration.y += jumpspeed * 0.3f;	
				touchingRight = false;
				touchingDown = false;
				timeSinceJumpPressed = jumpBuffer;//don't allow double jump
				BroadcastMessage ("EndContact",  Vector2.zero, SendMessageOptions.DontRequireReceiver);//let the children know we stopped touching something
				BroadcastMessage("Jump", SendMessageOptions.DontRequireReceiver); //tell all children that we jumped
			}
			else if (touchingDown) { //regular jump
				acceleration.y += jumpspeed;
				touchingDown = false;
				timeSinceJumpPressed = jumpBuffer;//don't allow double jump
				BroadcastMessage ("EndContact",  Vector2.zero, SendMessageOptions.DontRequireReceiver);//let the children know we stopped touching something
				BroadcastMessage("Jump", SendMessageOptions.DontRequireReceiver); //tell all children that we jumped
			}

		}


		if (touchingDown) acceleration += new Vector2 (Input.GetAxisRaw ("Horizontal"), 0) * walkspeed; //walking acceleration
		else acceleration += new Vector2 (Input.GetAxisRaw ("Horizontal"), 0) * airspeed; //air move acceleration

		if (touchingDown) acceleration.x += -walkdrag * velocity.x * Mathf.Abs (velocity.x); //friction on ground slows us down
		else acceleration.x += -airdrag * velocity.x * Mathf.Abs (velocity.x); //air resistance in the air slows us down
		acceleration.y += -airdrag * velocity.y * Mathf.Abs (velocity.y); //air resistance in the air slows us down

		velocity += acceleration * Time.fixedDeltaTime; //update velocity according to acceleration

		//we need to see if touching changes, because we want to wait until the body
		//has moved all the way to the surface before stopping it from moving more
		bool oldTouchingDown = touchingDown;
		bool oldTouchingUp = touchingUp;
		bool oldTouchingLeft = touchingLeft;
		bool oldTouchingRight = touchingRight;

		//call a function to detect collisions and zero out the velocity if we hit something
		UpdateTouches ();

		//update the position according to the velocity
		rb.MovePosition(transform.position + (Vector3)velocity * Time.fixedDeltaTime);
		
		
	}


	void Update(){
		BroadcastMessage ("UpdateVelocity", velocity,SendMessageOptions.DontRequireReceiver); //let all children know the new velocity

		//Camera should smoothly Lerp to the position of the player
		Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, new Vector3 (0, Mathf.Max(0f, transform.position.y - 2f),  -10f), 0.05f);
	}

	//This function updates data about if the player is touching anything
	void UpdateTouches(){

		//We're going to send out 4 imaginary boxes in 4 directions to see if the player should hit anything this fixedupdate
	
		Vector3 skin = new Vector3 (skinWidth, skinWidth,0); //reduce collider size for boxcasting so that the 'right' ray doesn't touch the ground, etc
		RaycastHit2D boxcastLeft = Physics2D.BoxCast (col.bounds.center, col.bounds.size - skin, 0, Vector2.right,Mathf.Min(velocity.x*Time.fixedDeltaTime - skinWidth,0), collisionMask);
		RaycastHit2D boxcastRight = Physics2D.BoxCast (col.bounds.center, col.bounds.size-skin, 0, Vector2.right, Mathf.Max(velocity.x*Time.fixedDeltaTime + skinWidth,0), collisionMask);
		RaycastHit2D boxcastDown = Physics2D.BoxCast (col.bounds.center, col.bounds.size-skin, 0, Vector2.up, Mathf.Min(velocity.y*Time.fixedDeltaTime-skinWidth,0), collisionMask);
		RaycastHit2D boxcastUp = Physics2D.BoxCast (col.bounds.center, col.bounds.size-skin, 0, Vector2.up, Mathf.Max(velocity.y*Time.fixedDeltaTime+skinWidth,0), collisionMask);


		if (boxcastRight.collider) { //the collider will be null unless we hit something to the right
			if (!touchingRight) { //new contact - move right up to the surface
				BroadcastMessage ("BeginContact", boxcastRight.point,SendMessageOptions.DontRequireReceiver); //let the children know we hit something and where
				velocity.x = (boxcastRight.distance - skinWidth/2f )/Time.fixedDeltaTime; //move exactly up to the colliding object this frame.
			}
			else { //old contact - don't let the player keep going in this direction
				velocity.x = Mathf.Min (0, velocity.x); //don't keep moving past the object we're touching
			}
			touchingRight = true;
		}
		else { //no contact
			if (touchingRight) BroadcastMessage ("EndContact",  boxcastRight.point ,SendMessageOptions.DontRequireReceiver);//let the children know we stopped touching something
			touchingRight = false;
		}
		if (boxcastLeft.collider){

			if (!touchingLeft){//new contact

				BroadcastMessage ("BeginContact",  boxcastLeft.point,SendMessageOptions.DontRequireReceiver);//let the children know we hit something and where
				velocity.x = (-boxcastLeft.distance + skinWidth/2f)/Time.fixedDeltaTime;//move exactly up to the colliding object this frame.
			}
			else {//old contact
				velocity.x = Mathf.Max (0, velocity.x);//don't keep moving past the object we're touching
			}

			touchingLeft = true; 
			
		}
		else { //no contact
			if (touchingLeft) BroadcastMessage ("EndContact",  boxcastLeft.point,SendMessageOptions.DontRequireReceiver);//let the children know we stopped touching something
			touchingLeft = false;
		}
		if (boxcastUp.collider) {
			if (!touchingUp) {//new contact
				BroadcastMessage ("BeginContact",  boxcastUp.point,SendMessageOptions.DontRequireReceiver);//let the children know we hit something and where
				velocity.y = (boxcastUp.distance - skinWidth/2f)/Time.fixedDeltaTime;//move exactly up to the colliding object this frame.
			}
			else {//old contact
				velocity.y = Mathf.Min (0, velocity.y);//don't keep moving past the object we're touching
			}
			touchingUp = true; 
		}
		else { //no contact
			if (touchingUp) BroadcastMessage ("EndContact",  boxcastUp.point,SendMessageOptions.DontRequireReceiver);//let the children know we stopped touching something
			touchingUp = false;
		}
		if (boxcastDown.collider) {

			if (!touchingDown){//new contact
				BroadcastMessage ("BeginContact",  boxcastDown.point,SendMessageOptions.DontRequireReceiver);//let the children know we hit something and where
				velocity.y =(-boxcastDown.distance + skinWidth/2f )/Time.fixedDeltaTime;//move exactly up to the colliding object this frame.
			}
			else {//old contact
				velocity.y = Mathf.Max (0, velocity.y);//don't keep moving past the object we're touching
			}

			touchingDown = true; 
		}
		else { //no contact
			if (touchingDown) BroadcastMessage ("EndContact",  boxcastDown.point,SendMessageOptions.DontRequireReceiver);//let the children know we stopped touching something
			touchingDown = false;
		}


		
	}

	//this function gets called by the block generator if a block hits the player
	public void HitBlock(){
		dead = true;
		BroadcastMessage ("Die",SendMessageOptions.DontRequireReceiver); //tell any children that we died
		Invoke("Reset", 1.0f);
	}

	public void Reset(){
		velocity = Vector2.zero; 
		transform.position = startingPos; //teleport back to original position
		dead = false;
		touchingLeft = false;
		touchingRight = false;
		touchingUp = false;
		touchingDown = false;
		timeSinceJumpPressed = jumpBuffer;
	}

}
