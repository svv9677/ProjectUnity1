using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Piece : MonoBehaviour {

	public Text NumberText;
	public int Number;

	public int SlotX;
	public int SlotY;

	// Use this for initialization
	void Start () {
		this.NumberText.text = this.Number.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
