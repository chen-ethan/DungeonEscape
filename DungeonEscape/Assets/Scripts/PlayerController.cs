using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {


	public Animator animator;
	protected Joystick joystick;
	protected JoyButton joybutton;


	protected bool buttonDown;
	private float buttonTimer;
	private float buttonCooldown;

	public float joyStickSens;
	public int level;

	public int Health;
	public Image[] Hearts;
	private float powerTimer;
	public float powerCooldown;

	public float FireCooldown;


	public GameObject spawn;
	public bool hasKey;

	public string power;

	private bool controlsActive;

	Rigidbody2D rigidbody;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		level = 1;
		Debug.Log("level: "+ level);
		setToSpawn();
		joystick = FindObjectOfType<Joystick>();
		joybutton = FindObjectOfType<JoyButton>();
		hasKey = false;
		animator.SetBool("Neutral",true);
		rigidbody =  GetComponent<Rigidbody2D>(); 
		controlsActive = true;
		for(int i = 0; i < Health; ++i){
			Hearts[i].GetComponent<Image>().enabled = true;
		}
		buttonTimer = 0.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Health <= 0){
			GameOver();
		}else{

			if(power != "neutral"){
				powerTimer += Time.deltaTime;
				buttonTimer += Time.deltaTime;
			}
			if(powerTimer >= powerCooldown && power!="neutral"){
				resetPower();
			}
			if(!buttonDown && joybutton.pressed == true&&buttonTimer >= buttonCooldown&&power!="neutral"){
				buttonDown = true;
				buttonTimer = 0.0f;
				usePower();
			}
			if(buttonDown && joybutton.pressed == false){
				buttonDown = false;
			}
			Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

			//transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
			rigidbody.velocity = moveVector * joyStickSens;

			//animation code
			animator.SetFloat("Speed", Mathf.Abs(moveVector.x) +Mathf.Abs(moveVector.y));
			if(moveVector.y<0 && Mathf.Abs(moveVector.y)>Mathf.Abs(moveVector.x)){
				animator.SetBool("Down",true);
				animator.SetBool("Up",false);
				animator.SetBool("Right",false);
				animator.SetBool("Left",false);
			}else if(moveVector.y>0 && Mathf.Abs(moveVector.y)>Mathf.Abs(moveVector.x)){
				animator.SetBool("Down",false);
				animator.SetBool("Up",true);
				animator.SetBool("Right",false);
				animator.SetBool("Left",false);
			}else if(moveVector.x>0 && Mathf.Abs(moveVector.x)>Mathf.Abs(moveVector.y)){
				animator.SetBool("Down",false);
				animator.SetBool("Up",false);
				animator.SetBool("Right",true);
				animator.SetBool("Left",false);
			}else if(moveVector.x<0 && Mathf.Abs(moveVector.x)>Mathf.Abs(moveVector.y)){
				animator.SetBool("Down",false);
				animator.SetBool("Up",false);
				animator.SetBool("Right",false);
				animator.SetBool("Left",true);
			}
		}		
		
	}
	void OnCollisionEnter2D(Collision2D other){
		Debug.Log("Collider");

		if(other.gameObject.CompareTag("Enemy")){
			Debug.Log("Collided with enemy");
			TakeDamage();
		}
	}

	void TakeDamage(){
		if(Health>0){
			Health--;
			Hearts[Health].GetComponent<Image>().enabled = false;
			if(Health>0){
				enableAllObjects();
				resetPower();
				setToSpawn();
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log("Collided");
		if(!hasKey && other.gameObject.CompareTag("Key")){
			hasKey = true;
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}
		if(hasKey && other.gameObject.CompareTag("Door")){
			hasKey = false;
			Destroy(other.gameObject);
			Debug.Log("Next Level");
			this.level++;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			setToSpawn();
		}
		if(other.gameObject.CompareTag("Shoe")){
			resetPower();
			power = "Hermes";
			joyStickSens *= 1.5f;
			animator.SetBool("Neutral",false);
			animator.SetBool("Hermes",true);
			powerTimer = 0.0f;
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}
		if(other.gameObject.CompareTag("FireBall")){
			resetPower();
			power = "Fire";
			animator.SetBool("Neutral",false);
			animator.SetBool("Fire",true);
			powerTimer = 0.0f;
			buttonCooldown = FireCooldown;
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}

	}
	void setToSpawn(){
		hasKey = false;
		//resetPower();
		
		spawn = GameObject.FindGameObjectsWithTag("Respawn")[0];
//Debug.Log("Spawn position 2: "+ spawn.transform.position);

		//Debug.Log("Spawns[length] = " + GameObject.FindGameObjectsWithTag("Respawn").Length);
		transform.position = spawn.transform.position;
	}

	void enableAllObjects(){
		foreach (SpriteRenderer GO in GameObject.FindObjectsOfType<SpriteRenderer>()){
			if(GO.gameObject.layer == 8){
				GO.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				GO.gameObject.GetComponent<BoxCollider2D>().enabled = true;
			}
		}
	}

	void resetPower(){
		if(power == "Hermes"){
			power = "neutral";
			animator.SetBool("Hermes",false);
			animator.SetBool("Neutral",true);
			joyStickSens /= 1.5f;
		}else if(power == "Fire"){
			power = "neutral";
			animator.SetBool("Fire",false);
			animator.SetBool("Neutral",true);
		}

	}

	void usePower(){
		if(power == "Fire"){
			Debug.Log("FIREBALL");
		}
	}

	void GameOver(){
		if(controlsActive){
			animator.SetBool("Hermes",false);
			animator.SetBool("Neutral",false);
			animator.SetBool("Dead",true);
			Canvas UI = Object.FindObjectOfType<Canvas>();
			for(int i=0; i< UI.transform.childCount; i++){
				var child = UI.transform.GetChild(i).gameObject;
				if(child.layer != 9){
					child.SetActive(false);
				}else{
					child.SetActive(true);

				}
			}

		}
		rigidbody.velocity = Vector3.zero;

	}
}
