using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Splash : Mode {

	public Image splashImage;
	public Button startButton;

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
		this.startButton.onClick.AddListener(OnStart);
		this.SetVisible (true);
	}

	public void ExitMode()
	{
		this.startButton.onClick.RemoveListener(OnStart);
		this.SetVisible (false);
	}

	public void OnStart()
	{
		GameMode.Instance.SetMode(eMode.E_M_PUZZLE);
	}
}
