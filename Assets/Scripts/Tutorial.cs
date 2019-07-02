using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {


    public GameObject UI;
    public GameObject controls;
    public GameObject[] walkPoints;
    public int currentPoint;
    public int currentPanel;
    public GameObject player;
    public GameObject[] panels;

    public GameObject FireBall;

    int maxPoints;
    int maxPanels;

    // Use this for initialization
    void Start () {
        UI = GameObject.Find("Tutorial Popups");
        controls = GameObject.Find("ControlsOverlay");
        UI.SetActive(false);
        currentPoint = 0;
        currentPanel = 0;
        player = GameObject.FindWithTag("Player");
        maxPoints = walkPoints.Length;
        maxPanels = panels.Length;
        for(int i =0; i< maxPanels; ++i)
        {
            panels[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}

    //GameObject point
    public void IncPoint(GameObject point)
    {
       
        if (point == walkPoints[currentPoint])
        {
            UI.SetActive(true);
            //controls.SetActive(false);
            Debug.Log("current point" + currentPoint);
            panels[currentPanel].SetActive(true);
            walkPoints[currentPoint].SetActive(false);

            
            player.GetComponent<PlayerController>().standStill();

        }
    }

    public void part()
    {
        panels[currentPanel].SetActive(false);
        currentPanel++;
        panels[currentPanel].SetActive(true);

    }

    public void Okay()
    {
        panels[currentPanel].SetActive(false);
        if(currentPoint == 4)
        {
            FireBall.SetActive(true);
        }
        currentPanel++;
        currentPoint++;
        UI.SetActive(false);
        controls.SetActive(true);
        player.GetComponent<PlayerController>().standStill();
    }
}

