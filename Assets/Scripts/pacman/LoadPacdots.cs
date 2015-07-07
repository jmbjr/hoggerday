using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class LoadPacdots : MonoBehaviour {
	// Use this for initialization
	void Start () {
		string input = File.ReadAllText( @"c:\dev\hoggerday\Assets\Maps\pacman\level1.txt" );

		int i = 0, j = 0;
		int numrows = 33, numcols = 30;
		int[,] result = new int[numrows, numcols];
		foreach (var row in input.Split('\n')) {
			j = 0;
			foreach (var col in row.Trim().Split(',')) {
				try {
					int.TryParse(col.Trim(), out result[i, j]);
					if (result[i,j] == 0) {
						Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/pacman/pacdot.prefab", typeof(GameObject));
						GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
						clone.transform.position = new Vector3(j, numrows - i -1, 0);
					}
				}
				catch { break;}
				j++;
			}
			i++;
		}
	}

}
