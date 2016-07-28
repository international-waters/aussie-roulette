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
	//public BetTypeEnum betType;
	public BoardBetSpaceType betSpaceType;
	public int[] winNumbers;
	public List<GameObject> placedChips;
	public List<SavedChip> lastBetHistory;
	public GameObject chipCounter;

	public float ColliderCenterOffsetX = 0;
	public float ColliderCenterOffsetY = 0;

	// Use this for initialization
	void Start () {
		betSpaceType = this.GetComponent<BoardBetSpaceType> ();
		betView = GameObject.Find ("RouletteTable").GetComponent<BetView> ();
		placedChips = new List<GameObject> ();
		lastBetHistory = new List<SavedChip> ();
	}
	 
	public void PlaceBet(Player player, int chipValue){
		if (placedChips.Count < STACK_LIMIT) {
			if (player.TryBuyChip (chipValue)) {
				// place the chip on the table and get a reference to it
				GameObject chipObject = betView.PlaceChip (ChipPlacementPosition());
				Chip chipDetails = chipObject.GetComponent<Chip> ();
				chipDetails.ownedByPlayer = player;
				chipDetails.Value = chipValue;
				placedChips.Add (chipObject);
				betView.UpdateStackCounter (this);
			}
		}
	}

	public void RemoveBet(Player player){
		if (placedChips.Count > 0) {
			Chip lastChip; 
			for (int i = placedChips.Count-1; i >= 0; i--) {
				lastChip = placedChips [i].GetComponent<Chip> ();
				if (lastChip.ownedByPlayer == player) {
					GameObject chipObject = placedChips [i];
					player.SellChip (lastChip.Value);
					placedChips.RemoveAt (i);
					Destroy (chipObject);
					betView.UpdateStackCounter (this);
					break;
				}
			}
		}
	}
	//used for losing bets, clears all bets on the betSpace without crediting players
	public void clearPlacedBets(){
		for (int i = placedChips.Count-1; i >= 0; i--){
			Destroy (placedChips [i]);
		}
		placedChips = new List<GameObject> ();
		betView.UpdateStackCounter (this);
	}

	public void ClearAndCreditPlayer(Player player){
		for (int i = placedChips.Count-1; i >= 0; i--)
		{
			Chip chip = placedChips[i].GetComponent<Chip> ();
			if (chip.ownedByPlayer == player) {
				GameObject chipObject = placedChips [i];
				player.SellChip (chip.Value);
				placedChips.RemoveAt(i);
				Destroy (chipObject);
			}
		}
	//	betView.UpdateStackCounter (this);
	}

	//used for winning bets, pays all bets owned by a player at this postion
	public int PayoutBets(Player player){
		int payoutRatio = betSpaceType.PayoutToOneRatio;
		int winnings = 0;
		foreach (GameObject chipObj in placedChips) {
			Chip chip = chipObj.GetComponent<Chip> ();
			if (chip.ownedByPlayer == player) {
				winnings += (chip.Value * payoutRatio);
			}
		}
		return winnings;
	}

	//not tested
	public int PlayersBetTotal(Player player){
		int total = 0;
		foreach (GameObject chipObj in placedChips) {
			Chip chip = chipObj.GetComponent<Chip> ();
			if (chip.ownedByPlayer == player) {
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
	public string winNumbersToString()
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
	public void SaveBets(){
		lastBetHistory = new List<SavedChip> ();
		foreach (GameObject placedChipObj in placedChips) {
			Chip placedchip = placedChipObj.GetComponent<Chip> ();
			SavedChip savedChip = new SavedChip (placedchip.Value, placedchip.ownedByPlayer);
			lastBetHistory.Add (savedChip);
		}
	}
	public void LoadBets(Player player){
		LoadBets (lastBetHistory, player);
	}

	public int SavedBetsValue(Player player){
		int total = 0;
		foreach (SavedChip chip in lastBetHistory) {
			if (chip.ownedByPlayer == player) {
				total += chip.value;
			}
		}
		return total;
	}

	public void LoadBets(List<SavedChip> savedBets, Player player){
		//first remove and credit the player for any bets that remain on the table
		ClearAndCreditPlayer (player);
		foreach (SavedChip savedChip in savedBets) {

			if (savedChip.ownedByPlayer == player) {
				PlaceBet (player, savedChip.value);
		
			}
		
		}
		betView.UpdateStackCounter (this);
	}

}
