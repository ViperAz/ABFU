using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public int id ;

	public Text playerName ;
	public Text playerMoney ;
	public Text playerNetworth ;



	public void SetUp(string name,int money , int networth){
		this.playerName.text = name ;
		this.playerMoney.text = "CurrentMoney : "+money.ToString() ;
		this.playerNetworth.text = "NetWorth : "+networth.ToString(); 
	}

	public void updateMoney(int money){
		this.playerMoney.text = "CurrentMoney : "+money.ToString() ;
	}

	public void updateNetWorth(int networth){
		this.playerNetworth.text = "NetWorth : "+networth.ToString(); 
	}
}
