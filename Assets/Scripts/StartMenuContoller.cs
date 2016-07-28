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
