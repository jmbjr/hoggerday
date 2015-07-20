using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;


public class CreateMap : MonoBehaviour {
	public static bool isWall(TileType theType)
	{
		return theType == TileType.SINGLE || theType == TileType.DOUBLE;
	}
	public static bool isEmpty(TileType theType)
	{
		return theType == TileType.BLANK || theType == TileType.PATH;
	}
	public static bool isCorner(boolDir boolWalls)
	{
		return ((boolWalls.TOP && boolWalls.RIGHT && !boolWalls.LEFT && !boolWalls.BOTTOM) ||
		        (boolWalls.TOP && !boolWalls.RIGHT && boolWalls.LEFT && !boolWalls.BOTTOM) ||
		        (!boolWalls.TOP && !boolWalls.RIGHT && boolWalls.LEFT && boolWalls.BOTTOM) ||
		        (!boolWalls.TOP && boolWalls.RIGHT && !boolWalls.LEFT && boolWalls.BOTTOM));
	}
	public static bool isFlat(boolDir boolWalls, TileType theTile)
	{
		return ((boolWalls.TOP &&  boolWalls.BOTTOM && (!boolWalls.RIGHT || !boolWalls.LEFT)) ||
		        (boolWalls.LEFT && boolWalls.RIGHT && (!boolWalls.TOP || !boolWalls.BOTTOM)) );
	}
	public static TileDir cornerDir(boolDir boolWalls, boolDir boolEmpty, TileType thisTile)
	{
		TileDir theWallDir = TileDir.MIDDLE;

		//1x
		switch (thisTile)
		{
		case TileType.SINGLE:
			if (boolWalls.TOP && boolWalls.RIGHT && boolEmpty.BOTTOMLEFT)
				theWallDir = TileDir.BOTTOMLEFT; //1x
			else if (boolWalls.TOP && boolWalls.LEFT && boolEmpty.BOTTOMRIGHT)
				theWallDir = TileDir.BOTTOMRIGHT; //1x
			else if (boolWalls.BOTTOM && boolWalls.RIGHT && boolEmpty.TOPLEFT)
				theWallDir = TileDir.TOPLEFT; //1x
			else if (boolWalls.BOTTOM && boolWalls.LEFT && boolEmpty.TOPRIGHT)
				theWallDir = TileDir.TOPRIGHT; //1x
			break;

		case TileType.DOUBLE:
			//2x
			if (boolWalls.TOP && boolWalls.RIGHT && boolEmpty.TOPRIGHT)
				theWallDir = TileDir.BOTTOMLEFT; //2x
			else if (boolWalls.TOP && boolWalls.LEFT && boolEmpty.TOPLEFT)
				theWallDir = TileDir.BOTTOMRIGHT;  //2x
			else if (boolWalls.BOTTOM && boolWalls.RIGHT && boolEmpty.BOTTOMRIGHT)
				theWallDir = TileDir.TOPLEFT;  //2x
			else if (boolWalls.BOTTOM && boolWalls.LEFT && boolEmpty.BOTTOMLEFT)
				theWallDir = TileDir.TOPRIGHT; //2x
			break;
		}
		return theWallDir;
	}
	public static TileDir flatDir(boolDir boolWalls, boolDir boolEmpty, TileType thisTile)
	{
		TileDir theWallDir = TileDir.MIDDLE;

		//1x
		switch (thisTile)
		{
		case TileType.SINGLE:
			if (boolWalls.TOP && boolWalls.BOTTOM && boolEmpty.LEFT)
				theWallDir = TileDir.LEFT; //1x
			else if (boolWalls.TOP && boolWalls.BOTTOM && boolEmpty.RIGHT)
				theWallDir = TileDir.RIGHT; //1x
			else if (boolWalls.LEFT && boolWalls.RIGHT && boolEmpty.TOP)
				theWallDir = TileDir.TOP; //1x
			else if (boolWalls.LEFT && boolWalls.RIGHT && boolEmpty.BOTTOM)
				theWallDir = TileDir.BOTTOM; //1x
			break;
		case TileType.DOUBLE:
			//2x
			if (boolWalls.TOP && boolWalls.BOTTOM && boolEmpty.RIGHT)
				theWallDir = TileDir.LEFT; //2x
			else if (boolWalls.TOP && boolWalls.BOTTOM && boolEmpty.LEFT)
				theWallDir = TileDir.RIGHT; //2x
			else if (boolWalls.LEFT && boolWalls.RIGHT && boolEmpty.TOP)
				theWallDir = TileDir.BOTTOM; //2x
			else if (boolWalls.LEFT && boolWalls.RIGHT && boolEmpty.BOTTOM)
				theWallDir = TileDir.TOP; //2x
			break;
		}
		return theWallDir;
	}
	public static TileDir flipDir(TileDir theWallDir)
	{
		if (theWallDir == TileDir.TOP)
			return TileDir.BOTTOM;
		else if (theWallDir == TileDir.BOTTOM)
			return TileDir.TOP;
		else if (theWallDir == TileDir.LEFT)
			return TileDir.RIGHT;
		else if (theWallDir == TileDir.RIGHT)
			return TileDir.LEFT;
		else if (theWallDir == TileDir.TOPRIGHT)
			return TileDir.BOTTOMLEFT;
		else if (theWallDir == TileDir.BOTTOMLEFT)
			return TileDir.TOPRIGHT;
		else if (theWallDir == TileDir.TOPLEFT)
			return TileDir.BOTTOMRIGHT;
		else if (theWallDir == TileDir.BOTTOMRIGHT)
			return TileDir.TOPLEFT;
		else if (theWallDir == TileDir.MIDDLE)
			return TileDir.MIDDLE;	
		else 
			return theWallDir;
	}
	public static boolDir checkWalls(TileType[] mapBlock)
	{
		boolDir boolWalls = new boolDir();

		boolWalls.TOP = isWall(mapBlock[(int)TileDir.TOP]);
		boolWalls.LEFT = isWall(mapBlock[(int)TileDir.LEFT]);
		boolWalls.BOTTOM = isWall(mapBlock[(int)TileDir.BOTTOM]);
		boolWalls.RIGHT = isWall(mapBlock[(int)TileDir.RIGHT]);
		
		boolWalls.TOPLEFT = isWall(mapBlock[(int)TileDir.TOPLEFT]);
		boolWalls.TOPRIGHT = isWall(mapBlock[(int)TileDir.TOPRIGHT]);
		boolWalls.BOTTOMLEFT = isWall(mapBlock[(int)TileDir.BOTTOMLEFT]);
		boolWalls.BOTTOMRIGHT = isWall(mapBlock[(int)TileDir.BOTTOMRIGHT]);

		return boolWalls;
	}
	public static boolDir checkEmpty(TileType[] mapBlock)
	{
		boolDir boolEmpty = new boolDir();
		
		boolEmpty.TOP = isEmpty(mapBlock[(int)TileDir.TOP]);
		boolEmpty.LEFT = isEmpty(mapBlock[(int)TileDir.LEFT]);
		boolEmpty.BOTTOM = isEmpty(mapBlock[(int)TileDir.BOTTOM]);
		boolEmpty.RIGHT = isEmpty(mapBlock[(int)TileDir.RIGHT]);
		
		boolEmpty.TOPLEFT = isEmpty(mapBlock[(int)TileDir.TOPLEFT]);
		boolEmpty.TOPRIGHT = isEmpty(mapBlock[(int)TileDir.TOPRIGHT]);
		boolEmpty.BOTTOMLEFT = isEmpty(mapBlock[(int)TileDir.BOTTOMLEFT]);
		boolEmpty.BOTTOMRIGHT = isEmpty(mapBlock[(int)TileDir.BOTTOMRIGHT]);
		
		return boolEmpty;
	}
	public prefabInfo setPrefab(WallInfo theWallInfo)
	{
		prefabInfo thisPrefab = new prefabInfo();

		thisPrefab.prefab="";
		thisPrefab.Zrot = 0;
		thisPrefab.Xoff = 0;
		thisPrefab.Yoff = 0;
		thisPrefab.Xscale = 1.0f;
		thisPrefab.Yscale= 1.0f;

		switch (theWallInfo.Shape)
		{
		case WallShape.CORNER:
			switch (theWallInfo.Type)
			{
			case TileType.SINGLE:
				thisPrefab.prefab = "Assets/Prefabs/pacman/wall_1x_corner.prefab";
				break;
			case TileType.DOUBLE:
				thisPrefab.prefab =  "Assets/Prefabs/pacman/wall_2x_corner.prefab";
				break;
			}
			switch (theWallInfo.Dir)
			{
			case TileDir.TOPRIGHT:
				thisPrefab.Zrot = -90;
				break;
			case TileDir.TOPLEFT:
				thisPrefab.Zrot = 0;
				break;
			case TileDir.BOTTOMRIGHT:
				thisPrefab.Zrot = 180;
				break;
			case TileDir.BOTTOMLEFT:
				thisPrefab.Zrot = 90;
				break;
			default:
				thisPrefab.Zrot = -45;
				break;
			}
			break;

		case WallShape.FLAT:
			switch (theWallInfo.Type)
			{
			case TileType.SINGLE:
				thisPrefab.prefab = "Assets/Prefabs/pacman/wall_1x_flat.prefab";
				break;
			case TileType.DOUBLE:
				thisPrefab.prefab =  "Assets/Prefabs/pacman/wall_2x_flat.prefab";
				break;
			}
			switch (theWallInfo.Dir)
			{
			case TileDir.TOP:
				thisPrefab.Zrot = 0;
				break;
			case TileDir.LEFT:
				thisPrefab.Zrot = 90;
				break;
			case TileDir.RIGHT:
				thisPrefab.Zrot = -90;
				break;
			case TileDir.BOTTOM:
				thisPrefab.Zrot = 180;
				break;
			default:
				thisPrefab.Zrot = -45;
				break;
			}
			break;
		}
		return thisPrefab;
	}


	//enums
	#region
	public enum TileType
	{
		PATH,
		SINGLE,
		DOUBLE,
		THIN,
		GATE,
		FIVE,
		SIX,
		SEVEN,
		EIGHT,
		BLANK,
		OFFMAP,
	};
	public enum WallShape
	{
		NONE,
		FLAT,
		CORNER,
		CORNER2,
		TEE,
	};
	public enum TileDir
	{
		TOPLEFT,
		TOP,
		TOPRIGHT,
		LEFT,
		MIDDLE,
		RIGHT,
		BOTTOMLEFT,
		BOTTOM,
		BOTTOMRIGHT,
	};
	#endregion

	public struct WallInfo {
		public WallShape Shape;
		public TileDir Dir;
		public TileType Type;
	}
	public struct boolDir {
		public bool TOPLEFT;
		public bool TOP;
		public bool TOPRIGHT;
		public bool LEFT;
		public bool MIDDLE;
		public bool RIGHT;
		public bool BOTTOMLEFT;
		public bool BOTTOM;
		public bool BOTTOMRIGHT;
	}
	public struct prefabInfo {
		public string prefab;
		public int Zrot;
		public float Xoff;
		public float Yoff;
		public float Xscale;
		public float Yscale;
	}

	// Use this for initialization
	void Start () {
		string input = File.ReadAllText( "Assets/Maps/pacman/level1.txt" );

		int i = 0, j = 0, ii = 0, jj = 0, iioff = 0, jjoff = 0, Zrot=0;
		float Xoff=0, Yoff=0, Xscale = 1, Yscale = 1;
		int numrows = 31, numcols = 28, iineighbor = 0;
		int[,] maparray = new int[numrows, numcols];
		string[,] cellBlock = new string[3,3] ;
		TileType[] theNeighbors = new TileType[9] ;
		WallInfo theWallInfo = new WallInfo();
		prefabInfo thisPrefab = new prefabInfo();

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
				TileType theTile = (TileType)maparray[i,j];
				iineighbor = 0;
				//initialize cellblock to all 'x'. this feels really inefficient.
				for (ii = 0; ii < 3; ii++){
					for (jj = 0; jj < 3; jj++){
						cellBlock[ii,jj] = "x";
						theNeighbors[iineighbor] = TileType.OFFMAP;
						iineighbor++;
					}

				}
				//get neighbors
				iineighbor = 0;
				for (iioff = -1; iioff < 2; iioff++){
					for (jjoff = -1; jjoff < 2; jjoff++) {
						if (i+iioff >=0 && i+iioff < numrows && j+jjoff >=0 && j+jjoff < numcols) { //valid i,j indices 
							cellBlock[iioff+1, jjoff+1] =  maparray[i+iioff, j+jjoff].ToString();
							theNeighbors[iineighbor] = (TileType)maparray[i+iioff, j+jjoff];
						}
						iineighbor++;
					}
				}
				//expand to single string
				theCode="";
				for (ii = 0; ii < 3; ii++){
					for (jj = 0; jj < 3; jj++)
						theCode=theCode + cellBlock[ii,jj];
				}

				#region 
				switch (theTile)
				{
				case TileType.PATH:
					UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/pacman/pacdot.prefab", typeof(GameObject));
					GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
					clone.transform.position = new Vector3(j + 1, numrows - i , 0);
					break;
				case TileType.BLANK:
					break;
				case TileType.SINGLE:// let's parse further to determine which map piece to add
				case TileType.DOUBLE:
					theWallInfo = getWallShape(theNeighbors);
					thisPrefab = setPrefab(theWallInfo);

					thePrefab = thisPrefab.prefab;
					Zrot = thisPrefab.Zrot;
					Xoff = thisPrefab.Xoff;
					Yoff = thisPrefab.Yoff;
					Xscale = thisPrefab.Xscale;
					Yscale = thisPrefab.Yscale;

				if (thePrefab != ""){
						//Debug.Log (i.ToString()+","+j.ToString()+":"+thePrefab);
						UnityEngine.Object prefab2 = AssetDatabase.LoadAssetAtPath(thePrefab, typeof(GameObject));
						GameObject clone2 = Instantiate(prefab2, new Vector3(j + Xoff+1, numrows - i +Yoff, 0), Quaternion.Euler(0, 0, Zrot)) as GameObject;
						clone2.transform.localScale = new Vector3(Xscale,Yscale,1);
				}
					break;

				default:
					break;
				}
				#endregion
			}
		}
	}

	public WallInfo getWallShape (TileType[] mapBlock)
	{
		TileType thisTile = mapBlock[(int)TileDir.MIDDLE];
		WallShape theWallShape = WallShape.NONE;
		TileDir theWallDir = TileDir.MIDDLE;
		WallInfo theWallInfo;
		boolDir boolWalls = new boolDir();
		boolDir boolEmpty = new boolDir();

		boolWalls = checkWalls(mapBlock);
		boolEmpty = checkEmpty(mapBlock);

		//check for corners
		if (isCorner (boolWalls)) {
			theWallShape = WallShape.CORNER;
			theWallDir = cornerDir(boolWalls, boolEmpty, thisTile);
		}
		//check for tees before flats since tees almost look like flats
		else if (isTee(boolWalls, boolEmpty)) {
			theWallShape = WallShape.TEE;
			theWallDir = teeDir(boolWalls, boolEmpty, thisTile);
		}
		//check for flats
		else if (isFlat(boolWalls)){
			theWallShape = WallShape.FLAT;
			theWallDir = flatDir(boolWalls, boolEmpty, thisTile);
		}
		else
			theWallShape = WallShape.NONE;

		theWallInfo.Shape = theWallShape;
		theWallInfo.Dir = theWallDir;
		theWallInfo.Type = thisTile;

		return theWallInfo;
	}

}
