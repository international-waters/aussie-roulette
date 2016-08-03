using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // need this directive
using UnityEngine.UI;

public class SpinTestScript : MonoBehaviour {


	//game must be started from the start screen in order to create the peristant
	//game manager object
	void Awake () {

		if (GameObject.Find ("GameManager") == null) {
			Destroy (gameObject);
			SceneManager.LoadScene ("StartScreen");
		}
	}

	void Start (){
		GameManager game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		int randWinner = (int)Mathf.Round(Random.Range (0f, 36f));
		Text label = GameObject.Find ("WinNumberlbl").GetComponent<Text> ();
		label.text = randWinner.ToString ();

		//set the game managers win number flag so that it will be processed
		//when returning to main game screen
		game.winNumberFlag = randWinner;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
