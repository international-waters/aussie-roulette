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
	public GameObject chipCounter;

	public float ColliderCenterOffsetX = 0;
	public float ColliderCenterOffsetY = 0;

	// Use this for initialization
	void Start () {
		betSpaceType = this.GetComponent<BoardBetSpaceType> ();
		betView = GameObject.Find ("RouletteTable").GetComponent<BetView> ();
	}
	 
	public void PlaceBet(Player player, int chipValue){
		if (placedChips.Count < STACK_LIMIT) {
			if (player.PlaceBet (chipValue)) {
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
		

	/* For some bet outside bet spaces the the center of the collider is not where
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
}
