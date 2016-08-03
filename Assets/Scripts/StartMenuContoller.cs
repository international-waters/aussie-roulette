/****************************************************************************
* Building IT Systems (CPT 111 / COSC 2635) SP2, 2016
* Game: Aussie Roulette  Group: International Waters
* Authors : Aaron Horton s3465420, David Morling s3492242
* Jeremy Cottell s3242784, Scott Nelson s3363315 , Simon Overton s3397924
*
* 
****************************************************************************/
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

	public void OnHighScoresBackButtonClick(){
		SceneManager.LoadScene ("StartScreen",LoadSceneMode.Single);
	}

	public void OnGameScreenBackButtonClick(){
		SceneManager.LoadScene ("StartScreen",LoadSceneMode.Single);
	}

	public void OnExitGameButtonClick(){
		SceneManager.LoadScene("CloseScreen", LoadSceneMode.Single);
		SceneManager.LoadScene("CloseDialog",LoadSceneMode.Additive);
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
