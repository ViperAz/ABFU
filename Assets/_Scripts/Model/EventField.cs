using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventField : Field {


	private ActionType actionType ; 

    public EventField()
    {
        type =  FieldType.startField;
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
