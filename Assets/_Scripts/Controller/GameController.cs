using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml ;
using System.IO ; 
public class GameController : MonoBehaviour {

	/**
	 * 
	 * BEWARE
	 *ID start from 1
	 *Index Start from 0
	 *
	 *Camera Changing not working right now. fuck me !
	**/

	//This var send from GameManager**
	public int playerCount = 4 ;

	//Need to be static to beable to call from other script without initialization from here 
	public static List<Player> players = new List<Player>();
	//All Field for calculate Walking
	public static List<Field> field  = new List<Field>();
	public List<DefaultField> defaultField = new List<DefaultField>();
	public RandomNum[] rans = new RandomNum[12] ;

	//Seed Collection 
	public SeedContainer seedLst ;

	public Transform topTrans;
	public Transform sideTrans; 

	private mainCameraController cameraController ;


	private int  currentTurn ;
	public static int currentPlayer = 0;
	public int turn ; //number of round for this game.
	public int startMoney ;

	public float moveTime ; // in sec
	private float inverseMoveTime;

	public static bool isInitFinish = false ; 
	private bool isGameOver = false;
	private bool isGameMoving = false;
	private bool isOtherFinish = false ; // Check isother play is finsished
	private bool isChangeFinished = false ;
	private int playerCamMode = 0 ;
	int boardLength ; 


	public GameObject ShopCanvas ; 
	public bool isShopOpen = false ; 
	public static bool isBuyFin = false ;
	public ShopScrollList  shoplist ; 

	// Use this for initialization
	void Start () {
		StartCoroutine(setUp());
		GameObject camobj = GameObject.Find ("MainCamera");
		cameraController = camobj.GetComponent<mainCameraController> ();
		cameraController.SetParent (topTrans);
		cameraController.transform.LookAt (cameraController.target);

//		Debug.Log((cameraController == null) ? "null" : "not null");
	}




	// Update is called once per frame
	void Update () {
		//Check Game State Section
		if (isInitFinish){
			cameraUpdate ();
		}
		

		//demo moving
		if (Input.GetKeyDown (KeyCode.Space) && !isGameMoving) {
			StartCoroutine( playerTurn ());
		}


				


	}


	IEnumerator playerTurn(){
		isGameMoving = true;
		isOtherFinish = false ;
		// switchCamera (currentPlayer, true);

		//Demo replace Dice  not live yet
		int diceNum = 1;
		Debug.Log ("Dice num :" + diceNum);


		//Move player to Center of the cell 
		yield  return StartCoroutine(aTob(players[currentPlayer], field [players[currentPlayer].fieldId-1].transform.position));

		// Move Player
		yield return Move(currentPlayer, diceNum);

		//Check Field and Field Action
		yield return StartCoroutine(checkField ()) ; 

		// All Action are finished
		// Move player back to his own Pos
		yield  return StartCoroutine(aTob(players[currentPlayer], field [players[currentPlayer].fieldId-1].trans[currentPlayer].position));


		// switchCamera (currentPlayer, false);

		currentPlayer = (currentPlayer + 1) %playerCount ;
		Debug.Log (currentPlayer);
		currentTurn++;
		isGameMoving = false;
		isOtherFinish = true; 

	}

	IEnumerator checkField (){
		FieldType type = field [players[currentPlayer].fieldId-1].type ; 
			switch(type){
				case FieldType.defaultField :{
					yield return StartCoroutine(defaultFieldEvent());
					break ; 
				}
				case FieldType.factoryField :{
					yield return StartCoroutine(defaultFieldEvent());
					break ; 
				}
				case FieldType.marketField :{
					yield return StartCoroutine(defaultFieldEvent());
					break ; 
				}
				case FieldType.startField : {
					Debug.Log("sa");
					break ; 
				}
				case FieldType.cardField :{
					Debug.Log("ca");
					break ; 
				}
				case FieldType.forestField :{
					Debug.Log("fo");
					break ; 
				}
				

				

		}

		

	}



	//Not used
	void movePlayer(int currentPlayer, int diceNum){
		//move to target position
		StartCoroutine(Move(currentPlayer,diceNum));
		//Check event
	}

	protected IEnumerator Move(int currentPlayer, int diceNum){
		//Field Index not Field Id (if Id = index +1)
		int currentField = players[currentPlayer].fieldId;
		Debug.Log("Current Player"+ players[currentPlayer].playerName+"\n"+
			"Before Move :\n" +
			"Current Field Id : "+currentField);

		yield return new WaitForSeconds (0.3f);
		//Need to disable all player while doing this action ** Not implemented

		//Make player move 1 slot per times
		for (int i = 0; i < diceNum; i++) {

			//SOUND and animation implement here when move

			yield return StartCoroutine(aTob(players[currentPlayer], field [currentField].transform.position));
			currentField = (currentField + 1 ) % boardLength; 

			// Check current that player stand Field here
			// yield return checkField(currentField-1);

			//wait for 0.3 sec then move to nextPos
			yield return new WaitForSeconds (0.3f);
		}
		players [currentPlayer].fieldId = currentField;  
		isGameMoving = false;
		Debug.Log ("Current Player"+ players[currentPlayer].playerName+"\n"+
			"After Move :\n" +
			" Current Field Id : "+currentField);
	}



	IEnumerator aTob (Player player,Vector3 nextPos){
		float stepSpeed = inverseMoveTime * Time.deltaTime;
		float sqrRemainingDistance = (player.transform.position - nextPos ).sqrMagnitude;
		while (sqrRemainingDistance > float.Epsilon) {
			Vector3 newPos = Vector3.MoveTowards (player.transform.position, nextPos, stepSpeed);
			player.transform.position  =  newPos;
			//Calculate remaining distance after moving 
			sqrRemainingDistance = (player.transform.position - nextPos).sqrMagnitude; 
			yield return null;
		}

	}

	// IEnumerator checkField(int currentField){
	// 	// Field field = Fields[currentField];


	// }
	//Set up the Game . 
	IEnumerator setUp(){

		ShopCanvas.SetActive(isShopOpen);
		players.Clear();
		field.Clear ();

		inverseMoveTime = 1f / moveTime;
		isOtherFinish = true; 
		isChangeFinished = false;

		//Load in Data 
		// seedLst = SeedContainer.Load (Path.Combine (Application.dataPath, "SeedContainer.xml"));

		// Debug.Log(seedLst.Seeds.Count);

		// foreach (Seed s in seedLst.Seeds){
		// 	Debug.Log(s.Name+" "+s.Cost);
		// }
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Fields")) {
			field.Add (g.GetComponent<Field> ());
			yield return null;
		}
		foreach (DefaultField g in FindObjectsOfType(typeof(DefaultField)) as DefaultField[]) {
			defaultField.Add (g.GetComponent<DefaultField> ());
			yield return null;
		}
		defaultField.Sort ((a, b) => a.Id.CompareTo (b.Id));
		field.Sort ((a, b) => a.Id.CompareTo (b.Id));


		boardLength = field.Count; 


		for (int i = 0; i < playerCount; i++) {
			GameObject player = (GameObject) Instantiate (Resources.Load("Prefabs/Player/Player")) ; 
			player.tag = "Players";
			player.name = "_Player " + (i+1); // Game ObjName

			players.Add (player.GetComponent<Player>());
			players [i].id = (i+1);
			players [i].money = startMoney; 
			players [i].fieldId = 1;
			// players [i].playerCamera.enabled = false; 

			//Move All player to Start Location
			players [i].transform.position = field [0].trans [i].position;
			yield return null ; 
		}

		

		// Set who start game // Couruteen needed
		// currentPlayer = Random.Range (0, playerCount);//Range 0 - (playerC -1)
		// int tempPlayer = currentPlayer;
		// for (int tempCount = playerCount; tempCount < players.Count; tempCount--) {

		// }
			isInitFinish = true ; 
	}

	IEnumerator switchCamera  (Transform trans){
		StartCoroutine (cameraController.setRotation(trans.eulerAngles));
		yield return StartCoroutine (cameraController.moveCameraTo(trans));

		isChangeFinished = true;



	}
	void cameraUpdate(){
		//State Camera Section had nothing to do with the game play
		if ((playerCamMode == 0) && !isChangeFinished) {
			cameraController.DetachFromParent ();

			cameraController.changeFocus (players [currentPlayer].transform);
			StartCoroutine (switchCamera (players [currentPlayer].playerCamera));
			cameraController.SetParent (players [currentPlayer].transform);

		} else if ((playerCamMode == 1) && !isChangeFinished) {
			cameraController.DetachFromParent ();
			cameraController.resetFocus ();
			cameraController.changeFocus (cameraController.target);
			StartCoroutine (switchCamera (topTrans));
			cameraController.SetParent (topTrans);
		} else if ((playerCamMode == 2) && !isChangeFinished) {
			cameraController.DetachFromParent ();
			cameraController.resetFocus ();
			StartCoroutine (switchCamera (sideTrans));
			cameraController.SetParent (sideTrans);
		}


		if (Input.GetKeyDown (KeyCode.F)) {
			playerCamMode = (playerCamMode + 1) % 3;
			isChangeFinished = !isChangeFinished;
		}


		if (Input.GetKey (KeyCode.Q)) {
			cameraController.setActiveRotation = true;
			cameraController.Direction = -1; 
		} else if (Input.GetKey (KeyCode.E)) {
			cameraController.setActiveRotation = true;
			cameraController.Direction = 1; 
		} else {
			cameraController.setActiveRotation = false; 
			cameraController.Direction = 0;

		}


	}


	IEnumerator defaultFieldEvent(){
		DefaultField field = defaultField.Find(x => x.Id== players[currentPlayer].fieldId);
		if (field.owner == null &&  players[currentPlayer].buyQouta >0){
					//No Owner
		isShopOpen = true ; 
		isBuyFin = false ;
		ShopCanvas.SetActive(isShopOpen);
		shoplist.Display (players[currentPlayer],defaultField.Find(x => x.Id== players[currentPlayer].fieldId));

		yield return new WaitUntil(() => isBuyFin == true);
		
		isShopOpen = false  ; 
		ShopCanvas.SetActive(isShopOpen);
		//
		yield return null ;
		}
		else if (field.owner == players[currentPlayer]){
			Debug.Log("same owner buy a new one or not");
		}
		else{
			//Other Owner
			Debug.Log("lose money and begin buy bitches");
		}


	}

	


		



//	private void switchCamera (int currentPlayer,bool mode){
//		Debug.Log ("Changing Camera to Player"+ (currentPlayer+1)+"Mode"+mode);
//		mainCamera.enabled = !mode; 
//		players [currentPlayer].playerCamera.enabled = mode;
//
//	}

}
