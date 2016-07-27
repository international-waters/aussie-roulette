using UnityEngine;
using System.Collections;

public class Chip : MonoBehaviour {
	public int value;
	public Player ownedByPlayer;
}

public class ClonedChip {
	public int value;
	public Player ownedByPlayer;

	public ClonedChip(){
		
	}
	public ClonedChip(int value, Player player){
		this.value = value;
		this.ownedByPlayer = player;
	}
}
