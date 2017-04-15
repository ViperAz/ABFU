using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	//Default var
	// private int id ;
	// float money ;
	// string playerName ;
	private Rigidbody rb;
	private int currentFieldId ; 

	public int buyQouta = 3 ;

	private bool isPastStartPoint = false  ;

	public Transform playerCamera; 

	public List<DefaultField> owning = new List<DefaultField>();


	void Start (){
		rb = GetComponent<Rigidbody> ();
//		playerCamera = GetComponent<Camera> ();
	}


	//Status var

	public Rigidbody rigidBody{ 
		get{
			return this.rb;
		}

		set{ 
			this.rb = value;
		}
	}

	public int id ;

	public int money ;

	public  string playerName ;

	public int fieldId {
		get{
			return this.currentFieldId;
		}
		set{
			this.currentFieldId = value;
		}
	}

	public bool isPastStart {
		get{return this.isPastStartPoint ; }
		set{this.isPastStartPoint = value ;}
	}

	public Material color { get; set; }

	public void AddField(DefaultField field){
		this.owning.Add(field);
	}

	public void removeField(DefaultField field){		
		this.owning.Remove(field);
	}


}
