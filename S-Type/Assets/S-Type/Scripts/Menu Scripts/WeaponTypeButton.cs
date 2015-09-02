using UnityEngine;
using System.Collections;

public class WeaponTypeButton : ButtonScript {
	
	public void Update() { if(mesh != null && menu != null) mesh.text = "WEAPON TYPE: " + (menu.weaponType + 1) + "/" + menu.WeaponTypeMax; }
	public override void PostStart() { }
	public override void PressButton() {
		if (Input.anyKeyDown == true && menu != null) {
			Vector2 InputAxis = new Vector2(Input.GetAxisRaw ("Horizontal 2"), Input.GetAxisRaw ("Vertical 2")); // Declare our input axis

			if(InputAxis.y < 0)
				menu.weaponType = 0;
			else if(InputAxis.y > 0)
				menu.weaponType = menu.WeaponTypeMax - 1;
			else if(InputAxis.x < 0) {
				menu.weaponType -= 1;
				if(menu.weaponType < 0)
					menu.weaponType = menu.WeaponTypeMax - 1;
			} else {
				menu.weaponType += 1;
				if(menu.weaponType >= menu.WeaponTypeMax)
					menu.weaponType = 0;
			}
		}
	}
}
