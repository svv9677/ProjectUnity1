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
	public List<string> ShuffleGuide;

	public void LoadFromDict(Dictionary<string, object> data)
	{
		Level = System.Int32.Parse ((string)data ["Level"]);
		MovesAvailable = System.Int32.Parse ((string)data ["Moves Available"]);
		MovesForTwoStars = System.Int32.Parse ((string)data ["2 Star"]);
		MovesForThreeStars = System.Int32.Parse ((string)data ["3 Star"]);
		MovesForShuffle = System.Int32.Parse ((string)data ["Moves"]);
		PlaySpeed = System.Int32.Parse ((string)data ["Play Speed"]);
		Pattern = new int[4, 4];
		string[] str1 = ((string)data ["Pattern"]).Split(',');
		for(int i=0; i< str1.Length; i++)
			Pattern[(i/4),(i%4)] = System.Int32.Parse (str1[i]);
		string[] str2 = ((string)data ["Shuffle Guide"]).Split(',');
		ShuffleGuide = new List<string>();
		for(int i=0; i< str2.Length; i++)
		{
			if(str2[i] == "d")
				ShuffleGuide.Add ("up");
			if(str2[i] == "u")
				ShuffleGuide.Add ("down");
			if(str2[i] == "l")
				ShuffleGuide.Add ("left");
			if(str2[i] == "r")
				ShuffleGuide.Add ("right");
		}
	}

	public void GetXYForNumber(int number, out int x, out int y)
	{
		x = 0; y = 0;
		for(int i=0; i<4; i++)
		{
			for(int j=0; j<4; j++)
			{
				if(Pattern[i,j] == number)
				{
					x = j;
					y = i;
					return;
				}
			}
		}
	}

	public override string ToString()
	{
		System.Text.StringBuilder str = new System.Text.StringBuilder();
		str.Append(Level.ToString ());
		str.Append(", Moves Available: ");
		str.Append(MovesAvailable.ToString ());
		str.Append(", 2 Star: ");
		str.Append(MovesForTwoStars.ToString ());
		str.Append(", 3 Star: ");
		str.Append(MovesForThreeStars.ToString ());
		str.Append("\n Total Moves: ");
		str.Append(MovesForShuffle.ToString ());
		str.Append(", Play Speed: ");
		str.Append(PlaySpeed.ToString ());
		str.Append("\n Pattern: ");
		for (int i=0; i<4; i++)
			for (int j=0; j<4; j++)
				str.Append(" " + Pattern [i, j].ToString ());
		str.Append("\n Shuffle Guide: ");
		for (int i=0; i<ShuffleGuide.Count; i++)
			str.Append(" " + ShuffleGuide[i]);

		return str.ToString();
	}
	/*{
		"Level":"1","Moves Available":"20","2 Star":"15","3 Star":"12","Moves":"10","Play Speed":"1",
		"Pattern":"1,2,3,4, 5,6,7,8, 9,10,11,12, 13,14,15,0", "Shuffle Guide":"up...."
	  },
	*/
}

public class LevelManager : Singleton<LevelManager>
{
	protected delegate void FileLoadedCallback(int result, string data);
	public Dictionary<int, LevelData> Levels;

	// Use this for initialization
	public void Load()
	{
		string FilePath = System.IO.Path.Combine("Blueprints", "db_Levels.json");
		List<object> data = FileUtils.Instance.GetJsonAsset<List<object>>(FilePath);
		Levels = new Dictionary<int,LevelData> ();
		for (int i=0; i< data.Count; i++) 
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)data [i];
			LevelData level = new LevelData ();
			level.LoadFromDict (dict);
			Levels.Add (level.Level, level);
			//Debug.Log ("Adding level: " + level.ToString());
		}
	}

	public LevelData GetLevelData(int level)
	{
		LevelData data;
		Levels.TryGetValue(level, out data);
		// TODO: Error check for invalid data!
		return data;
	}

	public override void Destroy()
	{
		Levels = null;

		base.Destroy();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

