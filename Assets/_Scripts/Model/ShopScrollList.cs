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
    private Seed seed ;
    public Text myGoldDisplay;
	public Text qoutaDisplay ; 

    public Text slectionText;
	public Text statusText ; 
    public SimpleObjectPool buttonObjectPool;

    public Button confirmBtn;
    public Button cancelBtn ;


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
        
        currentPlayer = player;
        currentField = field ;
        Debug.Log(currentPlayer+"==>"+this.currentField.Id);
        myGoldDisplay.text = "Money : "+currentPlayer.money.ToString();
        qoutaDisplay.text = "Qouta : "+currentPlayer.buyQouta.ToString();
        slectionText.text = "Selection : "+((this.seed == null) ? "" : this.seed.ToString());
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
        for (int i = 0; i < itemList.Count; i++) 
        {
            Seed item = itemList[i];
            if (currentField.seed != item){
                newButton = buttonObjectPool.GetObject();
                newButton.transform.SetParent(contentPanel,false);

                ShopButton = newButton.GetComponent<ShopButton>();
                ShopButton.Setup(currentField,item, this);
            }
            

        }
    }
    public void updateSelection(Seed seed,DefaultField field){
        this.seed = seed ;
        this.currentField = field ;
        updateDisplay();
    }

    public void updateDisplay(){

        myGoldDisplay.text = "Money : "+currentPlayer.money.ToString();
        qoutaDisplay.text = "Qouta : "+currentPlayer.buyQouta.ToString();
        slectionText.text = "Selection : "+((this.seed == null) ? "Land" : this.seed.ToString());

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
            if(this.seed !=null){
                this.currentField.seed = this.seed ;
                this.currentField.updatePlantModel((modelList.Find(x=> x.name == this.seed.name)));
            }
            // Instantiate((modelList.Find(x=> x.name == this.seed.name).model),this.currentField.plantArea.position,Quaternion.identity,this.currentField.plantArea);
            if(!this.currentPlayer.owning.Contains(this.currentField)){
                this.currentPlayer.AddField(this.currentField);
            }
            
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