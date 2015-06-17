using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum eMode
{
	E_M_NONE,
	E_M_SPLASH,
	E_M_LEVELS,
	E_M_PUZZLE,
}

public class GameMode : Singleton<GameMode> {

	private eMode mode;

	public Splash splash;
	public Puzzle puzzle;
	
	// Use this for initialization
	void Start () {
		this.mode = eMode.E_M_NONE;
		SetMode (eMode.E_M_SPLASH);
	}
	
	public void SetMode(eMode new_mode)
	{
		ExitMode();
		this.mode = new_mode;
		EnterMode();
	}

	private void ExitMode()
	{
		switch(this.mode)
		{
		case eMode.E_M_SPLASH:
		{
			this.splash.ExitMode();
			break;
		}
		case eMode.E_M_PUZZLE:
		{
			this.puzzle.ExitMode();
			break;
		}
		default:
			break;
		}
	}

	private void EnterMode()
	{
		switch(this.mode)
		{
		case eMode.E_M_SPLASH:
		{
			this.splash.EnterMode();
			break;
		}
		case eMode.E_M_PUZZLE:
		{
			this.puzzle.EnterMode();
			break;
		}
		default:
			break;
		}
	}
}
