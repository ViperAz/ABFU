using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	public List<Dice> dice = new List<Dice>();
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

	public LogManager logManager ;
	public GameObject ShopCanvas ; 
	public GameObject BuyOutCanvas;

	public GameObject LotteryCanvas ;

	public GameObject RollBtnCanvas ;

	public GameObject WinnerCanvas ;

	public Text winnerText ;

	public Text turnText;
	public Text globalMultiplyerText;
	public List<GameObject> DiceCanvas = new  List<GameObject>();
	public List<GameObject> playerCanvas = new List<GameObject>();
	public bool isShopOpen = false ; 

	//Buy shop
	public static bool isBuyFin = false ;

	int diceNum = 0 ;

	//Buy out
	public static bool isBuyoutFin = false;
	public static bool isBuyOut = false ;

	public static bool isRollPress = false ;
	public ShopScrollList  shoplist ; 
	public BuyOut buyout;

	public LuckyDraw luckydraw ;

	public Player winner ;

	public static int globalMultiplyer = 1;
	public static float Tax = 0.3f ;

	public static  int CRITICAL_TURN = 10 ;

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

		//Check winner

		
		if (isInitFinish){
			cameraUpdate ();
		}

		
		if (!isGameMoving && isOtherFinish && isInitFinish) {
			if(!checkWinner()){
				StartCoroutine( playerTurn ());
			}else if (isGameOver == true){
				StartCoroutine(getWinner());
			}
			
		}
		
		
		

	}

	bool checkWinner(){
		foreach(Player p in players){
			if (p.isWin){
				winner = p ;
				isGameOver = true ;
				return true ;
			}
		}
		if(players.Count == 1){
			winner = players[0];
			isGameOver = true ;
			return true ;
		}
		return false ;
	}

	IEnumerator getWinner(){
		WinnerCanvas.SetActive(true);
		winnerText.text = winner.name ;
		yield return new WaitUntil(()=>(isGameOver == false));
	}


	IEnumerator playerTurn(){

		if (players[currentPlayer].isMissNextTurn){
			players[currentPlayer].isMissNextTurn = false ;
			currentPlayer = (currentPlayer + 1) %playerCount ;
			if (currentPlayer == 0){
				this.turn -- ;
				turnText.text = "Turn Left : "+turn.ToString();
				if (turn < CRITICAL_TURN){
					globalMultiplyer = 2 ;
					globalMultiplyerText.text = "Global Multiplyer : "+globalMultiplyer.ToString();
				}
			}
			changePlayerCam();
			yield break;
		}
		isGameMoving = true;
		isOtherFinish = false ;


		yield return StartCoroutine(RollTheDice());

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
		// Debug.Log (currentPlayer);
		if (currentPlayer == 0){
			this.turn -- ;
		}

		isGameMoving = false;
		isOtherFinish = true; 
		yield return new WaitForSeconds(1.5f);
		changePlayerCam();

	}

	IEnumerator RollTheDice(){
		isRollPress = false;
		diceNum = 0;
		RollBtnCanvas.SetActive(!isRollPress);
		yield return new WaitUntil(() => isRollPress == true);
		RollBtnCanvas.SetActive(!isRollPress);
		foreach(GameObject g in DiceCanvas){
			g.SetActive(true);
		}
		foreach(Dice d in dice){
			d.RollTheDice();
		}
		foreach(Dice d in dice){
			yield return new WaitUntil(() => d.IsDiceRolling() == false);
			diceNum += d.DicedNumber();
		}
		
		yield return new WaitForSeconds(1.0f);

		foreach(GameObject g in DiceCanvas){
			g.SetActive(false);
		}
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
					Debug.Log("Do nothing eiei");
					break ; 
				}
				case FieldType.TrainField :{
					yield return StartCoroutine(TrainEvent());
					break ; 
				}
				case FieldType.forestField :{
					players[currentPlayer].isMissNextTurn = true ;
					break ; 
				}case FieldType.goToForestField :{
					yield return StartCoroutine(goToForestEvent());
					break;
				}case FieldType.LotteryField : {
					yield return StartCoroutine(LotterlyEvent());
					break;
				}
				

				

		}

		

	}





	protected IEnumerator Move(int currentPlayer, int diceNum){
		//Field Index not Field Id (if Id = index +1)
		int currentField = players[currentPlayer].fieldId;
		Debug.Log("Current Player"+ players[currentPlayer].playerName+"\n"+
			"Before Move :\n" +
			"Current Field Id : "+currentField);

		yield return new WaitForSeconds (0.3f);

		//Make player move 1 slot per times
		for (int i = 0; i < diceNum; i++) {

			//SOUND and animation implement here when move

			yield return StartCoroutine(aTob(players[currentPlayer], field [currentField].transform.position));
			currentField = (currentField + 1 ) % boardLength; 
			
			
			if (field[currentField].type == FieldType.startField){
				players[currentPlayer].money += 1000+(int)(players[currentPlayer].getSeedNetWorth()) ;
				players[currentPlayer].buyQouta = 3 ;
				players[currentPlayer].updateUI();
				//Log here
			}
			// Check current that player stand Field here
			// yield return checkField(currentField-1);

			//wait for 0.3 sec then move to nextPos
			yield return new WaitForSeconds (0.1f);
		}
		players [currentPlayer].fieldId = currentField;  
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
		Debug.Log("i move");

	}

	// IEnumerator checkField(int currentField){
	// 	// Field field = Fields[currentField];


	// }
	//Set up the Game . 
	IEnumerator setUp(){
		BuyOutCanvas.SetActive(false);
		ShopCanvas.SetActive(false);
		RollBtnCanvas.SetActive(false);
		LotteryCanvas.SetActive(false);
		players.Clear();
		field.Clear();
		dice.Clear();
		defaultField.Clear();
		DiceCanvas.Clear();


		inverseMoveTime = 1f / moveTime;
		isOtherFinish = true; 
		isChangeFinished = false;

		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Dice")) {
			dice.Add (g.GetComponent<Dice> ());
			DiceCanvas.Add(g);
			yield return null;
		}
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

		List<Province> provinces = Province.Load();
		int temp = 0 ;
		int zone = 0 ;
		for (int i = 0; i < defaultField.Count; i++)
		{
			if (temp ==0){
				zone++;
			}
			defaultField[i].name = provinces[i].name;
			defaultField[i].cost = provinces[i].cost;	
			defaultField[i].zone = zone;	
			defaultField[i].updateUI();
			temp = (temp+1)%8;
		}


		boardLength = field.Count; 


		foreach(GameObject g in GameObject.FindGameObjectsWithTag("PlayerUI")){
			g.SetActive(false);
			playerCanvas.Add(g);
		}
		playerCanvas.Sort((a,b) => a.GetComponent<PlayerUI>().id.CompareTo (b.GetComponent<PlayerUI>().id));

		for (int i = 0; i < playerCount; i++) {
			Debug.Log("player has been created");
			GameObject player = (GameObject) Instantiate (Resources.Load("Prefabs/Player/Player")) ; 
			player.tag = "Players";
			player.name = "_Player " + (i+1); // Game ObjName

			players.Add (player.GetComponent<Player>());
			players[i].playerName = "Player "+(i+1);
			players [i].id = (i+1);
			players [i].money = startMoney; 
			players [i].fieldId = 1;
			players[i].ui = playerCanvas[i].GetComponent<PlayerUI>();
			playerCanvas[i].SetActive(true);
			// players [i].playerCamera.enabled = false; 

			//Move All player to Start Location
			players [i].transform.position = field [0].trans [i].position;
			yield return null ; 
		}

		foreach(GameObject g in DiceCanvas){
			g.SetActive(false);
		}

		globalMultiplyerText.text = "Global Multiplyer : "+globalMultiplyer.ToString();
		turnText.text = "Turn Left : "+turn.ToString();
		isInitFinish = true ; 
	}



	//Camera Section
	IEnumerator switchCamera  (Transform trans){
		// StartCoroutine (cameraController.setRotation(trans.eulerAngles));
		yield return StartCoroutine (cameraController.moveCameraTo(trans));

		isChangeFinished = true;



	}

	void changePlayerCam(){
		if ((playerCamMode == 0)) {
			isChangeFinished = false;
			cameraController.freefly.enabled = false ;
		}
	}
	void cameraUpdate(){

		//State Camera Section had nothing to do with the game play	
		if ((playerCamMode == 0) && !isChangeFinished) {
			Cursor.visible = true ;
			Cursor.lockState = CursorLockMode.None;
			cameraController.freefly.enabled = false ;
			cameraController.DetachFromParent ();

			cameraController.changeFocus (players [currentPlayer].transform);
			StartCoroutine (switchCamera (players [currentPlayer].playerCamera));
			cameraController.SetParent (players [currentPlayer].transform);

		// } else if ((playerCamMode == 1) && !isChangeFinished) {
		// 	Cursor.visible = true ;
		// 	Cursor.lockState = CursorLockMode.None;
		// 	cameraController.freefly.enabled = false ;
		// 	cameraController.DetachFromParent ();
		// 	cameraController.resetFocus ();
		// 	cameraController.changeFocus (cameraController.target);
		// 	StartCoroutine (switchCamera (topTrans));
		// 	cameraController.SetParent (topTrans);
		} else if ((playerCamMode == 1 || playerCamMode == 2) && !isChangeFinished) {
			Cursor.visible = false ;
			Cursor.lockState = CursorLockMode.Locked;
			cameraController.DetachFromParent ();
			cameraController.resetFocus ();
			cameraController.freefly.enabled = true ;
			isChangeFinished = true;
			
		}


		if (Input.GetKeyDown (KeyCode.F)) {
			Debug.Log("Player cam"+playerCamMode);
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
		Player curPlayer= players[currentPlayer];
		Player owner  = field.owner ;
	
		if ((owner == null ||owner ==curPlayer) && curPlayer.buyQouta >0){
					//No Owner
		isShopOpen = true ; 
		isBuyFin = false ;
		ShopCanvas.SetActive(isShopOpen);
		// Debug.Log(defaultField.Find(x => x.Id== curPlayer.fieldId).ToString());
		shoplist.Display (curPlayer,defaultField.Find(x => x.Id== curPlayer.fieldId));
		// logManager.addLog("test");
		yield return new WaitUntil(() => isBuyFin == true);

		curPlayer.updateUI();
		field.updateUI();
		
		isShopOpen = false  ; 
		ShopCanvas.SetActive(isShopOpen);
		//
		yield return null ;
		}
		else{
			//Other Owner
			int standCost = field.getStandCost();
			int buyoutPrice =field.getBuyOutPrice();
			//Reduce Money here
			if(curPlayer.money > standCost){
				curPlayer.money -=  standCost; 
				owner.money += standCost;
				
				logManager.addLog(string.Format("{0} Lose {1} Baht to {2}.",curPlayer.name,standCost,owner.name));
				owner.updateUI();
				curPlayer.updateUI();
				// field.updateUI();
				if(curPlayer.money >= buyoutPrice){
					//Buy out here
					isBuyFin = false ;
					isBuyOut = false ;
					isBuyoutFin = false ;
					BuyOutCanvas.SetActive(true);
					buyout.Display(curPlayer,defaultField.Find(x => x.Id== curPlayer.fieldId));
					yield return new WaitUntil(() => isBuyoutFin == true);
					BuyOutCanvas.SetActive(false);
					owner.updateUI();
					curPlayer.updateUI();
					field.updateUI();
					if(isBuyOut && curPlayer.buyQouta >0 && field.type == FieldType.defaultField){
						isShopOpen = true ; 
						ShopCanvas.SetActive(isShopOpen);
						shoplist.Display (curPlayer,defaultField.Find(x => x.Id== curPlayer.fieldId));
						yield return new WaitUntil(() => isBuyFin == true);
						isShopOpen = false  ; 
						ShopCanvas.SetActive(isShopOpen);
						owner.updateUI();
						curPlayer.updateUI();
						field.updateUI();
					}
				}	
			}
			else{
				owner.money += standCost;
				foreach(DefaultField f in curPlayer.owning){
					f.removePlant();
					f.owner = null;
				}
				players.Remove(curPlayer);
			}
			
			yield return null ;	
		}


	}


	IEnumerator goToForestEvent(){
		int currentField = players[currentPlayer].fieldId;

		while(field [currentField-1].type != FieldType.forestField){
			
			currentField = (currentField - 1 ) % boardLength; 
			yield return StartCoroutine(aTob(players[currentPlayer], field [currentField].transform.position));
			Debug.Log(currentField);
			// yield return new WaitForSeconds(0.3f);

		}
		players [currentPlayer].fieldId = currentField;  
		logManager.addLog(string.Format("{0} lost in forest stop playing for 1 turn." ,players [currentPlayer])) ;
		yield return null ;
	}

	IEnumerator TrainEvent(){
		int currentField = players[currentPlayer].fieldId;
		currentField = (currentField + 20 ) % boardLength; 
		yield return StartCoroutine(aTob(players[currentPlayer], field [currentField-1].transform.position)) ;
		players [currentPlayer].fieldId = currentField; 
		logManager.addLog(string.Format("{0} just lost into the forest." ,players [currentPlayer])) ;
	}

	IEnumerator LotterlyEvent(){

		if (players[currentPlayer].money >= luckydraw.StartPrice){

			LotteryCanvas.SetActive(true);
			luckydraw.Display(players[currentPlayer]);
			yield return new WaitUntil(()=>(luckydraw.isStateReady == true || luckydraw.isFin == true));

			if (luckydraw.isStateReady == true){
				luckydraw.luckyDice.RollTheDice();

				yield return new WaitUntil (()=>(luckydraw.luckyDice.IsDiceRolling() == false));
				int diceNum = luckydraw.luckyDice.DicedNumber();
				int reward = luckydraw.getPrize(diceNum);
				players[currentPlayer].money += reward;
				if (reward == 0){
					logManager.addLog(string.Format("{0} won nothing." ,players [currentPlayer])) ;
				}else{
					logManager.addLog(string.Format("{0} just won {1} Baht from lottery event" ,players [currentPlayer],reward)) ;
				}
				
				players[currentPlayer].updateUI();
			}
			yield return new WaitForSeconds(1.0f) ;
		}
		LotteryCanvas.SetActive(false);
		
	}

//	private void switchCamera (int currentPlayer,bool mode){
//		Debug.Log ("Changing Camera to Player"+ (currentPlayer+1)+"Mode"+mode);
//		mainCamera.enabled = !mode; 
//		players [currentPlayer].playerCamera.enabled = mode;
//
//	}

}
