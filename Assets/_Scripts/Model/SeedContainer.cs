using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine ;



// Load all Seed from xml file 

[XmlRoot("SeedContainer")]
public class SeedContainer  {

	[XmlArray("Seeds"),XmlArrayItem("Seed")]
	public List<Seed> Seeds = new List<Seed>();


	public static SeedContainer Load (string path){
		var serializer = new XmlSerializer(typeof(SeedContainer));
		using(var stream = new FileStream(path, FileMode.Open))
		{
			return serializer.Deserialize(stream) as SeedContainer;
		}
	}
	
	


}

