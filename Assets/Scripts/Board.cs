﻿using UnityEngine;
using System.Collections;


public class Board: MonoBehaviour {
	
	public bool isTakingBets;
	//inside bet cell width and height
	public float INSIDECELL_X =0.204f;
	public float INSIDECELL_Y = 0.208f;

	//inside bet column and row size
	private const int INSIDE_COLS = 7;
	private const int INSIDE_ROWS = 24;	

	//location of the grid on the game screen offset from game x0, y0
	public float yOffset;
	public float xOffset;

	public GameObject InsideBetLocation;
	public int instanceCounter = 0;

	void Start () {
		InitialiseGrid ();
		isTakingBets = true;
	}
	private void InitialiseGrid()
	{
		
		int FirstRowNumber = -2;
		for (int row = 0; row < INSIDE_ROWS; row ++) {
			for (int col = 0; col < INSIDE_COLS; col ++) {
				instanceCounter++;
				//Create a new inside bet collider at the current grid location
				GameObject location = (GameObject) Instantiate (InsideBetLocation, 
					new Vector3((row * INSIDECELL_X)-xOffset, (col* INSIDECELL_Y) - yOffset, 0), Quaternion.identity);
				BoardBetSpace betSpace = location.GetComponent<BoardBetSpace> ();
				BoardBetSpaceType betSpaceType = location.GetComponent<BoardBetSpaceType> ();
				// check if the row corrosponds with "straight up locations"
				if (row % 2 == 1) {

					//first column of a new row, set the value of the first digit in this row
					if (col == 0) FirstRowNumber += 3;

					//both column and row indicate this is a "straight up" position
					if (col % 2 == 1) {
						betSpaceType.betTypeEnum = BetTypeEnum.StraightUp;
						betSpace.winNumbers = CalcGridWinNumbers (FirstRowNumber, col, BetTypeEnum.StraightUp);

					} 
					//wrong column for straight up, could either be a street or side split
					else {
						//first or last column, this is a street bet
						if (col == 0 || col == 6){
							betSpaceType.betTypeEnum = BetTypeEnum.Street;
							betSpace.winNumbers = CalcGridWinNumbers (FirstRowNumber, col, BetTypeEnum.Street);
						}
						//positon is a split accross the row
						else{
							betSpaceType.betTypeEnum = BetTypeEnum.Split;
							betSpace.winNumbers = CalcGridWinNumbers (FirstRowNumber, col, BetTypeEnum.Split);
						}
					}
				
				}
				//these rows corrospond with the lines on the table
				if (row % 2 == 0) {

					//first row has different rules
					if (row == 0) {
						//first or last column this is a first 4 bet
						if (col == 0 || col == 6) {
							betSpaceType.betTypeEnum = BetTypeEnum.FirstFour;
							betSpace.winNumbers = CalcGridWinNumbers (FirstRowNumber, col, BetTypeEnum.FirstFour);
						}
						//bet location is a tri position
						else if (col % 2 == 0) {
							betSpaceType.betTypeEnum = BetTypeEnum.Trio;
							betSpace.winNumbers = CalcGridWinNumbers (FirstRowNumber, col, BetTypeEnum.Trio);
						}
					}
					//not the first row
					else {
						//first or last column this is a double street bet
						if (col == 0 || col == 6) {
							betSpaceType.betTypeEnum = BetTypeEnum.DoubleStreet;
							betSpace.winNumbers = CalcGridWinNumbers (FirstRowNumber, col, BetTypeEnum.DoubleStreet);
						}
						//bet location is a corner position
						else if (col % 2 == 0) {
							betSpaceType.betTypeEnum = BetTypeEnum.Corner;
							betSpace.winNumbers = CalcGridWinNumbers (FirstRowNumber, col, BetTypeEnum.Corner);
						}
					}
					//this postion split accross columns
					if (col % 2 == 1) {
						betSpaceType.betTypeEnum = BetTypeEnum.Split;
						betSpace.winNumbers = CalcGridWinNumbers (FirstRowNumber, col, BetTypeEnum.Split);
					}
				}
			}

		}
	}

	private int[] CalcGridWinNumbers(int FirstRowNumber, int column, BetTypeEnum type)
	{
		int SecondRowNumber = FirstRowNumber + 1;
		int ThirdRowNumber = SecondRowNumber + 1;
		
		int[] winNumbers = new int[1]{0};
		switch (type) {
		case BetTypeEnum.StraightUp:
			switch (column) {
			case 1:
				winNumbers = new int[1]{FirstRowNumber};
				break;	
			case 3:
				winNumbers = new int[1]{SecondRowNumber};
				break;
			case 5:
				winNumbers = new int[1]{ThirdRowNumber};
				break;
			}
			break;
		case BetTypeEnum.Street:
			{
				winNumbers = new int[3]{ FirstRowNumber, SecondRowNumber, ThirdRowNumber };
				break;
			}
		case BetTypeEnum.FirstFour:
			{
				winNumbers = new int[4]{ 0, 1, 2, 3 };
				break;
			}
		case BetTypeEnum.Trio:
			{
				switch(column){
				case 2:
					{
						winNumbers = new int[3]{ 0, 1, 2};
						break;
					}
				case 4:
					{
						winNumbers = new int[3]{ 0, 2, 3};
						break;
					}
				}

				break;
			}
		case BetTypeEnum.Split:
			switch (column) {
			case 1:
				if (FirstRowNumber == -2) {
					winNumbers = new int[2]{ 0, 1 };
				} else
					winNumbers = new int[2]{ FirstRowNumber, FirstRowNumber + 3 };
				break;	
			case 2:
				winNumbers = new int[2]{ FirstRowNumber, SecondRowNumber };
				break;
			case 3:
				if (FirstRowNumber == -2) {
					winNumbers = new int[2]{ 0, 2 };
				} else
					winNumbers = new int[2]{ SecondRowNumber, SecondRowNumber + 3 };
				break;
			case 4:
				winNumbers = new int[2]{ SecondRowNumber, ThirdRowNumber };
				break;
			case 5:
				if (FirstRowNumber == -2) {
					winNumbers = new int[2]{ 0, 3 };
				} else
					winNumbers = new int[2]{ ThirdRowNumber, ThirdRowNumber + 3 };
				break;	
			}
			break;
		case BetTypeEnum.Corner:
			if (column == 2) {
				winNumbers = new int[4] {FirstRowNumber, SecondRowNumber,
					FirstRowNumber + 3, SecondRowNumber + 3};
			} else {
				winNumbers = new int[4] {SecondRowNumber, ThirdRowNumber,
					SecondRowNumber + 3, ThirdRowNumber + 3};
			}

			break;
		case BetTypeEnum.DoubleStreet:
				winNumbers = new int[6] {FirstRowNumber, SecondRowNumber,ThirdRowNumber,
				FirstRowNumber + 3, SecondRowNumber + 3, ThirdRowNumber + 3};

			break;
		}

			return winNumbers;

	}


}