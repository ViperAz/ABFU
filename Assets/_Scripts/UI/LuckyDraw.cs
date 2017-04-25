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


    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        confirmBtn.onClick.AddListener(Confirm);
        cancelBtn.onClick.AddListener(Cancel);
        int i = 250 ; 
        foreach(Button btn in moneyBtn){
            btn.onClick.AddListener(() =>{selectMoney(i);});
            i*=2;
        }
    }
    
	
	// Update is called once per frame
	void updateSelection () {
		
	}

    public void Display(Player player){

    }

    public void  selectMoney(int money){

    }

	public void Cancel()
    {
        throw new NotImplementedException();
    }

    public void Confirm()
    {
        throw new NotImplementedException();
    }
}
