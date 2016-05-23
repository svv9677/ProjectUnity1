using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ePuzzleState {
	E_PS_SET_TARGET_PATTERN = 0,
	E_PS_DISPLAY_SHUFFLE_START,
	E_PS_SHUFFLE_PIECES,
	E_PS_SET_STARTING_PATTERN,
	E_PS_DISPLAY_LEVEL_GOALS,
	E_PS_GAME_PLAY,
	E_PS_GAME_RESULTS
}

public class Puzzle : Mode {

	private ePuzzleState PuzzleState;
	private int[] SlotXs;
	private int[] SlotYs;

	public int currentEmptySlotX;
	public int currentEmptySlotY;

	public List<Piece> Pieces;
	 
	// Use this for initialization
	void Start () {

		this.mode = eMode.E_M_PUZZLE;

		this.SlotXs = new int[4];
		this.SlotXs[0] = -441;
		this.SlotXs[1] = -147;
		this.SlotXs[2] =  146;
		this.SlotXs[3] =  440;
		this.SlotYs = new int[4];
		this.SlotYs[0] =  441;
		this.SlotYs[1] =  150;
		this.SlotYs[2] = -150;
		this.SlotYs[3] = -445;
	}

	protected void SetVisible(bool hideFlags)
	{
		foreach (Piece pc in this.Pieces)
			pc.gameObject.SetActive (hideFlags);
		
		this.gameObject.SetActive (hideFlags);
	}
	
	public override void EnterMode()
	{
		this.PuzzleState = ePuzzleState.E_PS_SET_TARGET_PATTERN;
		this.SetVisible (true);
	}

	public override void ExitMode()
	{
		this.SetVisible (false);
	}
	
	protected virtual void OnEnable()
	{
		// Hook into the OnSwipe event
		Lean.LeanTouch.OnFingerSwipe += OnFingerSwipe;
	}
	
	protected virtual void OnDisable()
	{
		// Unhook into the OnSwipe event
		Lean.LeanTouch.OnFingerSwipe -= OnFingerSwipe;
	}

	public Piece GetPieceToSide(string direction)
	{
		int x = -1;
		int y = -1;

		if (direction == "left") {
			x = currentEmptySlotX - 1;
			y = currentEmptySlotY;
		}
		if (direction == "right") {
			x = currentEmptySlotX + 1;
			y = currentEmptySlotY;
		}
		if (direction == "up") {
			x = currentEmptySlotX;
			y = currentEmptySlotY + 1;
		}
		if (direction == "down") {
			x = currentEmptySlotX;
			y = currentEmptySlotY - 1;
		}

		if (x < 0 || x >= 4)
			return null;
		if (y < 0 || y >= 4)
			return null;

		foreach (Piece pc in Pieces) {
			if (pc.SlotX == x && pc.SlotY == y)
				return pc;
		}

		return null;
	}

	public string GetDirection(Vector2 dir)
	{
		dir.Normalize ();
		float x = dir.x;
		float y = dir.y;
		float delta = Mathf.Abs (x) - Mathf.Abs (y);

		if (Mathf.Abs (delta) > 0.5f) {
			if(delta > 0f)
			{
				if(x > 0f)
					return "left";
				else
					return "right";
			}
			else
			{
				if(y > 0f)
					return "up";
				else
					return "down";
			}
		}

		return "";
	}

	public void OnFingerSwipe(Lean.LeanFinger finger)
	{
		Vector2 dir = finger.SwipeDelta;

		string direction = GetDirection (dir);

		OnInput(direction);
	}

	private void OnInput(string direction)
	{
		Piece pc = GetPieceToSide (direction);
		if (pc != null) {
			//Debug.Log ("Direction: " + direction + ", Piece: " + pc.SlotX + "," + pc.SlotY);
			Vector3 pos = pc.transform.localPosition;
			pos.x = SlotXs [currentEmptySlotX];
			pos.y = SlotYs [currentEmptySlotY];
			iTween.MoveTo(pc.gameObject, 
			              iTween.Hash(	"position", pos, 
			            				"islocal", true, 
			            				"time", 0.5f, 
			            				"easeType", "easeOutBounce"));
			int x = pc.SlotX;
			int y = pc.SlotY;
			pc.SlotX = currentEmptySlotX;
			pc.SlotY = currentEmptySlotY;
			currentEmptySlotX = x;
			currentEmptySlotY = y;
		} else
			Debug.Log ("Piece not found to: " + direction + " of " + currentEmptySlotX + "," + currentEmptySlotY);
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			GameMode.Instance.SetMode(eMode.E_M_SPLASH);
		}

		// TODO: Move this into its own state
		if(Input.GetKeyDown(KeyCode.DownArrow))
			OnInput("down");
		if(Input.GetKeyDown(KeyCode.UpArrow))
			OnInput("up");
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			OnInput("right");
		if(Input.GetKeyDown(KeyCode.RightArrow))
			OnInput("left");

		switch(this.PuzzleState)
		{
		case ePuzzleState.E_PS_SET_TARGET_PATTERN:
		{
			LevelData current_level_data = LevelManager.Instance.Levels[Player.Instance.Level];
			foreach (Piece pc in Pieces) 
			{
				Vector3 pos = pc.transform.localPosition;
				int x=0,y=0;
				current_level_data.GetXYForNumber(pc.Number, out x, out y);
				pos.x = SlotXs [x];
				pos.y = SlotYs [y];
				iTween.MoveTo(pc.gameObject, 
				              iTween.Hash(	"position", pos, 
				            "islocal", true, 
				            "time", 0.5f, 
				            "easeType", "easeOutBounce"));
                pc.SlotX = x;
                pc.SlotY = y;
            }
			current_level_data.GetXYForNumber(0, out currentEmptySlotX, out currentEmptySlotY);
			this.PuzzleState = ePuzzleState.E_PS_DISPLAY_SHUFFLE_START;
			break;
		}
		case ePuzzleState.E_PS_DISPLAY_SHUFFLE_START:
		{
		}
			break;
		case ePuzzleState.E_PS_SHUFFLE_PIECES:
		{
		}
			break;
		case ePuzzleState.E_PS_SET_STARTING_PATTERN:
		{
		}
			break;
		case ePuzzleState.E_PS_DISPLAY_LEVEL_GOALS:
		{
		}
			break;
		case ePuzzleState.E_PS_GAME_PLAY:
		{
		}
			break;
		case ePuzzleState.E_PS_GAME_RESULTS:
		{
		}
			break;
		default:
			break;
		}
	}
}
