using UnityEngine;
using System.Collections;

public class LevelButtonScript : ButtonScript {

	public void Update() { 
		if(menu.level <= 0) menu.level = 1; 
		if(mesh != null && menu != null) mesh.text = "LEVEL: " + menu.level + "/" + menu.LevelMax; 
	}
	public override void PostStart() { }
	public override void PressButton() {
		if (Input.anyKeyDown == true && menu != null) {
			Vector2 InputAxis = new Vector2(Input.GetAxisRaw ("Horizontal 2"), Input.GetAxisRaw ("Vertical 2"));; // Declare our input axis
			
			if(InputAxis.y < 0)
				menu.level = 1;
			else if(InputAxis.y > 0)
				menu.level = menu.LevelMax;
			else if(InputAxis.x < 0f) {
				menu.level -= 1;
				if(menu.level < 0f)
					menu.level = menu.LevelMax;
			} else {
				menu.level += 1;
				if(menu.level > menu.LevelMax)
					menu.level = 1;
			}
		}
	}
}
