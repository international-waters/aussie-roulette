using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string playerName;
	public int wallet;
	private int currentBetTotal;

	// Use this for initialization
	void Start () {
		
		wallet = 100;
		playerName = "Joe";
	}

	public int Balance {
		get {return wallet;}
	}

	public int CurrentBetTotal{
		get{return currentBetTotal;}
		set{currentBetTotal = value;}
	}
		

	public bool TryPlaceBet(int chipValue){
		if (wallet - chipValue >= 0) {
			wallet -= chipValue;
			currentBetTotal += chipValue;

			return true;
		} else {
			return false;
		}
	}

	public void RemoveBet (int chipValue){
		wallet += chipValue;
		currentBetTotal -= chipValue;
	}


		

	// Update is called once per frame
	void Update () {
	
	}
}
