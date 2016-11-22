using UnityEngine;
using System.Collections;

public class ResetLocalScale : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//reset scale in case the parent has nonuniform scale
		transform.localScale = new Vector3 (transform.localScale.x/transform.lossyScale.x, transform.localScale.y/transform.lossyScale.y, 1);

	}
}
