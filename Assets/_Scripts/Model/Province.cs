
using System.Collections.Generic;
using UnityEngine ;
using System.IO;
using  System;

public class Province {

	public string name ;
	public int  cost ;

	public Province(string name ,int cost){
		this.name = name;
		this.cost = cost;
	}

	public static List<Province> Load(){
		List<Province> provinces = new List<Province>();
		string path = Path.Combine (Application.dataPath, "Province.txt");
		var file = File.OpenRead(@""+path);
		var reader = new StreamReader(file);
		while (!reader.EndOfStream){
			var line = reader.ReadLine();

			var value = line.Split(' ');

			provinces.Add(new Province(value[0],Int32.Parse(value[1])));

		}
		return provinces;
	}
	
}
