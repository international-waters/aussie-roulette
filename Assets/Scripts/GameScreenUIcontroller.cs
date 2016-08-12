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
	private GameManager game;
	private GameObject tableObj;
	private BetView betView;
	private BetController betController;
	public Toggle leaveBetsToggle;
	public Toggle lowValueToggle;
	public Toggle midValueToggle;
	public Toggle highValueToggle;
	private Text lowValueToggleTxt;
	private Text midValueToggleTxt;
	private Text highValueToggleTxt;

	private Toggle[] chipValueToggles;
	private Text[] chipValueTogglesTxt;

	void Start(){
		game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		board = GameObject.Find ("RouletteTable").GetComponent<Board> ();
		tableObj = GameObject.Find ("RouletteTable");
		try{
		winNumber = GameObject.Find("winNumberDropdown").GetComponent<Dropdown>();
		}
		catch{
		} 
		lowValueToggleTxt = GameObject.Find("LowValueToggleTxt").GetComponent<Text>();
		midValueToggleTxt = GameObject.Find("MidValueToggleTxt").GetComponent<Text>();
		highValueToggleTxt = GameObject.Find("HighValueToggleTxt").GetComponent<Text>();

		chipValueToggles = new Toggle[3]{ lowValueToggle, midValueToggle, highValueToggle };
		chipValueTogglesTxt = new Text[3]{ lowValueToggleTxt, midValueToggleTxt, highValueToggleTxt };
			
		initValueToggles ();
		leaveBetsToggle.isOn = board.isRepeatingBets;

		game.RefreshScorePanel ();
	}
		

	public void OnExitButtonClick(){
		game.SaveGame(game.player, board);
		game.selectedPlayerName = string.Empty;
		game.player = new Player ();
		DestroyObject (tableObj);
		SceneManager.LoadScene ("StartScreen");

	}

	public void OnSaveButtonClick(){
		game.SaveGame(game.player, board);
		//reload saved game (saving clears the board);
		OnLoadButtonClick();

	}

	public void OnLoadButtonClick(){
		game.player = game.LoadGame(game.player.playerName, board);
		game.player.CurrentBetTotal = board.CalculatePlayersTotalBet (game.player);
		game.RefreshScorePanel ();
		board.ClearStoredChipHistory ();
		this.initValueToggles ();
	}

	public void OnChipValueChanged(){
		for (int i = 0; i < 3; i++) {
			Text valueText = chipValueTogglesTxt[i].GetComponent<Text> ();
			if (chipValueToggles [i].isOn == true) {
				valueText.color = Color.yellow;
				int newChipValue = int.Parse (valueText.text);
				//save location of all bets
				board.StoreAllPlacedChipInfo ();
				//clear current bets and credit the player
				board.ClearAllBets (game.player);
				board.SelectedChipValue = newChipValue;
				//attempt to replace all bets at the new value 
				//(will not succeed if player cannot afford all bets)
				board.PlaceAllStoredChips (game.player,newChipValue);
				game.RefreshScorePanel ();

			} else {
				valueText.color = Color.white;
			}
		}
	}

	public void OnClearBetsButtonCick(){
		board.StoreAllPlacedChipInfo ();
		board.ClearAllBets (game.player, ChipMove.ToPlayer);
		game.RefreshScorePanel ();
	}

	public void OnTestWinnerButtonClick(){
		int winner = winNumber.value;
		game.ProcessWinNumber (winner);
	} 

	public void OnSpinButtonClick(){
		int winner = (int)Mathf.Round(Random.Range (0f, 36f));
		game.ProcessWinNumber (winner);

	} 

	public void OnSpinDiffSceneButtonClick(){
		SceneManager.LoadScene ("rouletteWheel");
	}
		

	public void OnRepeatLastBetsChanged(){
		board.isRepeatingBets = leaveBetsToggle.isOn;
	}
		

	public void initValueToggles(){
		switch (board.SelectedChipValue) {
		case(5):
			midValueToggle.isOn = true;
			break;
		case(10):
			highValueToggle.isOn = true;
			break;
		default:
			lowValueToggle.isOn = true;
			break;
		}
	}
		
}
