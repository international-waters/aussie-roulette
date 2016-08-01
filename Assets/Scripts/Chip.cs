/****************************************************************************
* Building IT Systems (CPT 111 / COSC 2635) SP2, 2016
* Game: Aussie Roulette  Group: International Waters
* Authors : Aaron Horton s3465420, David Morling s3492242
* Jeremy Cottell s3242784, Scott Nelson s3363315 , Simon Overton s3397924
*
* 
****************************************************************************/
using UnityEngine;
using System.Collections;
using System;

public class Chip : MonoBehaviour {

	private int chipValue;
	public int val; // just so that it shows in the inspector for reference;
	public string ownedByPlayer;
	public int betSpaceId = -1;
	public int spriteSortOrder = 3;
	public Sprite[] sprites;
	private SpriteRenderer spriteRenderer;

	public int Value {
		get{ return chipValue; }
		set{
			this.chipValue = value;
			SetSpriteByChipValue ();
			val = this.chipValue;
		}

	}
	public void Awake(){
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
	}
	public void Start() {
		//SetSpriteByChipValue ();
		spriteRenderer.sortingOrder = spriteSortOrder;
	}
	public ChipInfo ToChipInfo(){
		return new ChipInfo (this.Value, this.ownedByPlayer, this.betSpaceId);
	}
	public void SetSpriteByChipValue ()
	{
		switch (chipValue) {
		case 1:
			spriteRenderer.sprite = sprites [0];
			break;
		case 5:
			spriteRenderer.sprite = sprites [1];
			break;
		case 10:
			spriteRenderer.sprite = sprites [2];
			break;
		case 25:
			spriteRenderer.sprite = sprites [3];
			break;
		case 50:
			spriteRenderer.sprite = sprites [4];
			break;
		//else do nothing 
		}
	}
}


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



