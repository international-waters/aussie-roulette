using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuContoller : MonoBehaviour {

	public void OnPlayButtonClick(){
		SceneManager.LoadScene ("GamePlayScreen",LoadSceneMode.Single);
	}
	public void OnHighScoresButtonClick(){
		SceneManager.LoadScene ("HighScoresScreen",LoadSceneMode.Single);
	}

	public void OnExitGameButtonClick(){
		Application.Quit();
	}
		
	public void OnHighScoresBackButtonClick(){
		SceneManager.LoadScene ("GamePlayScreen",LoadSceneMode.Single);
	}

	public void OnGameScreenBackButtonClick(){
		SceneManager.LoadScene ("StartScreen",LoadSceneMode.Single);
	}


}
