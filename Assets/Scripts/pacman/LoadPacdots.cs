using UnityEngine;
using System.Collections;

public class LoadPacdots : MonoBehaviour {
	public TextAsset dataFile;

	// Use this for initialization
	void Start () {
		string[] dataLines = dataFile.text.Split('\n'); 

		int lineNum = 0;
		foreach (string line in dataLines)
		{
			Debug.Log(line);
		}
	}

}
