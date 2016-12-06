using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {

	public Material redMaterial;
	public Material blueMaterial;
	public Material yellowMaterial;
	public Material blackMaterial;

	public bool _locked = false;

	public GameObject block;

	private Renderer _renderer;
	private string _colour;

	private int _position;
	private int xCoord;
	private int zCoord;

	// Use this for initialization
	void Start () {
		_renderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setPosition(int i) {
		_position = i;
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

	public int getPosition() {
		return _position;
	}

	public string getColour() {
		return _colour;
	}

	public void setColour(string s) {
		if (!_locked) {

			if (s == "blue") {
				_renderer.material = blueMaterial;
			} else if (s == "red") {
				_renderer.material = redMaterial;
			} else if (s == "black") {
				_renderer.material = blackMaterial;
			}
			_colour = s;
		}
	
	}
}
