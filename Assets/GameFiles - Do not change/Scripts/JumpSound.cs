using UnityEngine;
using System.Collections;

//This script just plays a sound when the player presses the jump button
public class JumpSound : MonoBehaviour {
	AudioSource audio;

	void Start () {
		audio = GetComponent<AudioSource> ();
	}

	void Jump(){
		if (!audio.isPlaying) audio.Play ();
	}

}
