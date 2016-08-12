using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Globalization;

public class GameManager : MonoBehaviour {
	public static GameManager instance { get; private set;}

	public Player player;

	//game screen labels
	public Text lastWin_lbl;
	public Text placedBetsTotal_lbl;
	public Text score_lbl;
	private Toggle leaveBetsToggle;

	public string selectedPlayerName;

	private Board board;
	private BetView betView;
	private GameObject winMarker;
	private const int MAX_NUMBER_HIGHSCORES = 5;

	//Use this to set the winning number from a spin in another scene
	public int winNumberFlag = -1;

	//Seconds to wait after spin before clearing losing numbers and processing
	public float winDelaySeconds = 2f;
	//marker stays on the board a bit longer after losing chips are cleared
	public float winMarkerExtraDelay = 2f;
	private GameObject tableObj;
	public bool cheatMode = false;

	public bool loadGameOnStart = false;

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

	IEnumerator HideWinMarker(){
		yield return new WaitForSeconds (winMarkerExtraDelay);
		betView.HideWinMarker ();
		board.isTakingBets = true;
		this.ProcessRepeatBets ();
	}

	IEnumerator ProcessWinAfterDelay(int winNumber, BetView betView){

		yield return new WaitForSeconds (winDelaySeconds);
		board.ClearLosingBets (winNumber, true);
		board.PayoutWinnings (winNumber, player);
		player.CurrentBetTotal = board.CalculatePlayersTotalBet (player);
		RefreshScorePanel ();
		StartCoroutine (HideWinMarker ());
		//reset flag to default state
		this.winNumberFlag = -1;

	}

	public void ProcessWinNumber(){
		ProcessWinNumber (this.winNumberFlag);
	}

	public void ProcessWinNumber(int winNumber){
		if (board == null) {
			board = GameObject.Find ("RouletteTable").GetComponent<Board> ();
		}
		if (betView == null) {
			betView = GameObject.Find ("RouletteTable").GetComponent<BetView> ();
		}
		//record chip placement for repeat bet option
		board.StoreAllPlacedChipInfo ();
		board.isTakingBets = false;
		board.DisplayWinMarker (winNumber,betView);
		StartCoroutine (ProcessWinAfterDelay (winNumber, betView));
	}


	public void RefreshScorePanel(){
		if (SceneManager.GetActiveScene ().name == "GamePlayScreen") {
			LoadGameScreenResouces ();
			lastWin_lbl.text = player.LastWin.ToString();
			placedBetsTotal_lbl.text = player.CurrentBetTotal.ToString();
			score_lbl.text = player.Wallet.ToString();
		}
	}

	public void ProcessRepeatBets(){
		if (leaveBetsToggle == null) {
			leaveBetsToggle = GameObject.Find ("LeaveBetsToggle").GetComponent<Toggle> ();
		}
		if (board.isRepeatingBets) {
			//turns option off if player cannot afford repeat bets
			board.isRepeatingBets = board.PlaceAllStoredChips (player);
			leaveBetsToggle.isOn = board.isRepeatingBets;
		}
	}

	public void LoadGameScreenResouces(){
		if (lastWin_lbl == null) lastWin_lbl = GameObject.Find("lastWin").GetComponent<Text>();
		if (placedBetsTotal_lbl == null) placedBetsTotal_lbl = GameObject.Find("betTotal").GetComponent<Text>();
		if (score_lbl == null) score_lbl = GameObject.Find("score").GetComponent<Text>();
	}

	public void DeleteSavedGame(string nameToDelete){
		string saveGamesList = PlayerPrefs.GetString ("SavedGamesList");
		int index = saveGamesList.IndexOf (nameToDelete);
		int length = (nameToDelete.Length + 1);
		if (index >= 0) {
			string newList = saveGamesList.Remove (index, length);
			PlayerPrefs.SetString ("SavedGamesList", newList);
			PlayerPrefs.DeleteKey (nameToDelete);
		}
	}

	public List<Player> GetSavedPlayers(){
		List<Player> players = new List<Player> ();
		foreach (string playerName in SavedGameList()) {
			string playerData = PlayerPrefs.GetString (playerName);
			//player not found, return a new player with name = playerName
			Player tempPlayer;
			if (playerData == string.Empty) {
				tempPlayer = new Player (playerName);
			} else {
				StringReader reader = new StringReader (playerData);
				tempPlayer = JsonUtility.FromJson<Player> (reader.ReadLine ());
				reader.Close ();
			}
			players.Add (tempPlayer);
		}
		return players;
	}


	public string[] SavedGameList(){
		char[] sep = new char[1]{ ',' };//just to get remove empty entries working
		return PlayerPrefs.GetString ("SavedGamesList").Split (
			sep,System.StringSplitOptions.RemoveEmptyEntries);
	}

	public void SaveGame (Player player, Board board, bool usePlayerPrefs = true){
		if (!usePlayerPrefs){
			this.SaveGameToFile (player, board);
			return;
		}
		if (board == null) {
			board = GameObject.Find ("RouletteTable").GetComponent<Board> ();
		}
		DeleteSavedGame (player.playerName);

		//string date = DateTime.Now.ToString("G",CultureInfo.CreateSpecificCulture("en-au"));
		string date = DateTime.Now.ToString(@"dd/MM/yyyy HH:mm tt");
		//string date = "";
		player.lastSaveTime = date;
		board.ClearStoredChipHistory ();

		//save all the bets player has on the table to bet history (used to repeat bets)
		board.StoreAllPlacedChipInfo ();

		//clear the board and credit the player, the players actual score includes the value
		//of all chips they have on the table as well as their wallet balance.
		board.ClearAllBets(player);
		StringBuilder sb = new StringBuilder ();
		string temp = JsonUtility.ToJson (player);
		sb.AppendLine (temp);
		//sb.AppendLine (JsonUtility.ToJson(player));

		//write all saved chip information
		foreach(ChipInfo chip in board.savedChips){
			sb.AppendLine(JsonUtility.ToJson (chip));
		}

		PlayerPrefs.SetString (player.playerName, sb.ToString ());
		string savedGamesList = PlayerPrefs.GetString ("SavedGamesList");
		PlayerPrefs.SetString ("SavedGamesList",savedGamesList + player.playerName + ",");

		SaveHighScore (player, board);
	}

	//saves game to the file system
	private void SaveGameToFile(Player player, Board board){
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

	public Player LoadGame(string playerName, Board board, bool usePlayerPrefs = true){

		if (!usePlayerPrefs){
			return LoadGameFromFile (playerName, board);
		}

		if (betView == null) {
			betView = GameObject.Find ("RouletteTable").GetComponent<BetView> ();
		}

		//Create default list of players for testing
		if (PlayerPrefs.GetString ("SavedGamesList") == string.Empty) {
			PlayerPrefs.SetString ("SavedGamesList", "Simon,Scott,Dave,Aaron,Jeremy,"); 
		}
		board.ClearAllBets ();
		Player tempPlayer;
		string playerData = PlayerPrefs.GetString (playerName);
		//player not found, return a new player with name = playerName
		if (playerData == string.Empty) {
			tempPlayer = new Player (playerName);
		} else {
			StringReader reader = new StringReader (playerData);
			List<ChipInfo> savedChips = new List<ChipInfo> ();
			tempPlayer = JsonUtility.FromJson<Player> (reader.ReadLine());
			string line;
			while ((line = reader.ReadLine ()) != null) {
				ChipInfo chip = JsonUtility.FromJson<ChipInfo> (line);
				savedChips.Add (chip);
			}
			reader.Close ();
			board.savedChips = savedChips;
			board.PlaceAllStoredChips(tempPlayer);
		}
		//set the selected board value to match the loaded chips value
		if (board.savedChips.Count > 0) {
			board.SelectedChipValue = board.savedChips [1].value;
		}
		board.ClearStoredChipHistory ();
		return tempPlayer;
	}
		


	//load game from file system
	private Player LoadGameFromFile(string playerName, Board board){
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
			player = new Player();
			player.playerName = playerName;
		}
		finally{
			reader.Close ();
		}
		board.ClearStoredChipHistory ();
		return player;
	}

	public void SaveHighScore(Player player, Board board){
		 SaveHighScore(player.playerName,player.Wallet, board);
	}

	//checks players score against the highscores and returns the postion on the
	//ladder if the player achieved an new high score, otherwise 0 is returned
	public void SaveHighScore(string playerName, int score, Board board){
		List<HighScore> list = LoadHighScores ();
		HighScore playerScore = new HighScore (playerName, score);
		bool needToSave = false;
		bool playerExists = false;
		foreach (HighScore highScore in list) {
			if (highScore.playerName == playerName) {
				if (score >= highScore.score) {
					highScore.score = score;
					list.Sort ();
					needToSave = true;
				}
				playerExists = true;
				break;
			}
		}
		//add to list if limit not reached
		if (!needToSave && list.Count < MAX_NUMBER_HIGHSCORES) {
			if (!playerExists) {
				list.Add (playerScore);
				list.Sort ();
				needToSave = true;
			}

		//list is already sorted with lowest score at the bottom
		} else if (!needToSave && score > list [list.Count - 1].score) {
			if (!playerExists) {
				list.Add (playerScore);
				list.Sort ();
				list.RemoveAt (list.Count - 1);
				needToSave = true;
			}
			
		}
		if (needToSave) {
			StringBuilder sb = new StringBuilder ();
			foreach (HighScore highScore in list) {
				sb.AppendLine (JsonUtility.ToJson (highScore));
			}
			PlayerPrefs.SetString ("HighScores", sb.ToString ());		
		}
	}



		

	public List<HighScore> LoadHighScores(){
		List<HighScore> list = new List<HighScore> ();
		//create inital list of high scores
		if (!PlayerPrefs.HasKey("HighScores")) {
			createDefaultHighScores ();
		}
		string highScores = PlayerPrefs.GetString ("HighScores");

		StringReader reader = new StringReader (highScores);
		string line;
		while ((line = reader.ReadLine ()) != null) {
			HighScore score = JsonUtility.FromJson<HighScore> (line);
			list.Add (score);
		}
		reader.Close ();
		list.Sort ();
		return list;

	}


	private void createDefaultHighScores(){

		List<HighScore> list = new List<HighScore> ();
		list.Add(new HighScore("Simon", 205));
		list.Add(new HighScore("Scott", 195));
		list.Add(new HighScore("Dave", 150));
		list.Add(new HighScore("Jeremy", 125));
		list.Add(new HighScore("Aaron", 115));

		StringBuilder sb = new StringBuilder ();
		foreach(HighScore score in list){
			sb.AppendLine(JsonUtility.ToJson (score));
		}
		PlayerPrefs.SetString("HighScores",sb.ToString());
	}

	public void createDefaultSavedGames(){

		Player newPlayer = new Player ();

		newPlayer.playerName = "Simon";
		newPlayer.wallet = 205;
		SaveGame (newPlayer, board);

		newPlayer.playerName = "Scott";
		newPlayer.wallet = 195;
		SaveGame (newPlayer,  board);

		newPlayer.playerName = "Dave";
		newPlayer.wallet = 150;
		SaveGame (newPlayer, board);


		newPlayer.playerName = "Jeremy";
		newPlayer.wallet = 125;
		SaveGame (newPlayer, board);

		newPlayer.playerName = "Aaron";
		newPlayer.wallet = 115;
		SaveGame (newPlayer, board); 
	}

}

	
