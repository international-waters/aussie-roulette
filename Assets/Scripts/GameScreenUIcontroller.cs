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

	void Start(){
		board = GameObject.Find ("RouletteTable").GetComponent<Board> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		winNumber = GameObject.Find("winNumberDropdown").GetComponent<Dropdown>();
		lastWin = GameObject.Find("winAmount").GetComponent<Text>();
		placedBets = GameObject.Find("betTotal").GetComponent<Text>();
		wallet = GameObject.Find("balance").GetComponent<Text>();
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
		player.wallet += winnings;
		player.CurrentBetTotal = board.CalculatePlayersTotalBet (player);
		placedBets.text = player.CurrentBetTotal.ToString ();
		wallet.text = player.wallet.ToString ();
	}

	public void OnLoadLastBetsButtonClick(){
		board.LoadBets (player);
		placedBets.text = player.CurrentBetTotal.ToString ();
		wallet.text = player.wallet.ToString ();
	}
}
