using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	GameObject[] enemies;
	Vector3[] enemyLocations;
	void Start () {
		enemyLocations = new Vector3[30];
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for(int i = 0; i < enemies.Length; ++i){
			enemyLocations[i] = enemies[i].transform.position;
			Debug.Log("EM: enemyLocations["+i+"] = " + enemyLocations[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void resetEnemies(){
		for(int i = 0; i < enemies.Length; ++i){
			Debug.Log("EM: reset: i = " + i);
			enemies[i].transform.position = enemyLocations[i];
			enemies[i].GetComponent<SpriteRenderer>().enabled = true;
			enemies[i].GetComponent<BoxCollider2D>().enabled = true;
			enemies[i].gameObject.GetComponent<FollowEnemyController>().Start();
		}
	}

	public void BunnyAll(bool enable){
		foreach(GameObject e in enemies){
			e.GetComponent<FollowEnemyController>().Bunny(enable);
		}
	}

	public void swapBlindAll(bool enable){
		foreach(GameObject e in enemies){
			e.GetComponent<FollowEnemyController>().swapBlind(enable);
		}
	}
}
