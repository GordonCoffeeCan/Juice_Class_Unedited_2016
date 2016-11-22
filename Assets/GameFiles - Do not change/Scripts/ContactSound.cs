using UnityEngine;
using System.Collections;
//This script simply plays a sound when the object is touched.
public class ContactSound : MonoBehaviour {
	AudioSource audio;

	void Start () {
		audio = GetComponent<AudioSource> ();
	}

	public void BeginContact(Vector2 point){
		if (!audio.isPlaying) audio.Play ();
	}
}
