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

	public int id{
		get;set;
	}

	public float money {
		get;set;
	}

	public string playerName {
		get;set;
	}

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


}
