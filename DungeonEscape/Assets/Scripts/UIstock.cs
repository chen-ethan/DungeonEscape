using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIstock : MonoBehaviour {

	public Sprite[] numbers;
	private float AlphaThreshold = 0.1f;


	// Use this for initialization
	void Start () {
		this.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setStock(int i){
		this.gameObject.GetComponent<Image>().sprite = numbers[i];
	}

	public void enable(bool b){
		this.gameObject.GetComponent<Image>().enabled = b;
	}
}
