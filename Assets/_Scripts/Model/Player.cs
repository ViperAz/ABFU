using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class Player : MonoBehaviour {

	//Default var
	// private int id ;
	// float money ;
	// string playerName ;
	
	private Rigidbody rb;
	private int currentFieldId ; 

	public int buyQouta = 3 ;

	public bool isPastStartPoint = false  ;

	public Transform playerCamera; 

	public List<DefaultField> owning = new List<DefaultField>();


	public PlayerUI ui ;
	private int netWorth = 0;



	void Start (){
		rb = GetComponent<Rigidbody> ();
		this.netWorth = money ;
		ui.SetUp(this.playerName,this.money,this.netWorth);
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

	public bool isPastStart ;

	public Material color { get; set; }

	public void AddField(DefaultField field){
		this.owning.Add(field);
	}

	public void removeField(DefaultField field){		
		this.owning.Remove(field);
	}

	private void updateMoney (){
		ui.updateMoney(this.money);
	}

	private void updateNetWorth(){
		netWorth = money;
		foreach(DefaultField g in this.owning){
			netWorth += g.getBuyOutPrice();
		}
		ui.updateNetWorth(this.netWorth);
			}

	public void updateUI(){
		updateMoney ();
		updateNetWorth();
	}


}
