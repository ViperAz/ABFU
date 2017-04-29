using UnityEngine;
using UnityEngine.UI;

public class DefaultField : Field {

	

	public Transform plantArea;
	
	public int cost = 0 ;


	public Seed seed ; 

	// public FieldType type = (int) FieldType.defaultField;

	public int zone ;

	public Text ProvinceText;
	public Text StandCostText;

	public Text OwningText;

	


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
		
		GameObject obj = (GameObject )Instantiate(seed.model,this.plantArea.position,Quaternion.identity);
		obj.transform.SetParent(this.plantArea);

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

	public void updateUI(){
		Debug.Log("Update ui");
		this.ProvinceText.text = name;
		this.StandCostText.text = (owner == null)?"Buy Price "+cost.ToString()+" Baht":"StandPrice "+getStandCost().ToString()+" Baht";
		this.OwningText.text = (owner == null)? "No Owner":owner.name ;
	}



}
