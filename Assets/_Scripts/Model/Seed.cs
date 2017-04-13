using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Item {

	// public int Id;
	// public string Name;

	// public int Cost ;

	//area center = 1 south = 2 east = 3 north = 4 . 
	public int Zone ;

	public int getStandCost(int fieldZone){

		if (fieldZone == Zone) {
			return Cost; 
		}
		return Cost / 2; 
	}

}
