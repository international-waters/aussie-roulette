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

public class Player : MonoBehaviour {

	public string playerName;
	private int wallet;
	private int currentBetTotal;

	// Use this for initialization
	void Start () {
		
		wallet = 100;
		playerName = "Joe";
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
	}
		

	// Update is called once per frame
	void Update () {
	
	}
}
