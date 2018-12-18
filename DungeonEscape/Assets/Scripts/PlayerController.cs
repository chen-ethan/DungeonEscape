using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {

	protected Joystick joystick;
	public float joyStickSens;
	public int level;

	public Sprite defaultSprite;
	
	public Sprite hermesSprite;

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
		
		/*rigidbody.velocity = new Vector2(joystick.Horizontal * joyStickSens,
										joystick.Vertical * joyStickSens);
		*/
		Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);

		if(moveVector != Vector3.zero){
			//transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
			//transform.Translate(moveVector * joyStickSens * Time.deltaTime, Space.World);
			rigidbody.velocity = moveVector * joyStickSens;
		
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

		}

	}
	void setToSpawn(){
		spawn = GameObject.FindGameObjectsWithTag("Respawn")[0];
		transform.position = spawn.transform.position;
	}
}
