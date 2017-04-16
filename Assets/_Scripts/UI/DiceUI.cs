using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceUI : MonoBehaviour {
	
	public Button buttonComponent;
	// Use this for initialization
	void Start () {
		buttonComponent.onClick.AddListener(HandleClick);
	}
	
	// Update is called once per frame

	void HandleClick(){
		GameController.isRollPress = true ;

	}


}
