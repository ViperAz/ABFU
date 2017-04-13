using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventField : Field {


	private ActionType actionType ; 
	public FieldType type =  FieldType.startField;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

		
	public ActionType Action {
		get{
			return this.actionType;
		}
		set{
			this.actionType = value; 
		}


	}
}
