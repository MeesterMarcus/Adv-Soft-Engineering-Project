using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	
	protected MenuScript menu;
	protected TextMesh mesh;
	private int id;

	private void Start() { mesh = GetComponent<TextMesh> (); PostStart (); }
	private void OnMouseEnter() { if(menu != null) menu.mouseSelect (id); }
	private void OnMouseDown() { PressButton (); }

	public void Register(MenuScript menu, int id) { this.menu = menu; this.id = id; }
	public void SetColor(Color color) { if(mesh != null) mesh.color = color; }

	public virtual void PressButton() { }
	public virtual void PostStart() { }
}
