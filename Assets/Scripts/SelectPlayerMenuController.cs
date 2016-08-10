using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPlayerMenuController : MonoBehaviour {

	GameManager game;
	GameObject selectedPlayerPanel;
	Button playButton;
	Text selectedPlayerLabel;
	InputField newPlayerInput;
	PlayerListController listController;


	public void Start(){
		game = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		selectedPlayerPanel = GameObject.Find ("SelectedPlayerPanel");
		selectedPlayerLabel = GameObject.Find ("SelectedPlayerLabel").GetComponent<Text>();
		newPlayerInput = GameObject.Find ("NewPlayerInput").GetComponent<InputField>();
		playButton =  GameObject.Find ("PlayBtn").GetComponent<Button>();
		listController = GameObject.Find ("PlayerListContent").GetComponent<PlayerListController>();
	}
	public void Update(){
		if (string.IsNullOrEmpty(game.selectedPlayerName)) {
			playButton.interactable = false;
			selectedPlayerLabel.text = string.Empty;
			selectedPlayerPanel.SetActive (false);
		} else {
			playButton.interactable = true;
			selectedPlayerLabel.text = game.selectedPlayerName;
			selectedPlayerPanel.SetActive (true);
		}
	}

	public void OnPlayButtonClick(){
		SceneManager.LoadScene ("GamePlayScreen",LoadSceneMode.Single);
	}

	public void OnBackButtonClick(){
		SceneManager.LoadScene ("StartScreen",LoadSceneMode.Single);
	}

	public void OnAddNewPlayerClick(){
		//string newPlayer = newPlayerInput.text;

		if (!string.IsNullOrEmpty (newPlayerInput.text)) {
			game.player = new Player ();
			game.selectedPlayerName = newPlayerInput.text;
			game.player.playerName = newPlayerInput.text;
			game.loadGameOnStart = false;
		} else {
			game.selectedPlayerName = string.Empty;
		}
	}
		
	public void OnLoadPlayerButtonClick(){
		if (!string.IsNullOrEmpty (listController.selectedPlayer)) {
			game.selectedPlayerName = listController.selectedPlayer;
			game.loadGameOnStart = true;
		} else {
			game.selectedPlayerName = string.Empty;
			game.loadGameOnStart = false;
		}
	}
	public void OnDeletePlayerButtonClick(){
		if (!string.IsNullOrEmpty (listController.selectedPlayer)) {
			game.DeleteSavedGame (listController.selectedPlayer);
			SceneManager.LoadScene ("SelectPlayerScreen");
		}
	}

}
