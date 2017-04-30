using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyOut : MonoBehaviour,Action {

	public Player currentPlayer;
    public DefaultField currentField;

	public Text landNameText;
	public Text seedNameText;
	public Text currentMoneyText;
	public Text buyoutPriceText;

	public Button confirmBtn;
	public Button cancelBtn;

	public LogManager LogManager ; 


/// <summary>
/// Awake is called when the script instance is being loaded.
/// </summary>
	void Awake()
	{
		confirmBtn.onClick.AddListener(Confirm);
		cancelBtn.onClick.AddListener(Cancel);
	}

	public void Display(Player player,DefaultField field){
		this.currentPlayer = player ; 
		this.currentField = field ;
		landNameText.text="Province Name : "+this.currentField.name;
		seedNameText.text="Detail :"+this.currentField.seed.name;
		currentMoneyText.text = "Current Money :"+this.currentPlayer.money ;
		buyoutPriceText.text = "Buy out Price : "+this.currentField.getBuyOutPrice();

	}

	public void Confirm(){
		LogManager.addLog(string.Format("{0} bought {1} from {2} for {3} Baht.",
		currentPlayer.name,currentField.name,currentField.owner.name,
		currentField.getBuyOutPrice().ToString()));

		LogManager.addLog(string.Format("{0} get {1} Baht from Buyout",
		currentField.owner.name,(currentField.getBuyOutPrice()*0.7).ToString()));

		this.currentPlayer.money -= this.currentField.getBuyOutPrice();
		this.currentField.owner.money += (int) (this.currentField.getBuyOutPrice()
		*(1-GameController.Tax)/2);
		

		this.currentField.owner.removeField(this.currentField);
		if(this.currentField.type == FieldType.marketField){
			this.currentPlayer.changeMultiPlyer(this.currentField.zone,2);
			this.currentField.owner.changeMultiPlyer(this.currentField.zone,1);
		}
		this.currentField.owner = this.currentPlayer;
		this.currentPlayer.AddField(this.currentField);



		GameController.isBuyOut = true ; 
		GameController.isBuyoutFin = true ; 
	}

	public void Cancel(){
		GameController.isBuyoutFin = true ; 
		GameController.isBuyOut = false ; 
	}





}
