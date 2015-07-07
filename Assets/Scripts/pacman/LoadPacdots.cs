using UnityEngine;
using System.Collections;
using System.IO;

public class LoadPacdots : MonoBehaviour {
	public TextAsset dataFile;

	// Use this for initialization
	void Start () {
		string input = File.ReadAllText( @"c:\dev\hoggerday\Assets\Maps\pacman\level1.txt" );

		int i = 0, j = 0;
		int[,] result = new int[32, 29];
		foreach (var row in input.Split('\n')) {
			j = 0;
			foreach (var col in row.Trim().Split(',')) {
				Debug.Log ("another row");

				// result[i, j] = int.Parse(col.Trim());
				try {
					int.TryParse(col.Trim(), out result[i, j]);
					Debug.Log (i.ToString() + "," + j.ToString() + " : " + result[i, j]);
				}
				catch { break;}
				j++;
			}
			i++;
		}
	}

}
