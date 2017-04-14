using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultField : Field {

	

	public Transform plantArea;
	
	public int cost ;


	public Seed seed ; 

	// public FieldType type = (int) FieldType.defaultField;

	public int Zone {get;set;} 

	
	public int fieldCost{
		get{
			return this.cost;
		}
		set{
			this.cost = value; 
		}
	}

	public Player owner ;

	public override string ToString(){
		return this.name ;
	}


	void updatePlantModel(){
		

	}

}
