using UnityEngine;
using System.Collections;

public class StartGameButtonScript : ButtonScript {
	public override void PressButton() { if(menu != null) menu.StartGame (); }
}
