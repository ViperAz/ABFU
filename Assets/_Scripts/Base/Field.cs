using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field :MonoBehaviour {


	
	[SerializeField]
	private int id ;
	public Transform[] trans = new Transform[4];

	public int Id {
		get {
			return this.id;
		}
		set {
			this.id = value;
		}
	}
		





}
