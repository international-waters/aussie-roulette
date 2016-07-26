using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string playerName;
	public int wallet;

	// Use this for initialization
	void Start () {
		
		wallet = 100;
		playerName = "Joe";
	}

	public int Balance {
		get {return wallet;}
	}
		

	public bool PlaceBet(int chipValue){
		if (wallet - chipValue >= 0) {
			wallet -= chipValue;
			return true;
		} else {
			return false;
		}
	}

	public void RemoveBet (int chipValue){
		wallet += chipValue;
	}


		

	// Update is called once per frame
	void Update () {
	
	}
}
