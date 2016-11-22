using UnityEngine;
using System.Collections;

//This script generates a bunch of particles every time you hit something.
public class ImpactParticles : MonoBehaviour {
	public int particlesPerCollision = 16; //number of particles generated each time we hit something
	ParticleSystem particles;
	float rayLength = 0.1f;
	public float particleMaxSpeed = 10f; //how fast the particles go when they are born
	public float particleMaxLifeTime = 1f; //how long the particles last

	public LayerMask collisionMask; //manually choose layers to collide with in the inspector

	void Start () {
		particles = GetComponent<ParticleSystem> (); //get a reference to the particle system
	}
	

	public void BeginContact(Vector2 point){

		//send an imaginary ray in each direction to see if we're touching a surface
		RaycastHit2D rayhitLeft = Physics2D.Raycast(point, new Vector2(-1,0), rayLength,collisionMask);
		RaycastHit2D rayhitRight = Physics2D.Raycast(point, new Vector2(1,0), rayLength,collisionMask);
		RaycastHit2D rayhitDown = Physics2D.Raycast (point, new Vector2(0,-1), rayLength,collisionMask);
		RaycastHit2D rayhitUp = Physics2D.Raycast (point, new Vector2(0,1), rayLength,collisionMask);


		for (int i=0; i< particlesPerCollision; i++) {
			Vector3 particleSpeed = Vector3.zero;

			if (rayhitLeft.collider){ //hit something to the left - so shoot out particles up and down
				particleSpeed = new Vector3(0,Random.Range (-particleMaxSpeed,particleMaxSpeed), 0); //vertical speed
				particles.Emit (rayhitLeft.point , particleSpeed, 1f, particleMaxLifeTime, Color.black);
			}
			if (rayhitRight.collider){
				particleSpeed = new Vector3(0,Random.Range (-particleMaxSpeed,particleMaxSpeed), 0);
				particles.Emit (rayhitRight.point , particleSpeed, 1f, particleMaxLifeTime, Color.black);
			}
			if (rayhitUp.collider) {//hit something above this object - so shoot out particles left and right
				particleSpeed = new Vector3(Random.Range (-particleMaxSpeed,particleMaxSpeed), 0, 0);//horizontal speed
				particles.Emit (rayhitUp.point , particleSpeed, 1f, particleMaxLifeTime, Color.black);
			}
			if (rayhitDown.collider) {
				particleSpeed = new Vector3(Random.Range (-particleMaxSpeed,particleMaxSpeed), 0, 0);
				particles.Emit (rayhitDown.point , particleSpeed, 1f, particleMaxLifeTime, Color.black);
			}
		}
	}
}
