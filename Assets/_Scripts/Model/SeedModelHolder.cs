
using UnityEngine;

public class SeedModelHolder {

	public SeedModelHolder(string name){
		this.name = name ;
		// Debug.Log("Prefabs/fruit/Prefab"+name);
		model = Resources.Load("Prefabs/fruit/Prefab/"+name) as GameObject;

	}

	public string name ;
	public GameObject model ; 

}


