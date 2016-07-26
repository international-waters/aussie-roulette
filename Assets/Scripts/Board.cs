using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public List<GameObject> betSpaces;
	public bool isTakingBets;

	void Start () {
		//create the betting grid gameobjects and store them
		betSpaces = gameObject.GetComponent<GridConstructor> ().CreateBettingSpaces ();
		isTakingBets = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
