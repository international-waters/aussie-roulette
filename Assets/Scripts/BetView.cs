/****************************************************************************
* Building IT Systems (CPT 111 / COSC 2635) SP2, 2016
* Game: Aussie Roulette  Group: International Waters
* Authors : Aaron Horton s3465420, David Morling s3492242
* Jeremy Cottell s3242784, Scott Nelson s3363315 , Simon Overton s3397924
*
* This class is resposible for displaying and updating the visual
* components of the betting GUI.
****************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ChipStacking {Offset, Counter};
public class BetView : MonoBehaviour {

	//Option to disable the bet position display marker
	public bool isMarkerDisplayed = true;
	public ChipStacking stackMode;

	private GameObject betMarker;
	private GameObject chipPrefab;
	private GameObject chipTextPrefab;
	private GameObject stackCounter;

	//TODO: temporary text boxes for testing
	private Text betType;
	private Text winNums;
	private Text ratio;
	private Text balance;
	private Text betTotal;

	//TODO: temporary player
	Player player;

	void Start(){
		
		stackMode = ChipStacking.Counter;
		//load game resources
		GameObject markerPrefab = Resources.Load<GameObject>("prefabs/BetMarker");
		chipPrefab = Resources.Load<GameObject>("prefabs/BetChip");
		chipTextPrefab = Resources.Load<GameObject>("prefabs/ChipText");

		//TODO: temporary code for testing
		betType = GameObject.Find ("betType").GetComponent<Text>();
		winNums = GameObject.Find ("winNums").GetComponent<Text>();
		ratio = GameObject.Find ("payoutRatio").GetComponent<Text>();
		balance = GameObject.Find ("balance").GetComponent<Text>();
		betTotal = GameObject.Find ("betTotal").GetComponent<Text>();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		balance.text = player.Wallet.ToString();

		//instantiate and hide the bet marker
		betMarker = (GameObject) Instantiate (markerPrefab,new Vector3(), Quaternion.identity);
		betMarker.SetActive (false);


	}
	/****************************************************************************
    * This method enables the bet placement marker to be deactivated
    *****************************************************************************/
	public void SetMarkerActive(bool active){
		if (isMarkerDisplayed) {
			betMarker.SetActive (active);
		}
	}
	/****************************************************************************
    * This method moves the bet placement marker to a new position
    *****************************************************************************/
	public void MoveBetMarker(Vector3 newPositon){
		if (isMarkerDisplayed) {
			betMarker.transform.position = newPositon;
		}
	}
	/****************************************************************************
    * This method places a new chip on the table
	* returns a GameObject containing a reference to this chip
    *****************************************************************************/
	//TODO: will need another parameter to indicate value / type of chip
	public GameObject PlaceChip(Vector3 position){
		GameObject bet = (GameObject)Instantiate (chipPrefab, position,Quaternion.identity);
		return bet;
	}
		

	/****************************************************************************
    * This method updates the stack counter after chips are placed or removed.
    *****************************************************************************/
	public void UpdateStackCounter(BoardBetSpace betspace){
		if (stackMode == ChipStacking.Counter) {
			int chipCount = betspace.placedChips.Count;
			if (chipCount > 0) {
				if (betspace.chipCounter == null) {
					betspace.chipCounter = (GameObject)Instantiate (chipTextPrefab,
						betspace.ChipPlacementPosition(), Quaternion.identity);
					//offset z depth so that it shows on top of chips
					betspace.chipCounter.transform.position -= new Vector3 (0, 0, 1);
				}
				betspace.chipCounter.GetComponent<TextMesh> ().text = chipCount.ToString ();

			} else if (betspace.chipCounter != null) {
				Destroy (betspace.chipCounter);
			}
		}
		//TODO: for testing
		balance.text = player.Wallet.ToString();
		betTotal.text = player.CurrentBetTotal.ToString ();

	}

	private Quaternion random2dRotation(){
		return Quaternion.Euler(0f,0f,Random.Range(0f,360f));
	}
		
	//TODO: temporary code for testing
	public void DisplayLocationInfo(BoardBetSpace betSpace){
		betType.text = betSpace.betSpaceType.Name;
		winNums.text = betSpace.winNumbersToString ();
		ratio.text = betSpace.betSpaceType.PayoutToOneRatio.ToString ();
	}
}
