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

public class Board : MonoBehaviour {

	public List<GameObject> betSpaces;
	public int chipValue = 1;
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
/// <summary>
/// Calulates and pays any winnings to the player
/// </summary>
/// <returns>The amount won for information purposes</returns>
/// <param name="winningNumber">Winning number.</param>
/// <param name="player">Player.</param>
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
		player.RecieveWinnings (winnings);
		return winnings;
	}
	public void LoadBets(Player player){
		//only load bets if player can afford to place all of them
		if (player.Wallet >= SavedBetsTotalValue (player)) {
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
