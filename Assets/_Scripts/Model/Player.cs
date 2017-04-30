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

	public bool isMissNextTurn = false  ;

	public Transform playerCamera; 

	public List<DefaultField> owning = new List<DefaultField>();


	public PlayerUI ui ;
	private int netWorth = 0;

	public bool isWin = false ;



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


	public int getSeedNetWorth(){
		int temp  = 0 ;
		foreach(DefaultField f in owning){
			temp += f.seed.cost ;
		}
		return (int)(temp*0.1) ;
	}

	public void updateUI(){
		updateMoney ();
		updateNetWorth();
	}

	public bool isOwnMarket(int zone){

		foreach (DefaultField f in owning){
			if (f.type == FieldType.marketField && f.zone == zone){
				return true ; 
			}
		}
		return false;

	}

	public void changeMultiPlyer (int zone,int multiply){
		foreach(DefaultField f in owning){
			if(f.zone == zone){
				f.LocalMultiPlyer = multiply ;
			}
		}
	}


	public void checkFactoryWinner(){
		int count = 0 ;
		foreach(DefaultField f in owning){
			if (f.type == FieldType.factoryField){
				count++;
			}
		}
		if (count == 4){
			isWin =true ;
		}
	}

	public void checkLineWinner(int zone){
		int count = 0 ;

		foreach(DefaultField f in owning){
			if (f.zone == zone){
				count++;
			}
		}
		if(count == 8){
			isWin =true ;
		}
	}


}
