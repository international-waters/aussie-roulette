using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DisplayHighScores : MonoBehaviour {
	private GameManager game;
	void Start () {

		game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		List<HighScore> list = game.LoadHighScores ();

		for (int i = 0; i < 5; i++) {
			Text namelbl = GameObject.Find ("PlayerName" + (i + 1)).GetComponent<Text> ();
			Text scorelbl = GameObject.Find ("HighScore" + (i + 1)).GetComponent<Text> ();
			namelbl.text = list [i].playerName;
			scorelbl.text = list [i].score.ToString();
		}

	
	}
	

}
