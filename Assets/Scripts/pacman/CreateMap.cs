using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

public class CreateMap : MonoBehaviour {
	// Use this for initialization
	void Start () {
		string input = File.ReadAllText( @"c:\dev\hoggerday\Assets\Maps\pacman\level1.txt" );

		int i = 0, j = 0, ii = 0, jj = 0, iioff = 0, jjoff = 0, Zrot=0;
		float Xoff=0, Yoff=0;
		int numrows = 31, numcols = 28;
		int[,] maparray = new int[numrows, numcols];
		string[,] cellBlock = new string[3,3] ;
		string theCode = "", thePrefab = "";

		foreach (var row in input.Split('\n')) {
			j = 0;
			foreach (var col in row.Trim().Split(',')) {
				try {
					int.TryParse(col.Trim(), out maparray[i, j]);
				}
				catch { break;}
				j++;
			}
			i++;
		}
		//probably not efficient, but loop through maparray
		for (i = 0; i < numrows; i++){
			for (j = 0; j < numcols; j++) {
				//initialize cellblock to all 'x'. this feels really inefficient.
				for (ii = 0; ii < 3; ii++){
					for (jj = 0; jj < 3; jj++)
						cellBlock[ii,jj] = "x";
				}
				if (maparray[i,j] == 0) {
					UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/pacman/pacdot.prefab", typeof(GameObject));
					GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
					clone.transform.position = new Vector3(j + 1, numrows - i , 0);
				}
				else { // let's parse further to determine which map piece to add
					for (iioff = -1; iioff < 2; iioff++){
						for (jjoff = -1; jjoff < 2; jjoff++)
							if (i+iioff >=0 && i+iioff < numrows && j+jjoff >=0 && j+jjoff < numcols) //valid i,j indices
								cellBlock[iioff+1, jjoff+1] =  maparray[i+iioff, j+jjoff].ToString();
					}
					Debug.Log(i.ToString() + "," + j.ToString());
					theCode="";
					for (ii = 0; ii < 3; ii++){
						for (jj = 0; jj < 3; jj++)
							theCode=theCode + cellBlock[ii,jj];
					}
					Debug.Log(theCode);

					switch (theCode)
					{
						case "xxxx22x20": //top left corner 2x wall
							thePrefab="Assets/Prefabs/pacman/wall_2x_corner_tl.prefab";
							Zrot = 0;
							Xoff = 0.5f;
							Yoff = -0.5f;
							break;
						case "x20x20x20": //flat 2x wall. left side
							thePrefab="Assets/Prefabs/pacman/wall_2x_flat_l.prefab";
							Zrot = -90;
							Xoff = 0;
							Yoff = 0;
							break;
						default:
							thePrefab="";
							Zrot = 0;
							Xoff = 0;
							Yoff = 0;
							break;
					}	
					if (thePrefab != ""){
						UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(thePrefab, typeof(GameObject));
						Instantiate(prefab, new Vector3(j + Xoff, numrows - i +Yoff, 0), Quaternion.Euler(0, 0, Zrot));
					}
				}
			}
		}
	}

}
