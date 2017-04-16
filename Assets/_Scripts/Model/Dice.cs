using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

	public Sprite[] diceSites = new Sprite[6];
	public int minDiceRuns = 2;					// how long the dices roll
	public int maxDiceRuns = 6;					// the actual value is chosen randomly between the 2 variables

	int dicedNumber = 0;
	 bool isDiceRolling = false;


	// Use this for initialization
	void Start () {
		this.GetComponent<Image>().sprite = diceSites[Random.Range (0, diceSites.Length)];
	}
	
	public IEnumerator rollTheDice()
	{
		isDiceRolling = true;
		
		float diceTime = 0.1f;
		int diceRuns = Random.Range(minDiceRuns, maxDiceRuns) * diceSites.Length;
		
		int lastNumber = 0;
		for(int i = 0; i < diceRuns; i++)
		{
			int c;
			
			// this is to prevent the same number twice in a row
			while((c = Random.Range(0, diceSites.Length)) == lastNumber)
				yield return null;
			
			lastNumber = c;
			
			// increase the time to make it look like the dices getting slower over time
			diceTime += 0.25f/diceRuns; 
			//play sound here			

			yield return new WaitForSeconds(diceTime);
			
			// update the graphic to show the correct number
			this.GetComponent<Image>().sprite = diceSites[lastNumber];
			
			yield return null;
		}
		
		// add plus 1 because the Dice Numbers start at 1 not 0
		dicedNumber = lastNumber + 1;
		
		yield return new WaitForSeconds(0.1f);
		
		isDiceRolling = false;
		
		yield return 0;
	}
		/// <summary>
	/// Rolls the dice.
	/// </summary>
	public  void RollTheDice ()
	{
		StartCoroutine(rollTheDice());
	}

	/// <summary>
	/// The diced Number.
	/// </summary>
	/// <returns>The number.</returns>
	public  int DicedNumber ()
	{
		return dicedNumber;
	}

	/// <summary>
	/// Determines whether this dice is rolling.
	/// </summary>
	/// <returns><c>true</c> if this dice is rolling; otherwise, <c>false</c>.</returns>
	public  bool IsDiceRolling ()
	{
		return isDiceRolling;
	}
}
