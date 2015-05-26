using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Splash : MonoBehaviour {

	public Image splashImage;

	// Use this for initialization
	void Start () {
		this.SetVisible (false);
	}

	protected void SetVisible(bool hideFlags)
	{
		this.splashImage.gameObject.SetActive (hideFlags);

		this.gameObject.SetActive (hideFlags);
	}

	public void EnterMode()
	{
		this.SetVisible (true);
	}

	public void ExitMode()
	{
		this.SetVisible (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
