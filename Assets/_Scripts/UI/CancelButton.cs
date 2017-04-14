using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour
{
    public Button buttonComponent;
    public ShopScrollList ShopScrollList;
    

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Awake()
    {
        buttonComponent.onClick.AddListener (HandleClick);
    }

    public void HandleClick(){
        GameController.isBuyFin = true ; 
        ShopScrollList.RemoveButtons();
    }
}