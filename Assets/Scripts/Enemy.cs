using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public float speed = 3.0f;
	public float directionTime = 300;

	public float maxPos = 20;

	private Player playerInstance;

	private bool _alive;
	private enum Directions {
		posX = 0,
		posZ = 1,
		negX = 2,
		negZ = 3
	}
	private Directions _direction = Directions.posX;
	private float _directionTimer;

	[SerializeField] private int xCoord;
	[SerializeField] private int zCoord;
	private Vector3 destination;
	private Vector3 lastloc;
	private bool isMoving;

	private GameController _gameController;

	// Use this for initialization
	void Start () {
		_alive = true;
		_gameController = GameObject.Find ("Controller").GetComponent<GameController>();

		playerInstance = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();

		Debug.Log (transform.position.x);

//		xCoord = (int)(transform.position.x + 22.5) / 5;
//		zCoord = (int)(transform.position.z + 22.5) / 5;
	}

	public void setX(int x) {
		xCoord = x;
	}

	public void setZ(int z) {
		zCoord = z;
	}

	public int getX() {
		return xCoord;
	}

	public int getZ() {
		return zCoord;
	}

	private bool CheckValidMove(int x, int z) {
		return (_gameController.GetTileColourAt (x,z) == "white") && (_gameController.GetObjectTypeAt (x,z) == "") && (playerInstance.getX() != x) && (playerInstance.getZ() != z);
	}

	public void Move() {
		if (!isMoving) {

			List<Directions> possibles = new List<Directions>{Directions.posX, Directions.negX, Directions.posZ, Directions.negZ};

			//int currentX = (int) (transform.position.x + 22.5) / 5;
			//int currentZ = (int) (transform.position.z + 22.5) / 5;
			int currentX = xCoord;
			int currentZ = zCoord;
	
			if (transform.position.x > maxPos || !CheckValidMove (currentX+1, currentZ))
				possibles.Remove (Directions.posX);
			if (transform.position.x < -maxPos || !CheckValidMove(currentX-1, currentZ))
				possibles.Remove (Directions.negX); 
			if (transform.position.z > maxPos || !CheckValidMove(currentX, currentZ+1))
				possibles.Remove (Directions.posZ);
			if (transform.position.z < -maxPos || !CheckValidMove(currentX, currentZ-1))
				possibles.Remove (Directions.negZ); 

			if (possibles.Count == 0) {
				return;
			}

			int random = Random.Range (0, possibles.Count);
			_direction = possibles [random];


//			if (random < 0.25f) {
//				_direction = (transform.position.x < 20) ? Directions.posX : Directions.negX;
//				//_direction = Directions.posX;
//			} else if (random < 0.5f) {
//				_direction = (transform.position.z < 20) ? Directions.posZ : Directions.negZ;
//			} else if (random < 0.75f) {
//				_direction = (transform.position.x > -20) ? Directions.negX : Directions.posX;
//			} else {
//				_direction = (transform.position.z > -20) ? Directions.negZ : Directions.posZ;
//			}

			if (_direction == Directions.posX) {
				destination = transform.position + new Vector3 (5f, 0, 0);
				xCoord += 1;
				//destination = new Vector3(5*(currentX+1)-22.5,0,transform.position.z);
			} else if (_direction == Directions.negX) {
				destination = transform.position + new Vector3 (-5f, 0, 0);
				xCoord -= 1;
			} else if (_direction == Directions.posZ) {
				destination = transform.position + new Vector3 (0, 0, 5f);
				zCoord += 1;
			} else {
				destination = transform.position + new Vector3 (0, 0, -5f);
				zCoord -= 1;
			}

			isMoving = true;

			Debug.Log ("move. at: " + xCoord + ", " + zCoord);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
//		if (_alive) {
//
//			if (_directionTimer > directionTime) {
//
//				float random = Random.value;
//				if (random < 0.25f) {
//					_direction = Directions.posX;
//				} else if (random < 0.5f) {
//					_direction = Directions.posZ;
//				} else if (random < 0.75f) {
//					_direction = Directions.negX;
//				} else {
//					_direction = Directions.negZ;
//				}
//
//				_directionTimer = 0;
//
//			} else {
//
//				if (transform.position.x >= 22.5) {
//					_direction = Directions.negX;
//				} else if (transform.position.x <= -22.5) {
//					_direction = Directions.posX;
//				} else if (transform.position.z >= 22.5) {
//					_direction = Directions.negZ;
//				} else if (transform.position.z <= -22.5) {
//					_direction = Directions.posZ;
//				} 
//			
//				Vector3 movement;
//
//				if (_direction == Directions.posX) {
//					movement = new Vector3 (speed, 0, 0);
//				} else if (_direction == Directions.negX) {
//					movement = new Vector3 (-speed, 0, 0);
//				} else if (_direction == Directions.posZ) {
//					movement = new Vector3 (0, 0, speed);
//				} else {
//					movement = new Vector3 (0, 0, -speed);
//				}
//
//				movement = movement * Time.deltaTime;
//				transform.Translate (movement);
//			
//			}
//
//			_directionTimer += 1;

		if (isMoving) {
			transform.position = (Vector3.Lerp (transform.position, destination, speed*Time.deltaTime));

			if (transform.position == destination) {
				isMoving = false;
				lastloc = destination;
			}
		}

		if (transform.position == destination) {
			isMoving = false;
		}
				


	}

	void OnTriggerEnter(Collider other)
	{
		//called every time we trigger a collider
		if (other.gameObject.CompareTag ("Tile"))
		{
			Tile tile = other.gameObject.GetComponent<Tile> ();
			tile.setColour ("red");
		} else if (other.gameObject.CompareTag ("Marker")) {
//			Marker marker = other.gameObject.GetComponent<Marker> ();
//			marker.setColour ("red");
//			_gameController.CheckMarkers ();
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
//
//	}


}
