using UnityEngine;
using System.Collections;

public class InvincibilityButtonScript : ButtonScript {
	public void Update() { if(mesh != null && menu != null) mesh.text = "INVINCIBILITY: " + menu.invincibility; }
	public override void PressButton() { if (Input.anyKeyDown == true && menu != null) menu.invincibility = !menu.invincibility;  }
}
