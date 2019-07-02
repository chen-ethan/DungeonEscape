using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class PlayerController : MonoBehaviour {

	public Animator animator;
    //-----------------------
    //tutorial
    GameObject Tut;
    bool stopped;

    //-------
    //Canvas UI;	
    public GameObject UI;
	private bool controlsActive;

	protected Joystick joystick;
	protected JoyButton joybutton;
	private bool buttonOn;
	protected bool buttonDown;
	private float buttonTimer;
	private float buttonCooldown;
	public float joyStickSens;
	//-----------------------
	[HideInInspector]
	public int level;
	public int Health;
	public Image[] Hearts;
	//-----------------------
	private bool startTimer;
	private float powerTimer;
	public float hermesTimer;
	public float wizardTimer;

	public float camoTimer;
	private float powerCooldown;

//	public float FireCooldown;
//-----------------------
	private int buttonStock;
	public int fireballStock;
	public int wizardStock;

	public int camoStock;
//-----------------------
	public Rigidbody2D FireBall; 
	[HideInInspector]
	public GameObject spawn;
	[HideInInspector]
	public bool hasKey;
	[HideInInspector]

	public string power;
	Rigidbody2D rigidbody;
	//-----------------------
	public GameObject enemyManager;
	GameObject[] enemies;
	Vector3[] enemyLocations;
	void Start () {
        stopped = false;

        Tut = GameObject.Find("Tutorial Controller");

        DontDestroyOnLoad(this.gameObject);
		level = 1;
		//UI = FindObjectOfType<Canvas>();
        UI = GameObject.Find("ControlsOverlay");
		power = "neutral";
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
		buttonTimer = 0.0f;
		startTimer = false;
		setToSpawn();

	}
	//-----------------------------------------------------------------------------------------------------

	void FixedUpdate () {
		if(Health <= 0){
			GameOver();
		}else{

			if(startTimer){
				powerTimer += Time.fixedDeltaTime;
				//buttonTimer += Time.deltaTime;
				if(powerTimer >= powerCooldown){
					Debug.Log("powerTimer up");
					resetPower();
					
				}
			}

			if(buttonOn){
				if(buttonStock > 0){
					if(!buttonDown && joybutton.pressed == true&& power!="neutral"){
						buttonDown = true;
						//buttonTimer = 0.0f;
						buttonStock--;
						UI.transform.GetChild(2).gameObject.GetComponent<UIstock>().setStock(buttonStock);
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
							UI.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
							joybutton.pressed = false;
							break;
						case "Fire":
							Debug.Log("Used all Fire Stock");
							resetPower();
							break;
						case "Camo":
							Debug.Log("Used all Camo Stock");
							powerTimer = 0.0f;
							powerCooldown = camoTimer;
							startTimer = true;
							buttonDown = false;
							buttonOn = false;
							UI.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
							UI.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;

							joybutton.pressed = false;
							break;
					}
				}
			}
            
                Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

                //transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
            if (stopped)
            {
                moveVector = Vector3.zero;
            }
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
		//Debug.Log("Collider");

		if(other.gameObject.CompareTag("Enemy")){
			//Debug.Log("Collided with enemy");
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
				//resetPower();
				setToSpawn();
				enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
				enemyManager.GetComponent<EnemyManager>().resetEnemies();

			}
		}
	}

	//-----------------------------------------------------------------------------------------------------
	void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("Collided");
        if (other.gameObject.CompareTag("TutWP")) {
            Debug.Log("GameObject"  + other.gameObject);
            GameObject pt = other.gameObject;
            Tut.GetComponent<Tutorial>().IncPoint(pt);
        }
		else if(!hasKey && other.gameObject.CompareTag("Key")){
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
			UI.transform.GetChild(1).gameObject.GetComponent<JoyButton>().setButton(0);

			buttonOn = true;
			power = "Fire";
			animator.SetBool("Neutral",false);
			animator.SetBool("Fire",true);
			//buttonCooldown = FireCooldown;
			buttonStock = fireballStock;
			UI.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
			UI.transform.GetChild(2).gameObject.GetComponent<UIstock>().setStock(buttonStock);
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}else if(other.gameObject.CompareTag("Wand")){
			resetPower();
			//enable button TODO: change sprite
			UI.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
			UI.transform.GetChild(1).gameObject.GetComponent<JoyButton>().setButton(1);
			buttonOn = true;
			power = "Wizard";
			animator.SetBool("Neutral",false);
			animator.SetBool("Wizard",true);
			buttonStock = wizardStock;
			UI.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
			UI.transform.GetChild(2).gameObject.GetComponent<UIstock>().setStock(buttonStock);
			//buttonCooldown = FireCooldown;
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;


		}else if(other.gameObject.CompareTag("Eye")){
			resetPower();
			UI.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
			UI.transform.GetChild(1).gameObject.GetComponent<JoyButton>().setButton(2);
			buttonOn = true;
			power = "Camo";
			buttonStock = camoStock;
			UI.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
			UI.transform.GetChild(2).gameObject.GetComponent<UIstock>().setStock(buttonStock);
			other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}

	}
	//-----------------------------------------------------------------------------------------------------
	void setToSpawn(){
		hasKey = false;
		resetPower();
		
		spawn = GameObject.FindGameObjectsWithTag("Respawn")[0];
		transform.position = spawn.transform.position;
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
			enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
			enemyManager.GetComponent<EnemyManager>().BunnyAll(false);
		}else if(power == "Camo"){
			power = "neutral";
			animator.SetBool("Camo",false);
			animator.SetBool("Neutral",true);
			enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
			enemyManager.GetComponent<EnemyManager>().swapBlindAll(false);
		}
		UI.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
		UI.transform.GetChild(2).gameObject.GetComponent<UIstock>().setStock(0);
		//button is 2nd canvas obj, so ind = 1
		buttonDown = false;
		buttonOn = false;
		//UI.transform.GetChild(1).gameObject.SetActive(false);
		UI.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
		joybutton.pressed = false;


	}
	//-----------------------------------------------------------------------------------------------------

	void usePower(){

		if(power == "Fire"){
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
			enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
			enemyManager.GetComponent<EnemyManager>().BunnyAll(true);

		}else if(power == "Camo"){
			animator.SetBool("Neutral",false);
			animator.SetBool("Camo",true);
			enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
			enemyManager.GetComponent<EnemyManager>().swapBlindAll(true);
		}
	}
	//-----------------------------------------------------------------------------------------------------
    public void standStill()
    {
        stopped = !stopped;

    }

    void GameOver(){
		if(controlsActive){
			animator.SetBool("Hermes",false);
			animator.SetBool("Fire",false);
			animator.SetBool("Wizard",false);
			animator.SetBool("Neutral",false);
			animator.SetBool("Camo",false);
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
			//setToSpawn();
			enableAllObjects();
			enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
			enemyManager.GetComponent<EnemyManager>().resetEnemies();
			

		}
		rigidbody.velocity = Vector3.zero;

	}
	//-----------------------------------------------------------------------------------------------------
}
