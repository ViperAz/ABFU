using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	//Default var
	private int id ;
	float money ;
	string playerName ;
	private Inventory inventory ;
	private Rigidbody rb;
	private int currentFieldId ; 

	private bool isPastStartPoint ;

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

	public int ID{
		get{
			return id;
		}
		set{
			this.id = value;
		}
	}

	public float Money {

		get {
			return this.money;
		}
		set {
			this.money = value;
		}
	}

	public string PlayerName {
		get{
			return this.playerName;
		}
		set{
			this.playerName = value; 
		}
	}

	public int FieldId {
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
