using UnityEngine;

using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

// [System.Serializable]
// public class Item
// {
//     public string itemName;
//     public Sprite icon;
//     public float price = 1;
// }

public class ShopScrollList : MonoBehaviour ,Action{

	public SeedContainer seedBase;
    public List<Seed> itemList;

    public List<SeedModelHolder> modelList = new List<SeedModelHolder>();

    public Transform contentPanel;
    public Player currentPlayer;
    public DefaultField currentField;

    // public DefaultField field ;
    private Seed seed ;
    public Text myGoldDisplay;
	public Text qoutaDisplay ; 

    public Text selectionText;
	public Text statusText ; 
    public SimpleObjectPool buttonObjectPool;

    public Button confirmBtn;
    public Button cancelBtn ;

    public LogManager LogManager ;


    // private DefaultField field ;

    private bool isReadyBuy =false  ;

    private int currentCost = 0;
    



    // Use this for initialization
    void Start () 
    {

        
        // RefreshDisplay ();

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        seedBase = SeedContainer.Load (Path.Combine (Application.dataPath, "SeedContainer.xml"));
		foreach(Seed s in seedBase.Seeds){
            SeedModelHolder temp = new SeedModelHolder(s.name);
            
            modelList.Add(temp);
			itemList.Add(s);
		}

        confirmBtn.onClick.AddListener(Confirm);
        cancelBtn.onClick.AddListener(Cancel);
    }


   public void Display(Player player,DefaultField field)
    {
        this.seed = null;
        currentPlayer = player;
        currentField = field ;
        Debug.Log(currentPlayer.buyQouta);
        myGoldDisplay.text = "Money : "+currentPlayer.money.ToString();
        qoutaDisplay.text = "Qouta : "+currentPlayer.buyQouta.ToString();
        selectionText.text = "Selection : " ;
        statusText.text = "Choose Seed to Plant";
        // RemoveButtons();
        AddButtons();
    }

   
    public void RemoveButtons()
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
        GameObject newButton ;
        ShopButton ShopButton;
        if(currentField.owner == null){
            newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(contentPanel,false);
            //  Debug.Log(itemList.Count);
            ShopButton = newButton.GetComponent<ShopButton>();
            ShopButton.Setup(currentField, this);
        }
        if(this.currentField.type == FieldType.defaultField){
            for (int i = 0; i < itemList.Count; i++) {
                Seed item = itemList[i];
            
                if (currentField.seed != item){
                    Debug.Log(item.name);
                    newButton = buttonObjectPool.GetObject();
                    newButton.transform.SetParent(contentPanel,false);

                    ShopButton = newButton.GetComponent<ShopButton>();
                    ShopButton.Setup(currentField,item, this);
                }
            

            }
        }

    }
    public void updateSelection(Seed seed,DefaultField field){
        
        this.seed = seed ;
        this.currentField = field ;
        updateDisplay();

        
    }
    public void updateSelection(DefaultField field){
        
        this.seed = null ;
        this.currentField = field ;
        updateDisplay();
        
    }

    public void updateDisplay(){

        myGoldDisplay.text = "Money : "+currentPlayer.money.ToString();
        qoutaDisplay.text = "Qouta : "+currentPlayer.buyQouta.ToString();
        selectionText.text = "Selection : "+((this.seed == null) ? "Land" : this.seed.ToString());

        isReadyBuy = getStatus();
        statusText.text = (isReadyBuy) ? "OK" : "Not enough money";

    }

    public bool checkCost(int Cost){
        if(currentPlayer.money >= Cost){
            return true ;
        }
        return false ;
    }


    public bool getStatus(){
        if (this.seed != null){
            currentCost = this.seed.cost+this.currentField.cost;
            return checkCost(currentCost);
        }
        else{
            currentCost = this.currentField.cost;
            return checkCost(currentCost);
        }
    }

    public void Confirm(){
        if(isReadyBuy && this.currentField != null){
            this.currentPlayer.money -= currentCost ;
            this.currentField.owner = this.currentPlayer;
            LogManager.addLog(string.Format("{0} Buy {1}.",currentPlayer.name,currentField.name));
            if(this.seed !=null){
                this.currentField.seed = this.seed ;
                this.currentField.updatePlantModel((modelList.Find(x=> x.name == this.seed.name)));
                LogManager.addLog(string.Format("{0} Plant {1} on {2}.",currentPlayer.name,seed.name,currentField.name));
            }
            // Instantiate((modelList.Find(x=> x.name == this.seed.name).model),this.currentField.plantArea.position,Quaternion.identity,this.currentField.plantArea);
            if(!this.currentPlayer.owning.Contains(this.currentField)){
                this.currentPlayer.AddField(this.currentField);
            }
            if(this.currentField.type == FieldType.marketField){
			    this.currentPlayer.changeMultiPlyer(this.currentField.zone,2);
		    }
            if (this.currentField.type == FieldType.factoryField){
                this.currentPlayer.checkFactoryWinner();
            }
            this.currentPlayer.checkLineWinner(currentField.zone);
            GameController.isBuyFin = true ; 
            this.currentPlayer.buyQouta--;
            RemoveButtons();

        }else{
            statusText.text = "Please Select Seed to Plant";
        }
    }

    public void Cancel(){
         GameController.isBuyFin = true ; 
         RemoveButtons();
    }

    
}