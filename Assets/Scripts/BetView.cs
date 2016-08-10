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
	private ChipStacking stackMode; // private because stacking mode not implemented

	private GameManager game;
	private GameObject betMarker;
	private GameObject chipPrefab;
	private GameObject chipTextPrefab;
	private GameObject winMarker;
	private GameObject stackCounter;


	void Awake(){
		
		stackMode = ChipStacking.Counter;
		//load game resources
		try{
		game = GameObject.Find("GameManager").GetComponent<GameManager>();
		}
		catch{
		}
		GameObject markerPrefab = Resources.Load<GameObject>("prefabs/BetMarker");
		chipPrefab = Resources.Load<GameObject>("prefabs/BetChip");
		chipTextPrefab = Resources.Load<GameObject>("prefabs/ChipText");
		GameObject winMarkerPrefab = Resources.Load<GameObject> ("prefabs/WinMarker");

		//instantiate and hide the bet marker
		betMarker = (GameObject) Instantiate (markerPrefab,new Vector3(), Quaternion.identity);
		betMarker.transform.parent = transform;
		betMarker.SetActive (false);

		//instantiate and hide the bet marker
		winMarker = (GameObject) Instantiate (winMarkerPrefab,new Vector3(), Quaternion.identity);
		winMarker.transform.parent = transform;
		winMarker.SetActive (false);

	}
	//Displays an marker on the win number (auto hide script is on perfab)
	public void DisplayWinMarker(Vector3 numberPosition){
		winMarker.transform.position = numberPosition;
		winMarker.SetActive (true);
	}

	public void HideWinMarker(){
		winMarker.SetActive (false);
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
	public GameObject PlaceChip(Vector3 position, bool animate = false){
		GameObject ChipObj = (GameObject)Instantiate (chipPrefab, position,Quaternion.identity);
		ChipObj.transform.parent = this.transform;
		return ChipObj;
	}
		

	/****************************************************************************
    * This method updates the stack counter after chips are placed or removed.
    *****************************************************************************/
	public void UpdateStackCounter(BoardBetSpace betspace){
		if (stackMode == ChipStacking.Counter) {
			int chipCount = betspace.placedChips.Count;
			betspace.placedChipsCount = chipCount;
			if (chipCount > 0) {
				if (betspace.chipCounterObj == null) {
					betspace.chipCounterObj = (GameObject)Instantiate (chipTextPrefab,
						betspace.ChipPlacementPosition (), Quaternion.identity);
					//offset z depth so that it shows on top of chips
					betspace.chipCounterObj.transform.position -= new Vector3 (0, 0, 1);
					betspace.chipCounterObj.transform.parent = this.transform;
				}
				betspace.chipCounterObj.SetActive (true);
				betspace.chipCounterObj.GetComponent<TextMesh> ().text = chipCount.ToString ();

			}
		}
		game.RefreshScorePanel ();
	}
		
	private Quaternion random2dRotation(){
		return Quaternion.Euler(0f,0f,Random.Range(0f,360f));
	}
		
}
