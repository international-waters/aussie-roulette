using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerListController : MonoBehaviour {
	GameObject listItem;
	ToggleGroup toogleGroup;
	GameManager game;
	public string selectedPlayer;
	private List<Player> playerList; 

	private List<GameObject> listItems;

	//game must be started from the start screen in order to create the peristant
	//game manager object
	void Awake () {

		if (GameObject.Find ("GameManager") == null) {
			Destroy (gameObject);
			SceneManager.LoadScene ("StartScreen");
		}
	}
		

	void Start () {
		game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		listItem = Resources.Load<GameObject> ("prefabs/PlayerListItem");
		toogleGroup = gameObject.GetComponent<ToggleGroup> ();
		listItems = new List<GameObject> ();
		playerList = game.GetSavedPlayers ();
		playerList.Reverse ();
		RefreshPlayerList ();
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPlayerListSelectionChanged(bool isclick){
		foreach (GameObject item in listItems) {
			Toggle toggle = item.GetComponent<Toggle> ();
			Image image = item.GetComponent<Image> ();
			if (toggle.isOn) {
				image.color = Color.yellow;
				Text[] labels = item.GetComponentsInChildren<Text> ();
				this.selectedPlayer = labels [1].text;
			} else {
				image.color = Color.white;
			}
		}
	}

	public void RefreshPlayerList(){
		//DeleteExistingItems ();
		GameObject newItem;
		int i = 0;
		foreach (Player player in playerList) {
			i++;
			newItem = Instantiate (listItem) as GameObject;
			Text[] labels = newItem.GetComponentsInChildren<Text> ();
			labels [0].text = i.ToString ();
			labels [1].text = player.playerName;
			labels [2].text = '$'+ player.Wallet.ToString ();
			labels [3].text = player.lastSaveTime;
			Toggle toggle = newItem.GetComponent<Toggle> ();
			toggle.onValueChanged.AddListener (OnPlayerListSelectionChanged);
			toggle.group = toogleGroup;
			newItem.transform.SetParent (gameObject.transform);
			newItem.transform.localScale = Vector3.one;
			listItems.Add (newItem);	
		}			
	}
}
