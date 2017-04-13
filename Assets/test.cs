using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	public Transform  trans ; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(trans.position, new Vector3(0,-1,0), 30 * Time.deltaTime);

	}
}
