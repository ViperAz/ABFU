using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml ;
using System.IO ; 

// [System.Serializable]
// public class Item
// {
//     public string itemName;
//     public Sprite icon;
//     public float price = 1;
// }

public class ShopScrollList : MonoBehaviour {

	public SeedContainer seedBase;
    public List<Seed> itemList;
    public Transform contentPanel;
    public Player currentPlayer;
    public DefaultField currentField;
    public Text myGoldDisplay;
	public Text qoutaDisplay ; 
    public SimpleObjectPool buttonObjectPool;
    



    // Use this for initialization
    void Start () 
    {
		seedBase = SeedContainer.Load (Path.Combine (Application.dataPath, "SeedContainer.xml"));
		foreach(Seed s in seedBase.Seeds){
			itemList.Add(s);
		}

        // RefreshDisplay ();

    }


   public void Display(Player player,DefaultField field)
    {
        currentPlayer = player;
        currentField = field ;
        myGoldDisplay.text = "Money : "+player.money.ToString();
        qoutaDisplay.text = "Qouta : "+player.buyQouta.ToString();
        RemoveButtons();
        AddButtons();
    }

   
    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0) 
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
            // Destroy(toRemove);
        }
    }



    private void AddButtons()
    {
         GameObject newButton = buttonObjectPool.GetObject();
         newButton.transform.SetParent(contentPanel,false);

         ShopButton ShopButton = newButton.GetComponent<ShopButton>();
         ShopButton.Setup(currentField, this);
        for (int i = 0; i < itemList.Count; i++) 
        {
            Seed item = itemList[i];
            newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(contentPanel,false);

            ShopButton = newButton.GetComponent<ShopButton>();
            ShopButton.Setup(currentField,item, this);
        }
    }

    public void TryBuy(Seed seed,DefaultField field )
    {
         GameController.isBuyFin = true ;
        if (currentPlayer.buyQouta >0){

           
            currentPlayer.buyQouta  --;
        }else{

        }
    }

    
}