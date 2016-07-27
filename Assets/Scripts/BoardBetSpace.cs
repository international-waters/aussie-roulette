using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class BoardBetSpace : MonoBehaviour {

	private BetView betView;
	public const int STACK_LIMIT = 9;
	//public BetTypeEnum betType;
	public BoardBetSpaceType betSpaceType;
	public int[] winNumbers;
	public List<GameObject> placedChips;
	public List<ClonedChip> lastBetHistory;
	public GameObject chipCounter;

	public float ColliderCenterOffsetX = 0;
	public float ColliderCenterOffsetY = 0;

	// Use this for initialization
	void Start () {
		betSpaceType = this.GetComponent<BoardBetSpaceType> ();
		betView = GameObject.Find ("RouletteTable").GetComponent<BetView> ();
		placedChips = new List<GameObject> ();
	}
	 
	public void PlaceBet(Player player, int chipValue){
		if (placedChips.Count < STACK_LIMIT) {
			if (player.TryPlaceBet (chipValue)) {
				// place the chip on the table and get a reference to it
				GameObject chipObject = betView.PlaceChip (ChipPlacementPosition());
				Chip chipDetails = chipObject.GetComponent<Chip> ();
				chipDetails.ownedByPlayer = player;
				chipDetails.value = chipValue;
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
					player.RemoveBet (lastChip.value);
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
				player.RemoveBet (chip.value);
				placedChips.RemoveAt(i);
				Destroy (chipObject);
			}
		}
	}

	//used for winning bets, pays all bets owned by a player at this postion
	public int PayoutBets(Player player){
		int payoutRatio = betSpaceType.PayoutToOneRatio;
		int winnings = 0;
		foreach (GameObject chipObj in placedChips) {
			Chip chip = chipObj.GetComponent<Chip> ();
			if (chip.ownedByPlayer == player) {
				winnings += (chip.value * payoutRatio);
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
				total += (chip.value);
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
		position += new Vector3 (ColliderCenterOffsetX, ColliderCenterOffsetY,0);
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
		lastBetHistory = new List<ClonedChip> ();
		foreach (GameObject placedChipObj in placedChips) {
			Chip placedchip = placedChipObj.GetComponent<Chip> ();
			ClonedChip clone = new ClonedChip (placedchip.value, placedchip.ownedByPlayer);
			lastBetHistory.Add (clone);
		}
	}
	public void LoadBets(Player player){
		LoadBets (lastBetHistory, player);
	}

	public int SavedBetsValue(Player player){
		int total = 0;
		foreach (ClonedChip chip in lastBetHistory) {
			if (chip.ownedByPlayer == player) {
				total += chip.value;
			}
		}
		return total;
	}

	public void LoadBets(List<ClonedChip> savedBets, Player player){
		//first remove and credit the player for any bets that remain on the table
		ClearAndCreditPlayer (player);
		foreach (ClonedChip clone in savedBets) {

			if (clone.ownedByPlayer == player) {
				PlaceBet (player, clone.value);
		
			}
		
		}
	}

}
