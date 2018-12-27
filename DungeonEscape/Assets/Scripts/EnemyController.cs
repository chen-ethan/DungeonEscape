using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	// Use this for initialization
	public Animator animator;

	bool bunny;

	public GameObject[] walkPoints;
	public int currentPoint;

	public float speed;

	public string direction;

	private float bunnyDuration;
	private float bunnyTimer;

	Rigidbody2D rigidbody;
	void Start () {
		currentPoint = 0;
		rigidbody = GetComponent<Rigidbody2D>();
		bunny = false;

	}
	
	// Update is called once per frame
	void Update () {
		//.Log("enemy direction: " + direction);
		if(!bunny){
			if(direction == ""){
				direction = getDirection();
			//	Debug.Log("enemy direction null");
			}else if(direction == "Left"){
				rigidbody.velocity = (Vector3.right * speed * -1);
			//	Debug.Log("enemy direction left");
			}else if(direction == "Right"){
				rigidbody.velocity = (Vector3.right * speed);
			//	Debug.Log("enemy direction right");
			}else if(direction == "Up"){
				rigidbody.velocity = (Vector3.up * speed);			
			//	Debug.Log("enemy direction up");
			}else if(direction == "Down"){
				rigidbody.velocity = (Vector3.up * speed * -1);
			//	Debug.Log("enemy direction down");
				//float dist_away = walkPoints[currentPoint].transform.position.x - transform.position.x;

			}
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
		if(other == walkPoints[currentPoint].GetComponent<Collider2D>()){
			currentPoint = (currentPoint + 1)% walkPoints.Length;
			direction = getDirection();
		}
	}
	string getDirection(){
		float x_away = walkPoints[currentPoint].transform.position.x - transform.position.x;
		float y_away = walkPoints[currentPoint].transform.position.y - transform.position.y;
		if(Mathf.Abs(x_away) > Mathf.Abs(y_away)){
			if(x_away < 0){
				animator.SetBool("Down",false);
				animator.SetBool("Up",false);
				animator.SetBool("Right",false);
				animator.SetBool("Left",true);
				return "Left";
			}else{
				animator.SetBool("Down",false);
				animator.SetBool("Up",false);
				animator.SetBool("Right",true);
				animator.SetBool("Left",false);
				return "Right";
			}
		}else{
			if(y_away < 0){
				animator.SetBool("Down",true);
				animator.SetBool("Up",false);
				animator.SetBool("Right",false);
				animator.SetBool("Left",false);
				return "Down";
			}else{
				animator.SetBool("Down",false);
				animator.SetBool("Up",true);
				animator.SetBool("Right",false);
				animator.SetBool("Left",false);
				return "Up";

			}
		}
	}
}
