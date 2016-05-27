using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Piece : MonoBehaviour {

	public Text NumberText;
	private int _Number;
	public int Number { get { return _Number; } set { this._Number = value; this.NumberText.text = value.ToString (); } }

	public int SlotX;
	public int SlotY;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
