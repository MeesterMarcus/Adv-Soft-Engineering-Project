using UnityEngine;
using System.Collections;

public class LivesButtonScript : ButtonScript {

	public void Update() { if(menu != null && mesh != null) mesh.text = "@ x " + (menu.lives + 1) + "/" + menu.LivesMax; }
	public override void PostStart() { }
	public override void PressButton() {
		if (menu != null && Input.anyKeyDown == true) {
			Vector2 InputAxis = new Vector2(Input.GetAxisRaw ("Horizontal 2"), Input.GetAxisRaw ("Vertical 2")); // Declare our input axis
			
			if(InputAxis.y < 0)
				menu.lives = 0;
			else if(InputAxis.y > 0)
				menu.lives = menu.LivesMax - 1;
			else if(InputAxis.x < 0) {
				menu.lives -= 1;
				if(menu.lives < 0)
					menu.lives = menu.LivesMax - 1;
			} else {
				menu.lives += 1;
				if(menu.lives >= menu.LivesMax)
					menu.lives = 0;
			}
		}
	}
}
