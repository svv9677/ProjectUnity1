using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct LevelData
{
	public int Level;
	public int MovesAvailable;
	public int MovesForTwoStars;
	public int MovesForThreeStars;
	public int MovesForShuffle;
	public int PlaySpeed;
	public int[,] Pattern;

	public void LoadFromDict(Dictionary<string, object> data)
	{
		Level = System.Int32.Parse ((string)data ["Level"]);
		MovesAvailable = System.Int32.Parse ((string)data ["Moves Available"]);
		MovesForTwoStars = System.Int32.Parse ((string)data ["2 Star"]);
		MovesForThreeStars = System.Int32.Parse ((string)data ["3 Star"]);
		MovesForShuffle = System.Int32.Parse ((string)data ["Moves"]);
		PlaySpeed = System.Int32.Parse ((string)data ["Play Speed"]);
		Pattern = new int[4, 4];
		string[] str = ((string)data ["Pattern"]).Split(',');
		for(int i=0; i< str.Length; i++)
			Pattern[(i/4),(i%4)] = System.Int32.Parse (str[i]);
	}

	public override string ToString()
	{
		string str = "Level: ";
		str += Level.ToString ();
		str += ", Moves Available: ";
		str += MovesAvailable.ToString ();
		str += ", 2 Star: ";
		str += MovesForTwoStars.ToString ();
		str += ", 3 Star: ";
		str += MovesForThreeStars.ToString ();
		str += ", Total Moves: ";
		str += MovesForShuffle.ToString ();
		str += ", Play Speed: ";
		str += PlaySpeed.ToString ();
		str += ", Pattern: ";
		for (int i=0; i<4; i++)
			for (int j=0; j<4; j++)
				str += " " + Pattern [i, j].ToString ();

		return str;
	}
	/*{
		"Level":"1","Moves Available":"20","2 Star":"15","3 Star":"12","Moves":"10","Play Speed":"1",
		"Pattern":"1,2,3,4, 5,6,7,8, 9,10,11,12, 13,14,15,0"
	  },
	*/
}

public class LevelManager : Singleton<LevelManager>
{
	protected delegate void FileLoadedCallback(int result, string data);
	private List<LevelData> levels;

	// Use this for initialization
	void Start ()
	{
		string FilePath = System.IO.Path.Combine("Blueprints", "db_Levels.json");
		List<object> data = FileUtils.Instance.GetJsonAsset<List<object>>(FilePath);
		levels = new List<LevelData> ();
		for (int i=0; i< data.Count; i++) 
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)data [i];
			LevelData level = new LevelData ();
			level.LoadFromDict (dict);
			levels.Add (level);
			Debug.Log ("Adding level: " + level.ToString());
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

