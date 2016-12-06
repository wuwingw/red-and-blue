using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public Material redMaterial;
	public Material blueMaterial;
	public Material whiteMaterial;

	public GameObject redBlockTile;
	public GameObject blueBlockTile;

	private Renderer _renderer;
	private BoxCollider _collider;

	private bool _locked;

//	private enum Colour
//	{
//		white = 0,
//		blue = 1,
//		red = 2
//	}
//	private Colour colour = Colour.white;

	private string _colour;

	// Use this for initialization
	void Start () {
		_renderer = GetComponent<Renderer> ();
		_collider = GetComponent<BoxCollider> ();

		_renderer.material = whiteMaterial;
		_colour = "white";

		_locked = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setColour(string c) {
		if (c == "white" || (!_locked && _colour == "white")) {
			if (c == "red") {
				//_collider.size = new Vector3 (1, 50, 1);
				//Instantiate (redBlockTile,transform.position,Quaternion.identity);
				_renderer.material = redMaterial;
				_locked = true;
			} else if (c == "blue") {
				//Instantiate (blueBlockTile,transform.position,Quaternion.identity);
				_renderer.material = blueMaterial;
				_locked = true;
			} else if (c == "white") {
				_renderer.material = whiteMaterial;
			}
			_colour = c;
		}
	}

	public void setLocked(bool b) {
		Debug.Log ("set locked!!");
		_locked = b;
	}

	public string getColour() {
		return _colour;
	}
}
