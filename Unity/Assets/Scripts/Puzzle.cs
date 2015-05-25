using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Puzzle : MonoBehaviour {

	private int[] SlotXs;
	private int[] SlotYs;

	public int currentEmptySlotX;
	public int currentEmptySlotY;

	public List<Piece> Pieces;

	// Use this for initialization
	void Start () {
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
	void Update () {
	
	}
}
