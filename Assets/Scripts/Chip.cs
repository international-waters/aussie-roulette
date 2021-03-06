﻿/****************************************************************************
* Building IT Systems (CPT 111 / COSC 2635) SP2, 2016
* Game: Aussie Roulette  Group: International Waters
* Authors : Aaron Horton s3465420, David Morling s3492242
* Jeremy Cottell s3242784, Scott Nelson s3363315 , Simon Overton s3397924
*
* 
****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ChipMove {ToDealer,ToPlayer,DontMove,Disabled};
public class Chip : MonoBehaviour {

	private int chipValue;
	public int val; // just so that it shows in the inspector for reference;
	public string ownedByPlayer;
	public int betSpaceId = -1;
	public int spriteSortOrder = 3;
	private const float MOVESPEED = 10f;
	public Sprite[] sprites;
	private Vector3 DealerTargetPos;
	private Vector3 PlayerTargetPos;
	private SpriteRenderer spriteRenderer;

	public ChipMove ClearChip { set; private get;}

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
		this.ClearChip = ChipMove.DontMove;
	}
	public void Start() {
		//SetSpriteByChipValue ();
		spriteRenderer.sortingOrder = spriteSortOrder;
		DealerTargetPos = new Vector3 (UnityEngine.Random.Range(-1f,-3f),
			UnityEngine.Random.Range(2.7f,4f),0f);
		//PlayerTargetPos = new Vector3 (UnityEngine.Random.Range(-4f,-7f),-5f,0f);
		PlayerTargetPos = new Vector3 (UnityEngine.Random.Range(-1f,7f),-5f,0f);
	}

	public void Update() {
		if (ClearChip == ChipMove.ToDealer) {
			transform.position = Vector2.MoveTowards (transform.position,
				this.DealerTargetPos, MOVESPEED * Time.deltaTime);
		}
		if (ClearChip == ChipMove.ToPlayer) {
			transform.position = Vector2.MoveTowards (transform.position,
				this.PlayerTargetPos, MOVESPEED * Time.deltaTime);
		}

		if (transform.position == this.DealerTargetPos) {
			Destroy(gameObject);
		}
		if (transform.position == this.PlayerTargetPos) {
			Destroy(gameObject);
		}
		/*if (Vector3.Distance (transform.position, this.DealerTargetPositon )<=0){
			Destroy(gameObject);
		}*/
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



