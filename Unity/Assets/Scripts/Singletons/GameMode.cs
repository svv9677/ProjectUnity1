﻿using UnityEngine;
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

	[HideInInspector]
	public Mode modeObject = null;
	public eMode mode { get; private set; }

	public Puzzle puzzle;
	public Splash splash;

	// Use this for initialization
	void Start () {
		this.puzzle.gameObject.SetActive(false);
		this.splash.gameObject.SetActive(false);

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
		if(this.mode != eMode.E_M_NONE)
		{
			this.modeObject.ExitMode();
			this.modeObject = null;
		}
	}

	private void EnterMode()
	{
		switch(this.mode)
		{
		case eMode.E_M_SPLASH:
		{
			this.modeObject = this.splash;
			break;
		}
		case eMode.E_M_PUZZLE:
		{
			this.modeObject = this.puzzle;
			break;
		}
		default:
			break;
		}

		if(this.modeObject != null)
		{
			this.modeObject.gameObject.SetActive(true);
			this.modeObject.EnterMode();
		}
	}
}
