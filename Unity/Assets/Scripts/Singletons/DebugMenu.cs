using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class DebugMenu : Singleton<DebugMenu>
{
	public Text theText;

	public void Update()
	{
		StringBuilder sb = new StringBuilder();

		sb.Append("<b>Screen</b>: "); sb.Append(Screen.width); sb.Append(", "); sb.Append(Screen.height); sb.AppendLine();

		sb.Append("<b>Globals</b>: "); sb.Append(Globals.ToString()); sb.AppendLine();
		sb.Append("<b>Player</b>: "); sb.Append(Player.Instance.ToString()); sb.AppendLine();
		sb.Append("<b>Puzzle</b>: "); sb.Append(GameMode.Instance.puzzle.ToString()); sb.AppendLine();
		LevelData lvlData = LevelManager.Instance.GetLevelData(Player.Instance.Level);
		sb.Append("<b>Level</b>: "); sb.Append(lvlData.ToString()); sb.AppendLine();

		theText.text = sb.ToString();

	}
}

