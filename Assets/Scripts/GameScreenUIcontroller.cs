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

public class GameScreenUIcontroller : MonoBehaviour {
	private Player player;
	private Board board;
	private Dropdown winNumber;
	private Text lastWin;
	private Text placedBets;
	private Text wallet;
	private Dropdown chipValue;

	void Start(){
		board = GameObject.Find ("RouletteTable").GetComponent<Board> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		winNumber = GameObject.Find("winNumberDropdown").GetComponent<Dropdown>();
		lastWin = GameObject.Find("winAmount").GetComponent<Text>();
		placedBets = GameObject.Find("betTotal").GetComponent<Text>();
		wallet = GameObject.Find("balance").GetComponent<Text>();
		chipValue = GameObject.Find("ChipValueList").GetComponent<Dropdown>();
	}

	public void OnBackButtonClick(){
		SceneManager.LoadScene ("StartScreen",LoadSceneMode.Single);
	}

	public void OnClearBetsButtonCick(){
		board.ClearAllBets (player);
		player.CurrentBetTotal = 0;
		lastWin.text = "0";
	}

	public void OnTestWinnerButtonClick(){
		board.SaveAllBets ();
		board.ClearLosingBets (winNumber.value);
		int winnings = board.PayWinnings (winNumber.value, player);
		lastWin.text = winnings.ToString ();
		//player.RecieveWinnings(winnings);
		player.CurrentBetTotal = board.CalculatePlayersTotalBet (player);
		placedBets.text = player.CurrentBetTotal.ToString ();
		wallet.text = player.Wallet.ToString ();
	}

	public void OnLoadLastBetsButtonClick(){
		board.LoadBets (player);
		placedBets.text = player.CurrentBetTotal.ToString ();
		wallet.text = player.Wallet.ToString ();
	}

	public void OnChipValueListChanged(){
		board.ClearAllBets (player);
		player.CurrentBetTotal = 0;
		lastWin.text = "0";
		int value = 1;
		switch (chipValue.value) {
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
		board.chipValue = value;
	}
		
}
