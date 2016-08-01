using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour {
	public static GameManager instance { get; private set;}

	public Player player;

	//game screen lables
	public Text lastWin_lbl;
	public Text placedBetsTotal_lbl;
	public Text score_lbl;

	private GameObject tableObj;

	void Awake () {
		KeepAlive ();
		player = new Player ();
	}

	private void KeepAlive(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}


	
	// Update is called once per frame
	void Update () {
	
	}

	public void RefreshScorePanel(){
		if (SceneManager.GetActiveScene ().name == "GamePlayScreen") {
			LoadGameScreenResouces ();
			lastWin_lbl.text = player.LastWin.ToString();
			placedBetsTotal_lbl.text = player.CurrentBetTotal.ToString();
			score_lbl.text = player.Wallet.ToString();
		}
	}

	public void LoadGameScreenResouces(){
		if (lastWin_lbl == null) lastWin_lbl = GameObject.Find("lastWin").GetComponent<Text>();
		if (placedBetsTotal_lbl == null) placedBetsTotal_lbl = GameObject.Find("betTotal").GetComponent<Text>();
		if (score_lbl == null) score_lbl = GameObject.Find("score").GetComponent<Text>();
	}


	//this will need to be changed to use playerprefs as it won't work with webplayer
	public void SaveGame(Player player, Board board){
		string savePath = Application.persistentDataPath;

		board.ClearStoredChipHistory ();

		//save all the bets player has on the table to bet history (used to repeat bets)
		board.StoreAllPlacedChipInfo ();

		//clear the board and credit the player, the players actual score includes the value
		//of all chips they have on the table as well as their wallet balance.
		board.ClearAllBets(player);

		//create or overwrite the players save file
		StreamWriter writer = new StreamWriter (string.Format ("{0}\\{1}.save", savePath, player.playerName));
		//write the players details
		writer.WriteLine(JsonUtility.ToJson (player));

		//write all saved chip information
		foreach(ChipInfo chip in board.savedChips){
			writer.WriteLine(JsonUtility.ToJson (chip));
		}
		writer.Close ();
	}

	//this will need to be changed to use playerprefs as it won't work with webplayer
	public Player LoadGame(string playerName, Board board){
		string savePath = Application.persistentDataPath;
		Player player;
		board.ClearAllBets ();
		StreamReader reader = null;
		try{
			reader = new StreamReader (string.Format ("{0}\\{1}.save", savePath,playerName));
			player = JsonUtility.FromJson<Player> (reader.ReadLine());

			List<ChipInfo> savedChips = new List<ChipInfo> ();
			string line;
			while ((line = reader.ReadLine ()) != null) {
				ChipInfo chip = JsonUtility.FromJson<ChipInfo> (line);
				savedChips.Add (chip);
			}
			board.savedChips = savedChips;
			board.PlaceAllStoredChips(player);
		}
		catch (IOException){
			//oops file not found probably, good thing this is just a prototype!
			//start a new game with "playerName", hopefully know one will notice. [wink]
			board.ClearAllBets();
			board.ClearStoredChipHistory ();
			player = new Player();
			player.playerName = playerName;
		}
		finally{
			reader.Close ();
		}
		return player;
	}

	public bool SaveHighScore(Player player, Board board){
		return SaveHighScore(player.playerName,player.Wallet, board);
	}
	public bool SaveHighScore(string playerName, int score, Board board){
		//not yet implemented
		return false;
	}
}
	
