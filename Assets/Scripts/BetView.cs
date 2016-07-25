using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ChipStacking {Offset, Counter};
public class BetView : MonoBehaviour {

	//Option to disable the bet position display marker
	public bool isMarkerDisplayed = true;

	private GameObject betMarker;
	private GameObject chipPrefab;
	private GameObject chipTextPrefab;
	private GameObject stackCounter;

	//TODO: temporary text boxes for testing
	private Text betType;
	private Text winNums;
	private Text ratio;
	private Text balance;

	//TODO: temporary player
	Player player;

	void Start(){

		//load game resources
		GameObject markerPrefab = Resources.Load<GameObject>("prefabs/BetMarker");
		chipPrefab = Resources.Load<GameObject>("prefabs/BetChip");
		chipTextPrefab = Resources.Load<GameObject>("prefabs/ChipText");

		//TODO: temporary code for testing
		betType = GameObject.Find ("betType").GetComponent<Text>();
		winNums = GameObject.Find ("winNums").GetComponent<Text>();
		ratio = GameObject.Find ("payoutRatio").GetComponent<Text>();
		balance = GameObject.Find ("balance").GetComponent<Text>();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		balance.text = player.wallet.ToString();

		//instantiate and hide the bet marker
		betMarker = (GameObject) Instantiate (markerPrefab,new Vector3(), Quaternion.identity);
		betMarker.SetActive (false);


	}

	public void SetMarkerActive(bool active){
		if (isMarkerDisplayed) {
			betMarker.SetActive (active);
		}
	}
	public void MoveBetMarker(Vector3 newPositon){
		if (isMarkerDisplayed) {
			betMarker.transform.position = newPositon;
		}
	}

	public GameObject PlaceChip(Vector3 position){
		GameObject bet = (GameObject)Instantiate (chipPrefab, position, Quaternion.identity);
		return bet;
	}

	public void UpdateStackCounter(BoardBetSpace betspace){
		int chipCount = betspace.placedChips.Count;
		if (chipCount > 0 ){
			if (betspace.chipCounter == null) {
				betspace.chipCounter = (GameObject)Instantiate (chipTextPrefab,
					betspace.ChipPlacementPosition (), Quaternion.identity);
			}
			betspace.chipCounter.GetComponent<TextMesh>().text = chipCount.ToString ();

		}else if (betspace.chipCounter != null){
			Destroy (betspace.chipCounter);
		}
		//TODO: for testing
		balance.text = player.wallet.ToString();

	}
		
	//TODO: temporary code for testing
	public void DisplayLocationInfo(BoardBetSpace betSpace){
		betType.text = betSpace.betSpaceType.Name;
		winNums.text = betSpace.winNumbersToString ();
		ratio.text = betSpace.betSpaceType.PayoutToOneRatio.ToString ();
	}
}
