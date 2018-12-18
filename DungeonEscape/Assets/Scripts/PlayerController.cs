using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {


	public Animator animator;
	protected Joystick joystick;
	public float joyStickSens;
	public int level;

	public Sprite defaultSprite;
	
	public Sprite hermesSprite;
	private float powerTimer;
	public float powerCooldown;

	public GameObject spawn;
	public bool hasKey;

	public string power;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		setToSpawn();
		joystick = FindObjectOfType<Joystick>();
		//joybutton = FindObjectOfType<JoyButton>();
		hasKey = false;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		var rigidbody = GetComponent<Rigidbody2D>();
		if(power != "neutral"){
			powerTimer += Time.deltaTime;
		}

		if(powerTimer >= powerCooldown && power!="neutral"){
			power = "neutral";
			joyStickSens /= 1.5f;
			this.GetComponent<SpriteRenderer>().sprite = defaultSprite;

		}
		/*rigidbody.velocity = new Vector2(joystick.Horizontal * joyStickSens,
										joystick.Vertical * joyStickSens);
		*/
		Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

		if(moveVector != Vector3.zero){
			//transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
			//transform.Translate(moveVector * joyStickSens * Time.deltaTime, Space.World);
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

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("Collided");
		if(!hasKey && other.gameObject.CompareTag("Key")){
			hasKey = true;
			Destroy(other.gameObject);
		}
		if(hasKey && other.gameObject.CompareTag("Door")){
			hasKey = false;
			Destroy(other.gameObject);
			Debug.Log("Next Level");
			level++;
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			setToSpawn();
		}
		if(other.gameObject.CompareTag("Shoe")){
			power = "Speed";
			Destroy(other.gameObject);
			joyStickSens *= 1.5f;
			this.GetComponent<SpriteRenderer>().sprite = hermesSprite;
			powerTimer = 0.0f;

		}

	}
	void setToSpawn(){
		spawn = GameObject.FindGameObjectsWithTag("Respawn")[0];
		transform.position = spawn.transform.position;
	}
}
