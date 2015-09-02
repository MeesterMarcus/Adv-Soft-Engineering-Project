using UnityEngine;
using System.Collections;

public class ContinueGameButtonScript : ButtonScript {
	private void Update() { 
		if (menu.Continues <= 9)
			mesh.text = "CONTINUE GAME 000" + menu.Continues;
		else if (menu.Continues <= 99)
			mesh.text = "CONTINUE GAME 00" + menu.Continues;
		else if (menu.Continues <= 999)
			mesh.text = "CONTINUE GAME 0" + menu.Continues;
		else if (menu.Continues <= 9999)
			mesh.text = "CONTINUE GAME " + menu.Continues;
		else
			mesh.text = "CONTINUE GAME FREE";
	}
	public override void PressButton() { menu.ContinueGame (); }
}
