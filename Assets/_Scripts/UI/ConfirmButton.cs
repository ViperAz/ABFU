using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour
{
    public Button buttonComponent;

    public ShopScrollList ShopScrollList;
    

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
         buttonComponent.onClick.AddListener (HandleClick);
    }


    public void HandleClick(){
        ShopScrollList.Buy();
    }
}