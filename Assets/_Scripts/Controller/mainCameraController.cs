using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCameraController : MonoBehaviour {
	

	public  Transform defaultFocusTarget ;

	public Transform target ;

	private Transform cameraStand; 

	// private bool isdefault = true ; 

	private bool isRotateActivate = false;


	public float camSpeed ; 

	private int direction = 0;
	private float inverseMoveTime;

	private Camera mainCamera ; 

	public FreeFly freefly ;


	// Use this for initialization
	void Start () {

		freefly.enabled = false ;

		inverseMoveTime = 1 / camSpeed; 

		target = defaultFocusTarget;

		mainCamera = GetComponent<Camera> ();


//		Debug.Log((mainCamera == null) ? "null" : "not null");

	}
	
	// Update is called once per frame
	void Update () {

		transform.LookAt (target);

//		transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, inverseMoveTime* Time.deltaTime*200);
		if(isRotateActivate){
			rotateAround();
		}
			
	}

	public IEnumerator moveCameraTo (Transform trans){


		float stepSpeed = inverseMoveTime * Time.deltaTime;
		float sqrRemainingDistance = (mainCamera.transform.position - trans.position ).sqrMagnitude;
		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPos = Vector3.MoveTowards (mainCamera.transform.position, trans.position, stepSpeed);
			mainCamera.transform.position  =  newPos;
			//Calculate remaining distance after moving 
			sqrRemainingDistance = (mainCamera.transform.position - trans.position).sqrMagnitude; 
			yield return null;
		}
			
	}
	// Set one time rotation 
	public IEnumerator setRotation (Vector3 quaternion){

		float speed = inverseMoveTime * Time.deltaTime *  2  ;

		float sqrRemainingRotate = (mainCamera.transform.eulerAngles - quaternion).sqrMagnitude; 

		while (sqrRemainingRotate > float.Epsilon) {
			Vector3 newPos = Vector3.MoveTowards (mainCamera.transform.eulerAngles, quaternion, speed);
	
			mainCamera.transform.eulerAngles = newPos; 

			sqrRemainingRotate = (mainCamera.transform.eulerAngles - quaternion).sqrMagnitude;
			yield return null;
		}


	}

	public void changeFocus (Transform target){
		this.target = target; 
	}

	public void resetFocus (){
		this.target = defaultFocusTarget ; 
	}

	/**
	 * moveCamera function
	 * int direction  >> -1 == left
	 * 				  >> 1 == right
	 * 
	 */
	void rotateAround (){
		Debug.Log ("Rotating");
		mainCamera.transform.RotateAround (target.position, new Vector3 (0, direction, 0), Time.deltaTime * 120);
		
	}

	public int Direction {
		get{
			return this.direction;
		}
		set{
			this.direction = value;
		}
	}

	public bool setActiveRotation {
		get{ 
			return this.isRotateActivate;
		}
		set{ 
			this.isRotateActivate = value;
		}
	}

	public void SetParent( Transform newParent )
	{
		transform.SetParent(newParent);
	}



	public void DetachFromParent( )
	{
		transform.SetParent(null);
	}



	
}
