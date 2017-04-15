using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultField : Field {

	

	public Transform plantArea;
	
	public int cost = 0 ;


	public Seed seed ; 

	// public FieldType type = (int) FieldType.defaultField;

	public int zone ;

	


	public Player owner ;

	public override string ToString(){
		return this.name ;
	}


	public void updatePlantModel(SeedModelHolder seed){
		if(this.seed != null){
			foreach (Transform t in plantArea){
				Destroy(t.gameObject);
			}
		}

		Instantiate(seed.model,this.plantArea.position,Quaternion.identity,this.plantArea);

	}
	//Need stock multiplyer
	public int getStandCost(){
		return ((this.cost/2)+this.seed.getStandCost(this.zone))
		*GameController.globalMultiplyer;
	}
	//Need Market Multiplyer
	public int getBuyOutPrice(){
		return (int)((this.cost+this.seed.cost)*GameController.globalMultiplyer*1.3) ;
	}



}
