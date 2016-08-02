
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
        SceneManager.LoadScene("CloseScreen", LoadSceneMode.Single);
		SceneManager.LoadScene("CloseDialog",LoadSceneMode.Additive);
	}
		
	public void OnHighScoresBackButtonClick(){
		SceneManager.LoadScene ("StartScreen",LoadSceneMode.Single);
	}

	public void OnGameScreenBackButtonClick(){
		SceneManager.LoadScene ("StartScreen",LoadSceneMode.Single);
	}

    public void OnCloseDialogYes()
    {
        Application.Quit();
    }

    public void OnCloseDialogNo()
    {
        Application.CancelQuit();
        SceneManager.LoadScene("StartScreen", LoadSceneMode.Single);
    }

}
