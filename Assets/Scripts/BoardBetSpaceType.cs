using UnityEngine;
using System.Collections;

public class BoardBetSpaceType : MonoBehaviour {

	public BetTypeEnum betTypeEnum;
	//Payout Ratios
	private const int STRAIGHT_UP = 35; //	StraightUp, Split, Street, Corner, DoubleStreet, Trio, 
	private const int SPLIT = 17;
	private const int STREET = 11;
	private const int CORNER = 8;
	private const int DOUBLE_STREET = 5;
	private const int TRIO = 11;
	private const int FIRST_FOUR = 8;
	private const int LOW = 1;
	private const int HIGH = 1;
	private const int RED = 1;
	private const int BLACK = 1;
	private const int EVEN = 1;
	private const int ODD = 1;
	private const int DOZEN = 2;
	private const int COLUMN = 2;


	public string Name{
		get{
			return betTypeEnum.ToString ();
		}
	}
		
	public int PayoutToOneRatio{
		get{
			switch (betTypeEnum) {
			case BetTypeEnum.StraightUp: return STRAIGHT_UP;
			case BetTypeEnum.Split: return SPLIT;
			case BetTypeEnum.Street: return STREET;
			case BetTypeEnum.Corner: return CORNER;
			case BetTypeEnum.DoubleStreet: return DOUBLE_STREET;
			case BetTypeEnum.Trio: return TRIO;
			case BetTypeEnum.FirstFour: return FIRST_FOUR;
			case BetTypeEnum.Low: return LOW;
			case BetTypeEnum.High: return HIGH;
			case BetTypeEnum.Red: return RED;
			case BetTypeEnum.Black: return BLACK;
			case BetTypeEnum.Even: return EVEN;
			case BetTypeEnum.Odd: return ODD;
			case BetTypeEnum.Column: return COLUMN;
			case BetTypeEnum.Dozen: return DOZEN;
			default: return 0;
			}
		}
	}

}

public enum BetTypeEnum
{
	StraightUp, Split, Street, Corner, DoubleStreet, Trio, 
	FirstFour, Low, High, Red, Black, Even, Odd, Dozen, Column
}