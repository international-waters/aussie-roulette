using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class GameManager : MonoBehaviour {
	public static GameManager instance { get; private set;}

	public Player player;

	//game screen labels
	public Text lastWin_lbl;
	public Text placedBetsTotal_lbl;
	public Text score_lbl;

	private Board board;
	private const int MAX_NUMBER_HIGHSCORES = 5;

	//Use this to set the winning number from a spin in another scene
	public int winNumberFlag = -1;

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

	public void ProcessWinNumber(){
		ProcessWinNumber (this.winNumberFlag);



	}

	public void ProcessWinNumber(int winNumber){
		if (board == null) {
			board = GameObject.Find ("RouletteTable").GetComponent<Board> ();
		}
		//record chip placement for repeat bet option
		board.StoreAllPlacedChipInfo ();
		board.ClearLosingBets (winNumber);
		board.PayoutWinnings (winNumber, player);
		player.CurrentBetTotal = board.CalculatePlayersTotalBet (player);
		RefreshScorePanel ();
		//reset flag to default state
		this.winNumberFlag = -1;
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
		DeleteSavedGame (player.playerName);

		board.ClearStoredChipHistory ();

		//save all the bets player has on the table to bet history (used to repeat bets)
		board.StoreAllPlacedChipInfo ();

		//clear the board and credit the player, the players actual score includes the value
		//of all chips they have on the table as well as their wallet balance.
		board.ClearAllBets(player);
		StringBuilder sb = new StringBuilder ();
		sb.AppendLine (JsonUtility.ToJson(player));

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
		//Create default list of players for testing
		if (PlayerPrefs.GetString ("SavedGamesList") == string.Empty) {
			PlayerPrefs.SetString ("SavedGamesList", "Simon,Scott,Dave,Aaron,Jeremy,"); 
		}
		board.ClearAllBets ();
		Player player;
		string playerData = PlayerPrefs.GetString (playerName);
		//player not found, return a new player with name = playerName
		if (playerData == string.Empty) {
			player = new Player (playerName);
		} else {
			StringReader reader = new StringReader (playerData);
			List<ChipInfo> savedChips = new List<ChipInfo> ();
			player = JsonUtility.FromJson<Player> (reader.ReadLine());
			string line;
			while ((line = reader.ReadLine ()) != null) {
				ChipInfo chip = JsonUtility.FromJson<ChipInfo> (line);
				savedChips.Add (chip);
			}
			reader.Close ();
			board.savedChips = savedChips;
			board.PlaceAllStoredChips(player);
		}
		board.ClearStoredChipHistory ();
		return player;
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

		foreach (HighScore highScore in list) {
			if (highScore.playerName == playerName) {
				highScore.score = score;
				list.Sort ();
				needToSave = true;
				break;
			}
		}
		//add to list if limit not reached
		if (!needToSave && list.Count < MAX_NUMBER_HIGHSCORES) {
			list.Add (playerScore);
			list.Sort ();
			needToSave = true;

		//list is already sorted with lowest score at the bottom
		} else if (!needToSave && score > list [list.Count - 1].score) {
			list.Add (playerScore);
			list.Sort ();
			list.RemoveAt (list.Count - 1);
			needToSave = true;
			
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
		list.Add(new HighScore("Jeremy", 125));
		list.Add(new HighScore("Simon", 205));
		list.Add(new HighScore("Dave", 150));
		list.Add(new HighScore("Aaron", 115));
		list.Add(new HighScore("Scott", 195));

		StringBuilder sb = new StringBuilder ();
		foreach(HighScore score in list){
			sb.AppendLine(JsonUtility.ToJson (score));
		}
		PlayerPrefs.SetString("HighScores",sb.ToString());
	}

}

	
