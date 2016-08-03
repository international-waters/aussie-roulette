using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ChipInfo {
	public int value;
	public string ownedByPlayer;
	public int betSpaceId;

	public ChipInfo(){

	}
	public ChipInfo(int value, string playerName, int betSpaceId){
		this.value = value;
		this.ownedByPlayer = playerName;
		this.betSpaceId = betSpaceId;
	}
}
