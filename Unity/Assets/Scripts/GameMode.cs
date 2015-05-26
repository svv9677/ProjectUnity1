using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum eMode
{
	eModeSplash,
	eModePreview,
	eModeGame,
	eModeResult,
}

public class GameMode : MonoBehaviour {

	private eMode mode;

	public Splash splash;
	public Puzzle puzzle;
	
	// Use this for initialization
	void Start () {
		this.mode = eMode.eModeSplash;
		//this.splash.EnterMode ();
		this.puzzle.EnterMode ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
