using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Seed : Item {

	// public int Id;
	// public string Name;

	// public int Cost ;

	//area center = 1 south = 2 east = 3 north = 4 . 
	public int zone ;
	

	public int getStandCost(int fieldZone){

		if (fieldZone == zone) {
			return cost; 
		}
		return cost / 2; 
	}
	
	public override string ToString(){
		return this.name;
	}
	
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		
	}

}
