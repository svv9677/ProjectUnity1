using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ePuzzleState {
	E_PS_TARGET_PATTERN_TOAST = 0,
	E_PS_TARGET_PATTERN_TOAST_WAITING,
	E_PS_SET_TARGET_PATTERN,
	E_PS_DISPLAY_TARGET_PATTERN,
	E_PS_DISPLAY_SHUFFLE_START,
	E_PS_DISPLAY_SHUFFLE_START_WAITING,
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

	private System.DateTime ShuffleStartTime;
	private int ShuffleMovesDone;
	 
	// Use this for initialization
	void Start () {

		this.mode = eMode.E_M_PUZZLE;

		this.SlotXs = new int[4];
		this.SlotXs[0] = -317;
		this.SlotXs[1] = -105;
		this.SlotXs[2] =  107;
		this.SlotXs[3] =  318;
		this.SlotYs = new int[4];
		this.SlotYs[0] =  322;
		this.SlotYs[1] =  109;
		this.SlotYs[2] = -105;
		this.SlotYs[3] = -318;

		this.Pieces.Clear();
		GameObject pc_prefab = Resources.Load ("Prefabs/Piece") as GameObject;
		if(pc_prefab == null)
		{
			Debug.LogError("Failed to load Piece prefab!!");
			return;
		}
		for(int i=0; i<15; i++)
		{
			GameObject pc_obj = Instantiate(pc_prefab);
			pc_obj.transform.SetParent(this.gameObject.transform);
			pc_obj.transform.localScale = Vector3.one;
			pc_obj.transform.localPosition = Vector3.zero;
			pc_obj.transform.SetSiblingIndex(1);
			Piece pc = pc_obj.GetComponent<Piece>();
			pc.Number = i+1;
			this.Pieces.Add (pc);
		}
	}

	public override string ToString ()
	{
		return string.Format ("State: {0}", PuzzleState);
	}

	protected void SetVisible(bool hideFlags)
	{
		foreach (Piece pc in this.Pieces)
			pc.gameObject.SetActive (hideFlags);
		
		this.gameObject.SetActive (hideFlags);

		HUD.Instance.gameObject.SetActive(hideFlags);
	}
	
	public override void EnterMode()
	{
		this.PuzzleState = ePuzzleState.E_PS_TARGET_PATTERN_TOAST;
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
	
	private void ShufflePiece(LevelData current_level_data)
	{
		string direction = "up";
		direction = current_level_data.ShuffleGuide[this.ShuffleMovesDone];
		this.ShuffleMovesDone++;

		Debug.Log ("Shuffling: " + direction + ". ShuffleMovesDone: " + this.ShuffleMovesDone.ToString());
		OnInput(direction);
	}

	public void OnTargetPatternToast()
	{
		this.PuzzleState = ePuzzleState.E_PS_SET_TARGET_PATTERN;
	}

	public void OnShuffleToast()
	{
		this.PuzzleState = ePuzzleState.E_PS_SHUFFLE_PIECES;
		this.ShuffleStartTime = System.DateTime.Now;
		this.ShuffleMovesDone = 0;
	}

	IEnumerator OnDisplayTargetPattern()
	{
		yield return new WaitForSeconds(3.0f);
		this.PuzzleState = ePuzzleState.E_PS_DISPLAY_SHUFFLE_START;
		yield return null;
	}


	// Update is called once per frame
	void Update () 
	{
		LevelData current_level_data = LevelManager.Instance.GetLevelData(Player.Instance.Level);
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			GameMode.Instance.SetMode(eMode.E_M_SPLASH);
		}

		switch(this.PuzzleState)
		{
		case ePuzzleState.E_PS_TARGET_PATTERN_TOAST:
		{
			Globals.ShowToast("Level " + current_level_data.Level.ToString() + "\nTARGET PATTERN", Globals.TOAST_MEDIUM, OnTargetPatternToast);
				this.PuzzleState = ePuzzleState.E_PS_TARGET_PATTERN_TOAST_WAITING;
			break;
		}
		case ePuzzleState.E_PS_SET_TARGET_PATTERN:
		{
			Debug.Log ("Setting Target pattern to : " + current_level_data.ToString());
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
			this.PuzzleState = ePuzzleState.E_PS_DISPLAY_TARGET_PATTERN;
			StartCoroutine(OnDisplayTargetPattern());
			break;
		}
		case ePuzzleState.E_PS_DISPLAY_SHUFFLE_START:
		{
			Globals.ShowToast("SHUFFLING PIECES", Globals.TOAST_MEDIUM, OnShuffleToast);
			this.PuzzleState = ePuzzleState.E_PS_DISPLAY_SHUFFLE_START_WAITING;
			break;
		}
		case ePuzzleState.E_PS_SHUFFLE_PIECES:
		{
			System.TimeSpan ts = System.DateTime.Now - this.ShuffleStartTime;
			if(ts.TotalSeconds > (int)(this.ShuffleMovesDone/current_level_data.PlaySpeed))
			//if(Input.GetKeyDown(KeyCode.Space))
			{
				ShufflePiece(current_level_data);
				if(this.ShuffleMovesDone >= current_level_data.MovesForShuffle)
				{
					Debug.Log ("Done Shuffling: " + this.ShuffleMovesDone.ToString() + " <=> " + current_level_data.MovesForShuffle.ToString());
					this.PuzzleState = ePuzzleState.E_PS_SET_STARTING_PATTERN;
				}
			}
			break;
		}
		case ePuzzleState.E_PS_SET_STARTING_PATTERN:
		{
			#if UNITY_EDITOR
			// TODO: Move this into its own state
			if(Input.GetKeyDown(KeyCode.DownArrow))
				OnInput("down");
			if(Input.GetKeyDown(KeyCode.UpArrow))
				OnInput("up");
			if(Input.GetKeyDown(KeyCode.LeftArrow))
				OnInput("right");
			if(Input.GetKeyDown(KeyCode.RightArrow))
				OnInput("left");
			#endif
			
			break;
		}
		case ePuzzleState.E_PS_DISPLAY_LEVEL_GOALS:
		{
			break;
		}
		case ePuzzleState.E_PS_GAME_PLAY:
		{
			break;
		}
		case ePuzzleState.E_PS_GAME_RESULTS:
		{
			break;
		}
		default:
			break;
		}
	}
}
