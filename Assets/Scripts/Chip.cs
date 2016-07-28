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

public class Chip : MonoBehaviour {

	private int value;
	public int spriteSortOrder = 3;
	public Sprite[] sprites;
	public Player ownedByPlayer;
	private SpriteRenderer spriteRenderer;

	public int Value {
		get{ return value; }
		set{
			this.value = value;
			SetSpriteByChipValue ();
		}

	}
	public void Awake(){
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
	}
	public void Start() {
		SetSpriteByChipValue ();
		spriteRenderer.sortingOrder = spriteSortOrder;

}
	public void SetSpriteByChipValue ()
	{
		switch (value) {
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
		}
	}
}



public class SavedChip {
	public int value;
	public Player ownedByPlayer;

	public SavedChip(){
		
	}
	public SavedChip(int value, Player player){
		this.value = value;
		this.ownedByPlayer = player;
	}
}
