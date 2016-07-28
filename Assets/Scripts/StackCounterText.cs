using UnityEngine;
using System.Collections;
//This class is needed to set the stack counter text sort order so that
//it renders on top the the betting chips
public class StackCounterText : MonoBehaviour {
	void Start () {
		Renderer renderer = this.gameObject.GetComponent<Renderer> ();
		renderer.sortingLayerID = SortingLayer.GetLayerValueFromName ("Default");
		renderer.sortingOrder = 0;		
	}


}
