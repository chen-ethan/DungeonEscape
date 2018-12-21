using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class JoyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	[HideInInspector]
	public bool pressed;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerDown(PointerEventData eventData){
		pressed = true;
	}

	public void OnPointerUp(PointerEventData eventData){
		pressed = false;
	}

}
