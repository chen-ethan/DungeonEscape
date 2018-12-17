using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	protected Joystick joystick;
	public float joyStickSens;
	// Use this for initialization
	void Start () {
		joystick = FindObjectOfType<Joystick>();
		
		//joybutton = FindObjectOfType<JoyButton>();
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
}
