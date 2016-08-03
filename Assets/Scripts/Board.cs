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
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Board : MonoBehaviour {

	public List<GameObject> betSpaces;
	public List<ChipInfo> savedChips;
	public int SelectedChipValue = 1;
	public bool isTakingBets;
	private GameManager game;


	public static Board instance { get; private set;}
	void Awake () {
		//game must be started from the start screen in order to create the peristant
		//game manager object
		if (GameObject.Find ("GameManager") == null) {
			Destroy (gameObject);
			SceneManager.LoadScene ("StartScreen");
		} else {
			game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		}
		//singleton pattern for keeping alive accross scenes
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}


	void Start () {

		//create the betting grid gameobjects and store them
		betSpaces = gameObject.GetComponent<GridConstructor> ().CreateBettingSpaces ();
		isTakingBets = true;
	}
		
	void OnLevelWasLoaded(){
		//Toggle table, bets etc active if gamescreen is loaded otherwise deactivate
		bool isThisLevelLoaded = (SceneManager.GetActiveScene ().name == "GamePlayScreen") ? true : false;
		foreach (Transform child in transform) {
			if (child.name != "BetMarker(Clone)") {
				child.gameObject.SetActive (isThisLevelLoaded);
			}
		}
		//this should allow winning number to be processed upon return for the animation scene
		//(not tested)
		if (isThisLevelLoaded) {
			if (game.winningNumber >= 0 && game.winningNumber < 37) {
				game.ProcessWinningNumber (this);
				Text winLabel = GameObject.Find ("winningNumber").GetComponent<Text> ();
				winLabel.text = game.winningNumber.ToString ();
			}
		}
	}



	//this method clears all the chips owned by the player from the table and credits
	//the player for each chip removed
	public void ClearAllBets(Player playerToCredit)
	{
		//iterate through the list of bet Spaces to find those that contain bets
		foreach (GameObject betSpaceObj in betSpaces) {
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			if (playerToCredit != null) {
				betSpace.RemoveAllChips (playerToCredit);
				playerToCredit.CurrentBetTotal = 0;
			} else {
				betSpace.RemoveAllChips ();
			}
		}
	}

	public void ClearAllBets(){
		ClearAllBets (null);
	}

	public void ClearLosingBets(int winningNumber){
		foreach (GameObject betSpaceObj in betSpaces)
		{
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			int[] winners = betSpace.winNumbers;
			bool winningSpace = false;
			foreach (int number in winners) {
				if (number == winningNumber) {
					winningSpace = true;
					break;
				}
			}
			if (!winningSpace) {
				betSpace.RemoveAllChips ();
			}
		}
	}

	public int CalculatePlayersTotalBet(Player player){
		int total = 0;
		foreach (GameObject betSpaceObj in betSpaces) {
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			total += betSpace.PlacedChipsTotalValue (player);
		}
		return total;
	}

	//used to check that player can afford to repeat all saved bets before placing any.
	//the newChipValue field is used when changing all chips to a different value
	//O means no change
	private int CalcTotalSavedBetValue(Player player,int newChipValue = 0){
		int total = 0;
		if (savedChips != null){
			foreach (ChipInfo chip in savedChips) {
				if (chip.ownedByPlayer == player.playerName) {
					if (newChipValue == 0) {
						total += chip.value;
					} else {
						total += newChipValue;
					}
				}
			}
		}
		return total;
	}
/// <summary>
/// Calulates and pays any winnings to the player
/// </summary>
/// <returns>The amount won for information purposes</returns>
/// <param name="winningNumber">Winning number.</param>
/// <param name="player">Player.</param>
	public void PayoutWinnings(int winningNumber, Player player){
		int winnings = 0;
		foreach (GameObject betSpaceObj in betSpaces)
		{
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			int[] winners = betSpace.winNumbers;
			bool winningSpace = false;
			foreach (int number in winners) {
				if (number == winningNumber) {
					winningSpace = true;
					break;
				}
			}
			if (winningSpace) {
				winnings += betSpace.CalculateWinnings (player);
			}
		}
		player.RecieveWinnings (winnings);;
	}

	public void PlaceAllStoredChips(Player CurrentPlayer, int newChipValue = 0){
		//clear any bets from the table and credit player
		ClearAllBets(CurrentPlayer);
		//check that player can afford to place all of the bets
		if (CurrentPlayer.Wallet >= this.CalcTotalSavedBetValue (CurrentPlayer, newChipValue)) {

			foreach (ChipInfo chip in savedChips) {
				BoardBetSpace betSpace = betSpaces [chip.betSpaceId]
				.GetComponent<BoardBetSpace> ();
				if (newChipValue == 0) {
					betSpace.PlaceChip (CurrentPlayer, chip.value);
				} else {
					betSpace.PlaceChip (CurrentPlayer, newChipValue);
				}
			}
		}
	}

	public void ClearStoredChipHistory(){
		savedChips = new List<ChipInfo> ();
	}

	public void StoreAllPlacedChipInfo(){
		savedChips = new List<ChipInfo> ();
		foreach (GameObject betSpaceObj in betSpaces) {
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			foreach (GameObject placedChipObj in betSpace.placedChips) {
				Chip placedChip = placedChipObj.GetComponent<Chip> ();
				savedChips.Add (placedChip.ToChipInfo ());
			}
		}
	}

}
