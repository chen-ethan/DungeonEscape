using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {

	public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * Time.deltaTime * speed;
	}
	/* 
	void OnCollisionEnter2D(Collision2D other){
		Debug.Log("Fireball Collided");

		if(other.gameObject.CompareTag("Wall")){
			Destroy(this.gameObject);
		}
	}*/
	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("Fireball Collided");

		if(other.gameObject.CompareTag("Wall")){
			Destroy(this.gameObject);
		}else if(other.gameObject.CompareTag("Enemy")){
			Destroy(other.gameObject);
			Destroy(this.gameObject);

		}
	}
}
