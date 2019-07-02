using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {

	// Use this for initialization
	public void PlayGame (){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void GoToMainMenu(){
		SceneManager.LoadScene(0);
		Destroy(GameObject.FindGameObjectWithTag("Player"));
	}
	public void QuitGame(){
		Application.Quit();
	}
}
