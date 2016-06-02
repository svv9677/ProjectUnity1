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
		this.Level = PlayerPrefs.HasKey(Globals.PREF_LEVEL) ? PlayerPrefs.GetInt(Globals.PREF_LEVEL) : 1;
	}

	public void Save()
	{
		PlayerPrefs.SetInt(Globals.PREF_LEVEL, this.Level);
	}

	public override string ToString ()
	{
		return string.Format ("Level: {0}", Level);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			if(DebugMenu.Instance.gameObject.activeSelf)
				DebugMenu.Instance.gameObject.SetActive(false);
			else
				DebugMenu.Instance.gameObject.SetActive(true);
		}
		
	}
}
