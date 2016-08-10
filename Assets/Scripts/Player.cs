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
using System;

[Serializable]
public class Player{

	public string playerName;
	public int wallet;
	private int currentBetTotal;

	public int LastWin{ get; set;}
	public const int STARTING_BALANCE = 100;
	public string lastSaveTime;


	public Player(){
		this.playerName = "Anonymous";
		this.wallet = STARTING_BALANCE;
	}

	public Player (string playerName, int wallet = STARTING_BALANCE) {
		this.playerName = playerName;
		this.wallet = wallet;
	}
		
		
	public int Wallet {
		get {return wallet;}
	}

	public int CurrentBetTotal{
		get{return currentBetTotal;}
		set{currentBetTotal = value;}
	}
		

	public bool TryBuyChip(int chipValue){
		if (wallet - chipValue >= 0) {
			wallet -= chipValue;
			currentBetTotal += chipValue;
			return true;
		} else {
			return false;
		}
	}

	public void SellChip (int chipValue){
		wallet += chipValue;
		currentBetTotal -= chipValue;
	}

	public void RecieveWinnings(int winnings){
		wallet += winnings;
		LastWin = winnings;
	}
		

}
