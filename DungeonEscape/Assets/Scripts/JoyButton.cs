using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class JoyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	[HideInInspector]
	public bool pressed;
	public float AlphaThreshold = 0.1f;
	public Sprite[] buttons;

	void Start () {
		this.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setButton(int i){
		this.gameObject.GetComponent<Image>().sprite = buttons[i];
	}

	public void OnPointerDown(PointerEventData eventData){
		pressed = true;
	}

	public void OnPointerUp(PointerEventData eventData){
		pressed = false;
	}

}
