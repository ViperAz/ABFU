using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LogManager : MonoBehaviour {

	public SimpleObjectPool objpool ;

	public Transform Parent ;

	public List<GameObject> log = new List<GameObject>();



	public void addLog(string text){
		GameObject log ;
		Text msg ;
		log = objpool.GetObject();
		log.transform.SetParent(Parent,false);
		msg = log.GetComponent<Text>() ;
		msg.text = "   "+text;
		this.log.Add(log);
	}
}
