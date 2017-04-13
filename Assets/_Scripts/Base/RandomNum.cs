using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNum : MonoBehaviour {


	public int minVal;
	public int maxVal;
	public float probability ;



	public RandomNum (int minVal,int maxVal,float probability){
		this.minVal = minVal;
		this.maxVal = maxVal;
		this.probability = probability;
	}


	public int getVal(){
		return Random.Range (minVal, maxVal);
	}
}
