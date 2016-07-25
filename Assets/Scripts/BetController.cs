using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class BetController : MonoBehaviour {

	private BetView betView;
	private BoardBetSpace betSpace;
	private Board board;
	private Player player;
	public int betCount;
	bool isValidBetPosition = false;

	//TODO set chip value logic
	private int chipValue = 5;

	void Start(){
		
		betView = GameObject.Find ("RouletteTable").GetComponent<BetView> ();
		board =  GameObject.Find ("RouletteTable").GetComponent<Board> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		betSpace = gameObject.GetComponent<BoardBetSpace> ();

	}


	void OnMouseEnter(){

		//bets can only be placed while cursor is over a valid bet position 
		isValidBetPosition = true;
		betView.SetMarkerActive (true);

		betView.MoveBetMarker (betSpace.ChipPlacementPosition());

		//TODO: this is just used for dev / testing
		betView.DisplayLocationInfo (betSpace);

	
	}

	void OnMouseExit(){
		isValidBetPosition = false;
		betView.SetMarkerActive (false);

	}

	void OnMouseDown(){
		if (isValidBetPosition && board.isTakingBets) { {
				betSpace.PlaceBet (player, chipValue);
			}
		}
	}

	/* This method is used to remove bets from the table. There is
	 * no OnMouseDown() for right click in unity so this method is a work around */
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (1) && isValidBetPosition && board.isTakingBets) {
			betSpace.RemoveBet (player);
		}
	}
}
