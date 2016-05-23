using UnityEngine;
using System.Collections;

public class Player : Singleton<Player>
{
	public int Level { get; private set; }

	// Use this for initialization
	void Start ()
	{

	}

	public void Load()
	{
		this.Level = PlayerPrefs.HasKey(Globals.PREF_LEVEL) ? PlayerPrefs.GetInt(Globals.PREF_LEVEL) : 2;
	}

	public void Save()
	{
		PlayerPrefs.SetInt(Globals.PREF_LEVEL, this.Level);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
