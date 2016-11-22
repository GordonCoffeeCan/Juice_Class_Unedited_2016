using UnityEngine;
using System.Collections;

public class DisableOnPlay : MonoBehaviour {

	void Start () {
		gameObject.SetActive (false);
	}

}
