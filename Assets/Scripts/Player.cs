using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public float speed = 5.0f;
	public int initialMoves = 11;

	public Text moveText;
	public Text gameOverText;

	private int _movesLeft;

	private int _roundsWon = 0;

	private Rigidbody _rigidbody;

	private GameController _gameController;

	private enum Directions {
		posX = 0,
		posZ = 1,
		negX = 2,
		negZ = 3
	}
	private Directions _direction = Directions.posX;
	private Vector3 lastloc;
	private Vector3 destination;

	[SerializeField] private int xCoord;
	[SerializeField] private int zCoord;

	private bool isMoving = false;



	public int getX() {
		return xCoord;
	}

	public int getZ() {
		return zCoord;
	}

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody> ();
		_gameController = GameObject.Find ("Controller").GetComponent<GameController>();
		_movesLeft = initialMoves;
		moveText.text = "Round " + _roundsWon;
		lastloc = transform.position;

		xCoord = 5;
		zCoord = 5;
	}

	private bool CheckValidMove(int x, int z) {
		return (_gameController.GetTileColourAt (x,z) == "white");
	}

	// Update is called once per frame
	void Update () {


		if (!_gameController.gameOver && !isMoving) {

//			float h = Input.GetAxis("Horizontal") * speed;
//			float v = Input.GetAxis("Vertical") * speed;
//
//			Vector3 movement = new Vector3 (h, 0, v);
//			movement *= Time.deltaTime;
//
//			_rigidbody.MovePosition (transform.position + movement);

			if (Input.GetKeyDown (KeyCode.RightArrow) && CheckValidMove (xCoord+1, zCoord)) {
//				Vector3 movement = new Vector3 (5f, 0, 0);
//				_rigidbody.MovePosition (transform.position + movement);
				_direction = Directions.posX;
				destination = transform.position + new Vector3(5f,0,0);
				isMoving = true;
				xCoord += 1;
				moveEnemies ();
			} else if (Input.GetKeyDown (KeyCode.LeftArrow) && CheckValidMove (xCoord-1, zCoord)) {
				_direction = Directions.negX;
				destination = transform.position + new Vector3(-5f,0,0);
				isMoving = true;
				xCoord -= 1;
				moveEnemies ();
			} else if (Input.GetKeyDown (KeyCode.UpArrow) && CheckValidMove (xCoord, zCoord+1) ) {
				_direction = Directions.posZ;
				destination = transform.position + new Vector3(0,0,5f);
				isMoving = true;
				zCoord += 1;
				moveEnemies ();
			} else if (Input.GetKeyDown (KeyCode.DownArrow) && CheckValidMove (xCoord, zCoord-1)) {
				_direction = Directions.negZ;
				destination = transform.position + new Vector3(0,0,-5f);
				isMoving = true;
				zCoord -= 1;
				moveEnemies ();
			}
		}
//
		if (isMoving) {
			transform.position = (Vector3.Lerp (transform.position, destination, speed*Time.deltaTime));

			if (transform.position == destination) {
				isMoving = false;
				lastloc = destination;
			}
		}

		if (transform.position.y < 0) {
		
			gameOverText.text = "Game Over!";
			_gameController.gameOver = true;
		}

	}

	void moveEnemies() {
		List<Enemy> list = _gameController.enemyList;
		for (int i = 0; i < list.Count; i++) {
			list [i].Move ();
		}

		_gameController.UpdateGridPositions ();
	}

	void OnTriggerEnter(Collider other)
	{
		//called every time we trigger a collider
		if (other.gameObject.CompareTag ("Tile")) {
			Tile tile = other.gameObject.GetComponent<Tile> ();

			//string c = tile.getColour ();



			tile.setColour ("blue");
//			bool gotMarkers = _gameController.CheckMarkers ();
//
//			if (gotMarkers) {
//				_roundsWon += 1;
//				_movesLeft = initialMoves + 2;
//			} else {
//				_movesLeft -= 1;
//				CheckLose ();
//			}
//			//moveText.text = _movesLeft + " moves left";
//			moveText.text = "Round " + _roundsWon;

		} else if (other.gameObject.CompareTag ("Marker")) {
			Marker marker = other.gameObject.GetComponent<Marker> ();
//			Destroy (marker.block); 
			Destroy (other.gameObject);
			CheckRound ();
			//marker.setColour ("blue");
		} 
	}

//	void OnCollisionEnter(Collision other) {
//		if (other.gameObject.CompareTag ("Block")) {
//			Debug.Log ("hit");
//			//destination = lastloc;
//			isMoving = true;
//			//transform.position = Vector3.Lerp (destination, transform.position, 2*speed*Time.deltaTime);
//			destination = lastloc;
//		}
//	}

	void CheckLose() {
		if (_movesLeft == 0) {
		
			gameOverText.text = "Game Over";
			_gameController.gameOver = true;
		
		}
	}

	void CheckRound() {
		GameObject[] markers = GameObject.FindGameObjectsWithTag ("Marker");
		Debug.Log (markers.Length);
		if (markers.Length == 1) {
			_roundsWon += 1;
			moveText.text = "Round " + _roundsWon;
			_gameController.NextRound ();
		}
	}
}
