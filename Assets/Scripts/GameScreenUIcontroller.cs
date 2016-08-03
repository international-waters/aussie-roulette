/****************************************************************************
* Building IT Systems (CPT 111 / COSC 2635) SP2, 2016
* Game: Aussie Roulette  Group: International Waters
* Authors : Aaron Horton s3465420, David Morling s3492242
* Jeremy Cottell s3242784, Scott Nelson s3363315 , Simon Overton s3397924
*
* 
****************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameScreenUIcontroller : MonoBehaviour {
	private Board board;
	private Dropdown winNumber;
	private Text winnerlbl;
	private Dropdown chipValueList;
	private GameManager game;
	private BetView betView;
	private BetController betController;

	void Start(){
		game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		board = GameObject.Find ("RouletteTable").GetComponent<Board> ();
		winNumber = GameObject.Find("winNumberDropdown").GetComponent<Dropdown>();
		winnerlbl = GameObject.Find("winningNumber").GetComponent<Text>();
		chipValueList = GameObject.Find("ChipValueList").GetComponent<Dropdown>();
		game.RefreshScorePanel ();
	}
		

	public void OnBackButtonClick(){
		SceneManager.LoadScene ("StartScreen");
	}

	public void OnSaveButtonClick(){
		
		game.SaveGame(game.player, board);
		board.ClearAllBets ();
		game.RefreshScorePanel ();
	}

	public void OnLoadButtonClick(){
		game.player = game.LoadGame(game.player.playerName, board);
		game.player.CurrentBetTotal = board.CalculatePlayersTotalBet (game.player);
		game.RefreshScorePanel ();
		board.ClearStoredChipHistory ();
	}

	public void OnClearBetsButtonCick(){
		board.StoreAllPlacedChipInfo ();
		board.ClearAllBets (game.player);
		game.RefreshScorePanel ();
	}

	public void OnTestWinnerButtonClick(){
		int winner = winNumber.value;
		winnerlbl.text = winner.ToString ();
		game.ProcessWinNumber (board,winner);
	} 

	public void OnSpinButtonClick(){
		int winner = (int)Mathf.Round(Random.Range (0f, 36f));
		winnerlbl.text = winner.ToString ();
		game.ProcessWinNumber (board,winner);

	} 

	public void OnSpinDiffSceneButtonClick(){
		SceneManager.LoadScene ("SpinTestScene");
	}

	public void OnLoadLastBetsButtonClick(){
		board.ClearAllBets (game.player);
		board.PlaceAllStoredChips(game.player,board.SelectedChipValue);
		game.RefreshScorePanel ();
	}

	public void OnChipValueListChanged(){

		int value = 1;
		switch (chipValueList.value) {
		case 0:
			break;
		case 1:
			value = 5;
			break;
		case 2:
			value = 10;
			break;
		case 3:
			value = 25;
			break;
		
		case 4:
			value = 50;
			break;
		}
		board.ClearAllBets (game.player);
		board.SelectedChipValue = value;
		game.RefreshScorePanel ();
	}
		
}
