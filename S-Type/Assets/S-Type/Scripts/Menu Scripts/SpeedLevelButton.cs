using UnityEngine;
using System.Collections;

public class SpeedLevelButton : ButtonScript {	
	public void Update() { if(mesh != null && menu != null) mesh.text = "SPEED LEVEL: " + (menu.speedLevel + 1) + "/" + menu.SpeedLevelMax; }
	public override void PostStart() { }
	public override void PressButton() {
		if (Input.anyKeyDown == true && menu != null) {
			Vector2 InputAxis = new Vector2(Input.GetAxisRaw ("Horizontal 2"), Input.GetAxisRaw ("Vertical 2")); // Declare our input axis
			
			if(InputAxis.y < 0)
				menu.speedLevel = 0;
			else if(InputAxis.y > 0)
				menu.speedLevel = menu.SpeedLevelMax - 1;
			else if(InputAxis.x < 0) {
				menu.speedLevel -= 1;
				if(menu.speedLevel < 0)
					menu.speedLevel = menu.SpeedLevelMax - 1;
			} else {
				menu.speedLevel += 1;
				if(menu.speedLevel >= menu.SpeedLevelMax)
					menu.speedLevel = 0;
			}
		}
	}
}
