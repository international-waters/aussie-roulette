using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HighScore : IComparable<HighScore>{
	public string playerName;
	public int score;

	public HighScore(){
	}

	public HighScore(string playerName, int score){
		this.playerName = playerName;
		this.score = score;
	}

	int IComparable<HighScore>.CompareTo (HighScore other)
	{
		return other.score.CompareTo (this.score);
	}


}
