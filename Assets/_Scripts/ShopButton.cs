using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {

	public Button buttonComponent;
    public Text nameLabel;
    public Text priceText;   
    private Seed item;
    private DefaultField field;
    private ShopScrollList scrollList;
    
    // Use this for initialization
    void Start () 
    {
         buttonComponent.onClick.AddListener (HandleClick);
    }
    //Set up  Field Buy and seed 
    public void Setup(DefaultField currentField,Seed currentItem, ShopScrollList currentScrollList)
    {
        item = currentItem;
        field = currentField ;
        nameLabel.text = item.name;
        priceText.text = (item.cost+field.fieldCost).ToString ()+" Baht.";
        scrollList = currentScrollList;
        
    }
    // Set up Only Field Buy
    public void Setup(DefaultField currentField, ShopScrollList currentScrollList){
         field = currentField ;
         nameLabel.text = "Land";
         priceText.text = currentField.fieldCost.ToString()+ " Baht.";
         scrollList = currentScrollList;

    }

    public void HandleClick(){

        scrollList.TryBuy(item,field);

    }

}