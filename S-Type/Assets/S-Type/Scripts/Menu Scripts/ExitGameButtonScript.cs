using UnityEngine;
using System.Collections;

public class ExitGameButtonScript : ButtonScript {
	public override void PressButton() { if(menu != null) Application.LoadLevel ("Leaderboard"); }
}
