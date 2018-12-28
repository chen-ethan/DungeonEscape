using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {

	public Animator animator;
	//-----------------------
	Canvas UI;	
	private bool controlsActive;

	protected Joystick joystick;
	protected JoyButton joybutton;
	private bool buttonOn;
	protected bool buttonDown;
	private float buttonTimer;
	private float buttonCooldown;
	public float joyStickSens;
	//-----------------------
	public int level;
	public int Health;
	public Image[] Hearts;
	//-----------------------
	private bool startTimer;
	private float powerTimer;
	public float hermesTimer;
	public float wizardTimer;
	private float powerCooldown;

//	public float FireCooldown;
//-----------------------
	private int buttonStock;
	public int fireballStock;
	public int wizardStock;
//-----------------------
	public Rigidbody2D FireBall; 
	public GameObject spawn;
	public bool hasKey;
	public string power;
	Rigidbody2D rigidbody;
	//-----------------------
	GameObject[] enemies;
	Vector3[] enemyLocations;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		level = 1;
		UI = Object.FindObjectOfType<Canvas>();
		//UI.transform.GetChild(1).gameObject.SetActive(false);
		enemyLocations = new Vector3[30];

		setToSpawn();
		joystick = FindObjectOfType<Joystick>();
		joybutton = FindObjectOfType<JoyButton>();
		buttonOn = false;
		buttonDown = false;
		hasKey = false;
		animator.SetBool("Neutral",true);
		rigidbody =  GetComponent<Rigidbody2D>(); 
		controlsActive = true;
		for(int i = 0; i < Health; ++i){
			Hearts[i].GetComponent<Image>().enabled = true;
		}
		//enemyLocations[0] = this.transform;
		//Debug.Log(enemyLocations[0]);
		buttonTimer = 0.0f;
		startTimer = false;
	}
	//-----------------------------------------------------------------------------------------------------

	// Update is called once per frame
	void FixedUpdate () {
		if(Health <= 0){
			GameOver();
		}else{

			if(startTimer){
				powerTimer += Time.deltaTime;
				//buttonTimer += Time.deltaTime;
				if(powerTimer >= powerCooldown){
					Debug.Log("powerTimer up");
					resetPower();
				}
			}

			if(buttonOn){
				if(buttonStock > 0){
					if(!buttonDown && joybutton.pressed == true&&/* buttonTimer >= buttonCooldown&& */ power!="neutral"){
						buttonDown = true;
						//buttonTimer = 0.0f;
						buttonStock--;
						usePower();
					}
					if(buttonDown && joybutton.pressed == false){
						buttonDown = false;
					}
				}else{
					switch(power){
						case "Wizard":
							Debug.Log("Used all Wizard Stock");
							powerTimer = 0.0f;
							powerCooldown = wizardTimer;
							startTimer = true;
							buttonDown = false;
							buttonOn = false;
							UI.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
							UI.transform.GetChild(1).gameObject.GetComponent<JoyButton>().enabled = false;
							joybutton.pressed = false;
							break;
						case "Fire":
							Debug.Log("Used all Fire Stock");
							resetPower();
							break;
					}
				}
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
	//-----------------------------------------------------------------------------------------------------
	void OnCollisionEnter2D(Collision2D other){
		Debug.Log("Collider");

		if(other.gameObject.CompareTag("Enemy")){
			Debug.Log("Collided with enemy");
			TakeDamage();
		}
	}
	//-----------------------------------------------------------------------------------------------------

	void TakeDamage(){
		if(Health>0){
			Health--;
			Hearts[Health].GetComponent<Image>().enabled = false;
			if(Health>0){
				enableAllObjects();
				resetPower();
				resetEnemies();
				setToSpawn();
			}
		}
	}
	//-----------------------------------------------------------------------------------------------------
	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log("Collided");
		if(!hasKey && other.gameObject.CompareTag("Key")){
			hasKey = true;
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}else if(hasKey && other.gameObject.CompareTag("Door")){
			hasKey = false;
			Destroy(other.gameObject);
			Debug.Log("Next Level");
			this.level++;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			setToSpawn();
		}else if(other.gameObject.CompareTag("Shoe")){
			resetPower();
			power = "Hermes";
			joyStickSens *= 1.5f;
			animator.SetBool("Neutral",false);
			animator.SetBool("Hermes",true);
			powerTimer = 0.0f;
			startTimer = true;
			powerCooldown = hermesTimer;
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}else if(other.gameObject.CompareTag("FireBall")){
			resetPower();
			UI.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
			UI.transform.GetChild(1).gameObject.GetComponent<JoyButton>().enabled = true;
			buttonOn = true;
			power = "Fire";
			animator.SetBool("Neutral",false);
			animator.SetBool("Fire",true);
			//buttonCooldown = FireCooldown;
			buttonStock = fireballStock;
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}else if(other.gameObject.CompareTag("Wand")){
			resetPower();
			//enable button TODO: change sprite
			UI.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
			UI.transform.GetChild(1).gameObject.GetComponent<JoyButton>().enabled = true;
			buttonOn = true;
			power = "Wizard";
			animator.SetBool("Neutral",false);
			animator.SetBool("Wizard",true);
			buttonStock = wizardStock;
			//buttonCooldown = FireCooldown;
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;


		}

	}
	//-----------------------------------------------------------------------------------------------------
	void setToSpawn(){
		hasKey = false;
		//resetPower();
		
		spawn = GameObject.FindGameObjectsWithTag("Respawn")[0];
		//Debug.Log("Spawn position 2: "+ spawn.transform.position);

		//Debug.Log("Spawns[length] = " + GameObject.FindGameObjectsWithTag("Respawn").Length);
		transform.position = spawn.transform.position;
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for(int i = 0; i < enemies.Length; ++i){
			Debug.Log(enemies[i].transform);
			enemyLocations[i] = enemies[i].transform.position;
			Debug.Log(enemyLocations[i]);

		}
	}
	//-----------------------------------------------------------------------------------------------------

	void enableAllObjects(){
		foreach (SpriteRenderer GO in GameObject.FindObjectsOfType<SpriteRenderer>()){
			if(GO.gameObject.layer == 8){
				GO.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				GO.gameObject.GetComponent<BoxCollider2D>().enabled = true;
			}
		}
	}
	//-----------------------------------------------------------------------------------------------------
	void resetEnemies(){
		for(int i = 0; i < enemies.Length; ++i){
			enemies[i].transform.position = enemyLocations[i];
			enemies[i].GetComponent<SpriteRenderer>().enabled = true;
			enemies[i].GetComponent<BoxCollider2D>().enabled = true;
			enemies[i].gameObject.GetComponent<FollowEnemyController>().Start();
		}
	}
	//-----------------------------------------------------------------------------------------------------


	void resetPower(){
		startTimer = false;
		if(power == "Hermes"){
			power = "neutral";
			animator.SetBool("Hermes",false);
			animator.SetBool("Neutral",true);
			joyStickSens /= 1.5f;
		}else if(power == "Fire"){
			power = "neutral";
			animator.SetBool("Fire",false);
			animator.SetBool("Neutral",true);
		}else if(power == "Wizard"){
			power = "neutral";
			animator.SetBool("Wizard",false);
			animator.SetBool("Neutral",true);
		}
		//button is 2nd canvas obj, so ind = 1
		buttonDown = false;
		buttonOn = false;
		//UI.transform.GetChild(1).gameObject.SetActive(false);
		UI.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
		UI.transform.GetChild(1).gameObject.GetComponent<JoyButton>().enabled = false;
		joybutton.pressed = false;


	}
	//-----------------------------------------------------------------------------------------------------

	void usePower(){
		Debug.Log("usePower:");

		if(power == "Fire"){
			Debug.Log("FIREBALL");
			Rigidbody2D clone;
			if(animator.GetBool("Up") == true){
				clone = Instantiate(FireBall,this.transform.position,Quaternion.Euler(0,0,90));
			}else if(animator.GetBool("Down") == true){
				clone = Instantiate(FireBall,this.transform.position,Quaternion.Euler(0,0,270));
			}else if(animator.GetBool("Right") == true){
				clone = Instantiate(FireBall,this.transform.position,Quaternion.Euler(0,0,0));
			}else if(animator.GetBool("Left") == true){
				clone = Instantiate(FireBall,this.transform.position,Quaternion.Euler(0,0,180));
			}
		}else if(power == "Wizard"){
			Debug.Log("BUNNY");
			//GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(GameObject e in enemies){
				Debug.Log("Turn into bunny");
				e.GetComponent<FollowEnemyController>().Bunny(wizardTimer);
			}


		}
	}
	//-----------------------------------------------------------------------------------------------------

	void GameOver(){
		if(controlsActive){
			animator.SetBool("Hermes",false);
			animator.SetBool("Fire",false);
			animator.SetBool("Wizard",false);
			animator.SetBool("Neutral",false);
			animator.SetBool("Dead",true);
			//Canvas UI = Object.FindObjectOfType<Canvas>();
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
	//-----------------------------------------------------------------------------------------------------
}
