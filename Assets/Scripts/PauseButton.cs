using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [HideInInspector]
    public bool pressed;
    public float AlphaThreshold = 0.1f;
    public bool paused;
    GameObject player;

    void Start()
    {
        //this.GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
        paused = false;
        player = this.transform.parent.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("script pointer down!");
        pressed = true;
        if (paused)
        {
            paused = false;
            player.GetComponent<PlayerController>().pause(false);
        }
        else
        {
            paused = true;
            player.GetComponent<PlayerController>().pause(true);
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("script pointer up!");

        pressed = false;
    }
}
