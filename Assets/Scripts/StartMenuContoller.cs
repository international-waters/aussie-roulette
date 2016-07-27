using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuContoller : MonoBehaviour {

	public void OnPlayButtonClick(){
		SceneManager.LoadScene ("GamePlayScreen");
	}
	public void OnHighScoresButtonClick(){
		SceneManager.LoadScene ("HighScoresScreen");
	}

	public void OnExitGameButtonClick(){
		Application.Quit();
	}
		
	public void OnHighScoresBackButtonClick(){
		SceneManager.LoadScene ("GamePlayScreen");
	}
		

}
