using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyController : MonoBehaviour {
	
	public Animator animator;

//-----------------------
	public float speed;

	public string direction;
//-----------------------
	bool bunny;
	private float bunnyDuration;
	private float bunnyTimer;

	private Transform target;

	Rigidbody2D rigidbody;


	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		bunny = false;
		target.position = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if(!bunny){
			Debug.Log("Moving");
			transform.position = Vector2.MoveTowards(transform.position, target.position, speed*Time.deltaTime);
		}else{
			rigidbody.velocity = Vector3.zero;
			bunnyTimer += Time.deltaTime;
			if(bunnyTimer >= bunnyDuration){
				resetBunny();
			}
		}
	}

	public void Bunny(float Time){
		if(!bunny){
			Debug.Log("EnemyC: setBunny");
			bunny = true;
			bunnyDuration = Time;
			bunnyTimer = 0.0f;
			animator.SetBool("Down",false);
			animator.SetBool("Up",false);
			animator.SetBool("Right",false);
			animator.SetBool("Left",false);
			animator.SetBool("Bunny",true);
			this.gameObject.tag = "Bunny";
			this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}
	}

	public void resetBunny(){

		Debug.Log("EnemyC: RE - setBunny");
		bunny = false;
		animator.SetBool("Bunny",false);
		this.gameObject.tag = "Enemy";
		this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("Player")){
			target = other.gameObject.transform;
		}
	}
}
