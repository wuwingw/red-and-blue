using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	//public GameObject[] tiles;
	public int markerNumber = 3;
	public int enemyNumber = 5;
	public int boardDimension = 10;

	// prefabs
	public GameObject tile;
	public GameObject enemy;
	public GameObject marker;
	public GameObject player;
	public GameObject block;

	private Player playerInstance;

	private Tile[,] tileGrid;
	private GameObject[,] objectGrid;

	private List <Vector3> tilePositions = new List<Vector3>();
	private Tile[] tileList = new Tile[1];
	private Marker[] markerList;
	private List<int> takenMarkerPositions;

	public List <Enemy> enemyList = new List<Enemy>();


	public bool gameOver = false;

	// Use this for initialization
	void Start () {

		// generate position of tiles
		for (int x = 0; x < boardDimension; x++) {
			for (int y = 0; y < boardDimension; y++) {
				tilePositions.Add (new Vector3 (5f*x - 22.5f, 0.5f, 5f*y - 22.5f)); 
			}
		}

		playerInstance = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();

		takenMarkerPositions = new List<int> ();
		objectGrid = new GameObject[boardDimension,boardDimension];
		objectGrid [5, 5] = playerInstance.gameObject;

//		tileList = new Tile[tilePositions.Count];
//
//		// fill with random colours
//		for (int x = 0; x < tilePositions.Count; x++) {
//			GameObject o = Instantiate (tile, tilePositions [x], Quaternion.identity) as GameObject;
//			Tile t = o.GetComponent<Tile> ();
//			tileList [x] = t;
//		}

		GenerateTiles ();

		// enemies
		for (int i = 0; i < enemyNumber; i++) {
			AddEnemy ();
		}

		GenerateMarkers ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void GenerateTiles() {

		// destroy the old tiles
		if (tileList.Length > 1) {
			for (int x = 0; x < tileList.Length; x++) {
				Destroy (tileList [x].gameObject);
			}
		}

		// destroy the invisible blocking objects
//		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
//		for (int x = 0; x < blocks.Length; x++) {
//			Destroy (blocks [x]);
//		}

		tileList = new Tile[tilePositions.Count];

		tileGrid = new Tile[boardDimension, boardDimension];

		// fill with random colours
		int xco = 0;
		int yco = 0;
		for (int i = 0; i < tilePositions.Count; i++) {
			GameObject o = Instantiate (tile, tilePositions [i], Quaternion.identity) as GameObject;
			Tile t = o.GetComponent<Tile> ();
			Debug.Log (t);
			tileList [i] = t;
			Debug.Log (t);

			// put the tile in the grid too
			if (yco < boardDimension) {
				tileGrid [xco,yco] = t;
				yco += 1;
			} else { //yco == boardDimension
				yco = 0;
				xco += 1;
				tileGrid [xco,yco] = t;
				yco += 1;
			}

			//Debug.Log (tileGrid [xco, yco]);
		}

		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				Debug.Log (tileGrid [i, j]);
			}
		}
	}

	void AddEnemy() {

		// find a non taken position
		int randomX, randomY;
		do {
			randomX = Random.Range (0, boardDimension);
			randomY = Random.Range (0, boardDimension);
		} while (objectGrid [randomX, randomY] != null);
			
		GameObject o = Instantiate (enemy, tilePositions [boardDimension*randomX + randomY], Quaternion.identity) as GameObject;
		Enemy e = o.GetComponent<Enemy> ();
		e.setX (randomX);
		e.setZ (randomY);
		enemyList.Add (e);
		objectGrid [randomX, randomY] = e.gameObject;

	}

	void AddPlayer() {
		Instantiate (player, tilePositions[Random.Range(0, tilePositions.Count)], Quaternion.identity);
	}

	void GenerateMarkers() {

		markerList = new Marker[markerNumber];

		for (int i = 0; i < markerNumber; i++) {
			
//			int position;
//			do {
//				position = Random.Range (0, tilePositions.Count); 
//			} while (takenMarkerPositions.Contains (position));
//			takenMarkerPositions.Add (position);

			// find a non taken position
			int randomX, randomY;
			do {
				randomX = Random.Range (0, boardDimension);
				randomY = Random.Range (0, boardDimension);
			} while (objectGrid [randomX, randomY] != null);
				
			Vector3 pos = tilePositions [boardDimension*randomX + randomY];
			pos.y = 1;
			GameObject o = Instantiate (marker, pos, Quaternion.identity) as GameObject;
			//Debug.Log (o);
			Marker m = o.GetComponent<Marker> ();
			m.setPosition (boardDimension*randomX + randomY);
			m.setX (randomX);
			m.setZ (randomY);
			objectGrid [randomX, randomY] = m.gameObject;
			GetObjectTypeAt (randomX, randomY);
			//Debug.Log (m);
			markerList [i] = m;

			//m.block = Instantiate (block, pos, Quaternion.identity) as GameObject;


			//this.markerList[i] = Instantiate (marker, pos, Quaternion.identity) as Marker; 
		}

		Debug.Log (markerList.Length);
	
	}

	void DestroyMarkers() {
	
		for (int i = 0; i < markerList.Length; i++) {
			Tile t = tileList [markerList [i].getPosition ()];
			t.setLocked (true);
			//markerList [i].setColour ("black");
			markerList[i]._locked = true;
			markerList [i].transform.Translate (0f, -0.5f, 0f);
			//Destroy (markerList[i].gameObject);
		}

	}

//	public bool CheckMarkers() {
//	
//		string colour = markerList[0].getColour ();
//
//		for (int i = 0; i < this.markerList.Length; i++) {
//			if (markerList [i].getColour () != colour) {
//				return false;
//			}
//		}
//
//		if (colour == "red") {
//			DestroyMarkers ();
//			AddEnemy ();
//			GenerateMarkers ();
//			return false;
//		} else if (colour == "blue") {
//			DestroyMarkers ();
//			AddEnemy ();
//			//AddPlayer (); 
//			GenerateMarkers ();
//			return true;
//		}
//
//		return false;
//
//	}

	public void NextRound() {
		GenerateTiles ();
		markerNumber += 1;
		GenerateMarkers ();
		//AddEnemy ();
	}

	public string GetTileColourAt(int x, int z) {
		Debug.Log(x + " " + z);
		if (x < 0 || x >= boardDimension || z < 0 || z >= boardDimension)
			return "";

		//Debug.Log (tileGrid [x, z]);
		//Debug.Log (tileList[x*boardDimension + z]); 
		return tileGrid [x, z].getColour ();
		//return tileList[x*boardDimension + z].getColour ();
	}

	public string GetObjectTypeAt(int x, int z) {
		if (x < 0 || x > boardDimension || z < 0 || z > boardDimension)
			return "";
		GameObject o = objectGrid [x, z];
		if (o == null) {
			return "";
		} else {
			Debug.Log (o.tag);
			return o.tag;
		}
	}

	public void UpdateGridPositions() {
		objectGrid = new GameObject[boardDimension, boardDimension];

		foreach (Enemy e in enemyList) {
			if (e != null)
				objectGrid [e.getX (), e.getZ ()] = e.gameObject;
		}

		foreach (Marker m in markerList) {
			if (m != null)
				objectGrid [m.getX (), m.getZ ()] = m.gameObject;
		}

		objectGrid [playerInstance.getX (), playerInstance.getZ ()] = playerInstance.gameObject;

	}
}
