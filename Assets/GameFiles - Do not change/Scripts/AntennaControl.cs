using UnityEngine;
using System.Collections;

public class AntennaControl : MonoBehaviour {
	public Transform tip;
	public Transform spring;
	public SpriteRenderer springRenderer;
	FixedJoint2D joint;
	Vector3 originalSize;
	// Use this for initialization
	void Start () {

		joint = tip.GetComponent<FixedJoint2D> ();

		//get the original size of the sprite, unscaled, to use later
		originalSize =springRenderer.sprite.bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
		//get the  position of the spring's root
		Vector3 springRootPos = transform.position + (Vector3)joint.connectedAnchor;
		//stretch the spring to sit in between the tip and base
		spring.position = Vector3.Lerp(springRootPos, tip.position, 0.5f);
		//rotate the spring to match
		Vector2 distanceVector = (Vector2)(tip.position - springRootPos);
		Vector2 directionVector = distanceVector.normalized;
		float angle = Mathf.Atan2 (directionVector.y, directionVector.x) * Mathf.Rad2Deg;
		spring.localEulerAngles = new Vector3(0,0,angle);

		//scale the spring so it covers the distance
		float distance = distanceVector.magnitude;
		//make it long enough to cover the distance, and get thinner/fatter based on how stretched it is
		float horizontalStretch = distance/originalSize.x;
		float verticalStretch = Mathf.Clamp(1f / horizontalStretch,0.1f,1.1f); 
		spring.localScale = new Vector3 (horizontalStretch, verticalStretch, 1);
	}
}
