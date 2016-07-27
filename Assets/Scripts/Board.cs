using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public List<GameObject> betSpaces;
	public bool isTakingBets;
	private BetView betView;

	void Start () {
		//create the betting grid gameobjects and store them
		betSpaces = gameObject.GetComponent<GridConstructor> ().CreateBettingSpaces ();

		betView = gameObject.GetComponent<BetView> ();
		isTakingBets = true;
	}

	//this method clears all the chips owned by the player from the table and credits
	//the player for each chip removed
	public void ClearAllBets(Player player)
	{
		//iterate through the list of bet Spaces to find those that contain bets
		foreach (GameObject betSpaceObj in betSpaces) {
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			betSpace.ClearAndCreditPlayer (player);
			betView.UpdateStackCounter (betSpace);
		}
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
				betSpace.clearPlacedBets ();
			}
		}
	}

	public int CalculatePlayersTotalBet(Player player){
		int total = 0;
		foreach (GameObject betSpaceObj in betSpaces) {
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			total += betSpace.PlayersBetTotal (player);
		}
		return total;
	}

	public int PayWinnings(int winningNumber, Player player){
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
				winnings += betSpace.PayoutBets (player);
			}
		}
		return winnings;
	}
	public void LoadBets(Player player){
		//only load bets if player can afford to place all of them
		if (player.wallet >= SavedBetsTotalValue (player)) {
			foreach (GameObject betSpaceObj in betSpaces) {
				BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
				betSpace.LoadBets (player);
			}
			player.CurrentBetTotal = CalculatePlayersTotalBet (player);
		}
	}
	public void SaveAllBets(){
		foreach (GameObject betSpaceObj in betSpaces) {
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			betSpace.SaveBets ();
		}
	}

	//used to check that player can afford to repeat all saved bets before placing any.
	private int SavedBetsTotalValue(Player player){
		int total = 0;
		foreach (GameObject betSpaceObj in betSpaces) {
			BoardBetSpace betSpace = betSpaceObj.GetComponent<BoardBetSpace> ();
			total += betSpace.SavedBetsValue (player);
		}
		return total;
	}
}
