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
using System.Text;

public class BoardBetSpace : MonoBehaviour {

	private BetView betView;
	private const int Z_CHIP_POS = -3;
	public const int STACK_LIMIT = 9;

	public BoardBetSpaceType betSpaceType;
	public int[] winNumbers;
	private int id = -1;
	public List<GameObject> placedChips;
	public GameObject chipCounterObj;
	public int placedChipsCount;

	public float ColliderCenterOffsetX = 0;
	public float ColliderCenterOffsetY = 0;
	public int visibleID;

	//ID number is used when saving / loading games
	public int ID {
		get{return id; }
		//work around to ensure id number is only set once (can't be changed after being set) 
		set{
			if (id == -1){
				id = value;
				visibleID = id;
			}
		}
	}
		
	void Start () {
		betSpaceType = this.GetComponent<BoardBetSpaceType> ();
		betView = GameObject.Find ("RouletteTable").GetComponent<BetView> ();
		placedChips = new List<GameObject> ();
	}

	//remove stack counter labels when there are no chips left on the betting spot
	public void LateUpdate(){
		if (placedChips.Count < 1) {
			Destroy (chipCounterObj);
		}
	}
		
	private void PlaceChip(Player player, string playerName,bool isPayedFor,int chipValue,bool animate = false){
		if (placedChips.Count < STACK_LIMIT) {
			bool isChipPayedFor = false;

			//if the chip is not payed for or is being loaded and belongs to the current player
			if (!isPayedFor && player.playerName == playerName) {
				//try and buy a chip, if player cannot afford it no bet is placed.
				isChipPayedFor = player.TryBuyChip (chipValue);
			//if chip does not belong to the current player it is just placed on the board.
			}else{
				isChipPayedFor = true;
			}
			if (isChipPayedFor) {
				// place the chip GameObject on the table and get a reference to it
				GameObject chipObject = betView.PlaceChip (ChipPlacementPosition (),animate);
				Chip chipDetails = chipObject.GetComponent<Chip> ();
				if (player != null) {
					chipDetails.ownedByPlayer = player.playerName;
				} else {
					chipDetails.ownedByPlayer = playerName;
				}
				chipDetails.Value = chipValue;
				chipDetails.betSpaceId = this.ID;
				placedChips.Add (chipObject);
			}
			betView.UpdateStackCounter (this);
		}
	}

	public void PlaceChip(Player currentPlayer, int chipValue){
		PlaceChip (currentPlayer,currentPlayer.playerName,false,chipValue);
	}
		

	public void PlaceChip(Player currentPlayer, ChipInfo chipInfo, bool animate){
		PlaceChip (currentPlayer, chipInfo.ownedByPlayer,false,chipInfo.value, animate);
	}
	public void RemoveLastPlacedChip(Player player, bool animate = false){
		if (placedChips.Count > 0) {
			Chip lastChip; 
			for (int i = placedChips.Count-1; i >= 0; i--) {
				lastChip = placedChips [i].GetComponent<Chip> ();
				if (lastChip.ownedByPlayer == player.playerName) {
					GameObject chipObject = placedChips [i];
					player.SellChip (lastChip.Value);
					placedChips.RemoveAt (i);
					if (animate) {
						lastChip.ClearChip = ChipMove.ToPlayer;
					} else {
						Destroy (chipObject);
					}
					betView.UpdateStackCounter (this);
					break;
				}
			}
		}
	}
	//Remove all chips, no player is credited, used for losing numbers and for clearing the board
	public void RemoveAllChips(ChipMove chipMove = ChipMove.Disabled){
		bool animate = (chipMove != ChipMove.Disabled);
		for (int i = placedChips.Count-1; i >= 0; i--){
			if (!animate) {
				Destroy (placedChips [i]);
			} else {
				Chip chip = placedChips [i].GetComponent<Chip> ();
				chip.ClearChip = chipMove;
			}
		}
		placedChips = new List<GameObject> ();
		betView.UpdateStackCounter (this);
	}

	//Remove all chips, specifed player is credited
	//TODO: will need a player[] version if more than one player is implemented
	public void RemoveAllChips(Player playerToCredit, ChipMove chipMove = ChipMove.Disabled){
		bool animate = (chipMove != ChipMove.Disabled);
		for (int i = placedChips.Count-1; i >= 0; i--)
		{
			Chip chip = placedChips[i].GetComponent<Chip> ();
			GameObject chipObject = placedChips [i];
			if (chip.ownedByPlayer == playerToCredit.playerName) {
				playerToCredit.SellChip (chip.Value);
				if (animate) {
					chip.ClearChip = chipMove;
				} else {
					Destroy (chipObject);
				}
			} else {
				Destroy (chipObject);
			}
		}
		placedChips = new List<GameObject> ();
		betView.UpdateStackCounter (this);
	}

	//calculate winnings to be payed to the player
	public int CalculateWinnings(Player player){
		int payoutRatio = betSpaceType.PayoutToOneRatio;
		int winnings = 0;
		foreach (GameObject chipObj in placedChips) {
			Chip chip = chipObj.GetComponent<Chip> ();
			if (chip.ownedByPlayer == player.playerName) {
				winnings += (chip.Value * payoutRatio);
			}
		}
		return winnings;
	}
		
	public int PlacedChipsTotalValue(Player player){
		int total = 0;
		foreach (GameObject chipObj in placedChips) {
			Chip chip = chipObj.GetComponent<Chip> ();
			if (chip.ownedByPlayer == player.playerName) {
				total += (chip.Value);
			}
		}
		return total;
	}
		

	/* For some outside bet spaces the the center of the collider is not where
	 * the chip should be placed*/
	public Vector3 ChipPlacementPosition(){

		//get the position of this bet space collider 
		Vector3 position = this.gameObject.transform.position;
	
		//add offset
		position += new Vector3 (ColliderCenterOffsetX, ColliderCenterOffsetY,Z_CHIP_POS);
		return position;
	}

	//Utiliy method to write the winning numbers array as a comma delimited string
	public string WinNumbersToString()
	{
		var sb = new StringBuilder();
		int counter = 0;
		foreach (int i in winNumbers) {
			counter++;
			sb.Append(i);
			if (counter < winNumbers.Length) {
				sb.Append (',');
			}
		}
		return sb.ToString ();
	}

}
