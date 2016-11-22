using UnityEngine;
using System.Collections;

//This script generates blocks at the top of the screen above the player
public class BlockMakerScript : MonoBehaviour {
	public GameObject blockPrefab;
	public GameObject player;
	float timeUntilSpawn;
	float spawnDelay ;

	void Start () {
		ResetGame ();
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilSpawn -= Time.deltaTime;
		if (timeUntilSpawn <= 0f) {

			//pick a position above the player, but round it to an integer so the blocks fall in nice columns
			float playerPosX = Mathf.Round(player.transform.position.x);

			//make sure the spawn position is just out of view of the camera at the top
			float cameraPosY = Camera.main.transform.position.y + 6.0f;

			//instantiate a new block
			GameObject newBlock = Instantiate(blockPrefab,new Vector3( playerPosX, cameraPosY,0.0f),Quaternion.identity) as GameObject;
			newBlock.transform.parent = transform;
			spawnDelay *= 0.995f; //gradually speed up
			timeUntilSpawn = Random.Range(-0.1f,0.1f) + spawnDelay;//slightly randomize the delay until the next block
		}
	}

	//this function gets called by a block if it hits the player
	public void HitPlayer(){
		Debug.Log ("gameOver");
		player.SendMessage ("HitBlock"); //let the player know it's been hit
		Invoke ("ResetGame", 0.8f); //reset the game after 0.8 seconds.
	}

	public void ResetGame(){
		foreach(Transform child in transform) {
			Destroy(child.gameObject); //destroy all the blocks from the previous game by looking in this parent object's transform
		}
		spawnDelay = 2.0f;
		timeUntilSpawn = spawnDelay;
	}
}
