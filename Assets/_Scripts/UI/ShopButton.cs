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
    // void Awake () 
    // {
    //      buttonComponent.onClick.AddListener (HandleClick);
    // }
    //Set up  Field Buy and seed 
    public void Setup(DefaultField currentField,Seed currentItem, ShopScrollList currentScrollList)
    {
        
        item = currentItem;
        field = currentField ;
        nameLabel.text = item.name;
        priceText.text = (item.cost+field.cost).ToString ()+" Baht.";
        scrollList = currentScrollList;
        buttonComponent.onClick.AddListener (HandleClickSeed);
        Debug.Log(item);
        
    }
    // Set up Only Field Buy
    public void Setup(DefaultField currentField, ShopScrollList currentScrollList){
         field = currentField ;
         nameLabel.text = "Land";
         priceText.text = currentField.cost.ToString()+ " Baht.";
         scrollList = currentScrollList;
         buttonComponent.onClick.AddListener (HandleClickField);

    }

    public void HandleClickSeed(){
        scrollList.updateSelection(item,field);

    }
    public void HandleClickField(){
        // item = null ;
        scrollList.updateSelection(field);

    }

}