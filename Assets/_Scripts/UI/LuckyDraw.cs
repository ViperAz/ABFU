using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyDraw : MonoBehaviour ,Action{


    public Button confirmBtn ;
    public Button cancelBtn ; 

    public Button[] moneyBtn = new Button[3];


    public Text selectionText ;

    public List<GameObject> disObject  = new List<GameObject>();

    public int current; 

    public int StartPrice ;

    public Dice luckyDice ;

    public GameObject startMode ;

    public GameObject rollMode ;

    public Player currentPlayer ;

    public bool isFin = false ;


    public bool isStateReady = false ;

    public LogManager LogManager;


    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        isFin = false ;
        confirmBtn.onClick.AddListener(Confirm);
        cancelBtn.onClick.AddListener(Cancel);
        foreach(Button btn in moneyBtn){
            int temp = Int32.Parse(btn.GetComponentInChildren<Text>().text);
            btn.onClick.AddListener(delegate{selectMoney(temp);});
        }

        luckyDice  = GameObject.FindGameObjectWithTag("LotteryDice").GetComponent<Dice>();
        startMode.SetActive(true);
        rollMode.SetActive(false);
    }
    
	
	// Update is called once per frame
	void updateSelection () {

        this.selectionText.text = "Current Selection : "+this.current.ToString();
		
	}

    public void Display(Player player){
        isFin = false ;
        isStateReady = false ;
        startMode.SetActive(true);
        rollMode.SetActive(false);
        this.currentPlayer = player;
        current =  0 ; 
        updateSelection ();

    }

    public void  selectMoney(int money){
        Debug.Log(money);
        this.current = money ; 
        updateSelection ();
    }

	public void Cancel()
    {
        isFin = true ;
    }

    public void Confirm()
    {
        if (current != 0){
            startMode.SetActive(false);
            rollMode.SetActive(true);
            isStateReady = true ;
            this.currentPlayer.money -= this.current;
            this.currentPlayer.updateUI();

        }
    }

    public int getPrize(int diceNum)
    {
        int reward ;
        if (diceNum <=3){
            reward  = 0 ;
        }
        else if (diceNum ==  4){
           reward = this.current ;
        }
        else if (diceNum == 5){
           reward = (int)(this.current*1.5) ;
        }
        else{
            reward = this.current*2 ;
        }
        return reward;
    }
}
