/****************************************************************************
* Building IT Systems (CPT 111 / COSC 2635) SP2, 2016
* Game: Aussie Roulette  Group: International Waters
* Authors : Aaron Horton s3465420, David Morling s3492242
* Jeremy Cottell s3242784, Scott Nelson s3363315 , Simon Overton s3397924
*
* This class contains the event handlers for the betting user interface
****************************************************************************/

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
		
		//Load required resourses
		betView = GameObject.Find ("RouletteTable").GetComponent<BetView> ();
		board =  GameObject.Find ("RouletteTable").GetComponent<Board> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		betSpace = gameObject.GetComponent<BoardBetSpace> ();

	}
	/****************************************************************************
    * This method determines if the cursor is over a valid bet postion on the 
    * roulette table and enables bet placement. The bet placement marker is set
	* active and will display if enabled.
    *****************************************************************************/
	void OnMouseEnter(){

		isValidBetPosition = true;
		betView.SetMarkerActive (true);
		betView.MoveBetMarker (betSpace.ChipPlacementPosition());

		//TODO: this is just used for dev / testing
		betView.DisplayLocationInfo (betSpace);

	
	}
	/****************************************************************************
    * This method detects when the mouse leaves a valid Bet Space area and
	* disables bet placement. The bet placement marker is also deactivated.
    *****************************************************************************/
	void OnMouseExit(){
		isValidBetPosition = false;
		betView.SetMarkerActive (false);

	}
	
	/****************************************************************************
    * If the mouse cursor is over a valid Bet Space area and the left mouse button
	* is pressed a chip is placed at that location if the board is taking bets.
    *****************************************************************************/
	void OnMouseDown(){
		if (isValidBetPosition && board.isTakingBets) { {
				betSpace.PlaceBet (player, chipValue);
			}
		}
	}

	/****************************************************************************
    * If the mouse cursor is over a Bet Space area that has chips placed and the
	* right mouse button is pressed a chip will be removed if the board is taking
	* bets. There is no OnMouseDown() for right click in unity so this method is 
	* a work around
    *****************************************************************************/
	void OnMouseOver(){
		if (Input.GetMouseButtonDown (1) && isValidBetPosition && board.isTakingBets) {
			betSpace.RemoveBet (player);
		}
	}
}
