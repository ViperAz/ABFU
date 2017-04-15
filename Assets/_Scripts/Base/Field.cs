using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field :MonoBehaviour {


	
	[SerializeField]
	private int id ;

	public string name ;
	public Transform[] trans = new Transform[4];

	public FieldType type = (int) FieldType.defaultField;

	public int Id {
		get {
			return this.id;
		}
		set {
			this.id = value;
		}
	}

	public void Plant(){

	}
		





}
