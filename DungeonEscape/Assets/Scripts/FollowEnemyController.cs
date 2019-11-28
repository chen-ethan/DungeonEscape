using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemyController : MonoBehaviour {
	
	public Animator animator;
//-----------------------
	public GameObject[] walkPoints;
	public int currentPoint;
//-----------------------
	public bool Follow;
	private bool triggered;
//-----------------------

	private bool blind;
//-----------------------
	public float speed;
	public string direction;
//-----------------------
	private Transform target;
	Rigidbody2D rigidbody;
//-----------------------
	bool bunny;


	// Use this for initialization
	public void Start () {
		currentPoint = 0;
		direction = "";
		bunny = false;
		rigidbody = GetComponent<Rigidbody2D>();
		rigidbody.velocity = Vector2.zero;
		target= transform;
		triggered = false;
		blind = false;
		animator.SetBool("Bunny", false);
		animator.SetBool("Right", false);
		animator.SetBool("Left", false);
		animator.SetBool("Up", false);
		animator.SetBool("Down", true);
		animator.SetFloat("Speed",0.0f);

	}
	
	// Update is called once per frame
	void Update () {
		if(triggered && Follow && !blind){
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed*Time.deltaTime);
			
			animator.SetFloat("Speed",speed);
			//var direction =(Vector2) (target.position);
			//rigidbody.velocity = direction * speed;
			float x_dif = transform.position.x - target.position.x;
			float y_dif = transform.position.y - target.position.y;
			if(Mathf.Abs(x_dif) > Mathf.Abs(y_dif)){
				if(x_dif < 0){
					animator.SetBool("Right", true);
					animator.SetBool("Left", false);
					animator.SetBool("Up", false);
					animator.SetBool("Down", false);
				} else if(x_dif > 0){
					animator.SetBool("Left", true);
					animator.SetBool("Right", false);
					animator.SetBool("Up", false);
					animator.SetBool("Down", false);
				}
			} else{
				if(y_dif < 0){
					animator.SetBool("Up", true);
					animator.SetBool("Right", false);
					animator.SetBool("Left", false);
					animator.SetBool("Down", false);
				} else if(y_dif > 0){
					animator.SetBool("Down", true);				
					animator.SetBool("Right", false);
					animator.SetBool("Up", false);
					animator.SetBool("Left", false);
				}
			}
		}else if(!bunny && walkPoints.Length > 0){
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
			animator.SetFloat("Speed",Mathf.Abs(rigidbody.velocity.x)+Mathf.Abs(rigidbody.velocity.y));

		}else if(bunny){
			rigidbody.velocity = Vector3.zero;
			animator.SetFloat("Speed", 0.0f);

		}
		if(blind){
			animator.SetFloat("Speed", 0.0f);

		}
	}

	public void swapBlind(bool enable){
		if(!blind && enable){
			triggered = false;
			blind = true;
			animator.SetBool("Down",true);
			animator.SetBool("Up",false);
			animator.SetBool("Right",false);
			animator.SetBool("Left",false);
		}else{
			blind = false;
		}
	}
//-----------------------------------------------------------------------------------------------------

	public void Bunny(bool enable){
		if(!bunny && enable){
			bunny = true;
			triggered = false;
			animator.SetBool("Down",false);
			animator.SetBool("Up",false);
			animator.SetBool("Right",false);
			animator.SetBool("Left",false);
			animator.SetBool("Bunny",true);
			this.gameObject.tag = "Bunny";
			//this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
			this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
		}else{
			bunny = false;
			animator.SetBool("Bunny",false);
			this.gameObject.tag = "Enemy";
			this.gameObject.GetComponent<BoxCollider2D>().enabled = true;

		}
	}

	//-----------------------------------------------------------------------------------------------------

	void OnTriggerEnter2D(Collider2D other){
		if(walkPoints.Length > 0 && other == walkPoints[currentPoint].GetComponent<Collider2D>()){
			currentPoint = (currentPoint + 1)% walkPoints.Length;
			direction = getDirection();
		}
		if(!bunny && !blind && other.gameObject.CompareTag("Player")){
			target = other.gameObject.transform;
			triggered = true;
		}
	}
	//-----------------------------------------------------------------------------------------------------
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
