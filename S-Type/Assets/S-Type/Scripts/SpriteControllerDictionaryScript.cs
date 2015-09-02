using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteControllerDictionaryScript : MonoBehaviour {
	[System.Serializable] public class SpriteController {
		public Sprite defaultSprite;
		public RuntimeAnimatorController animatorController;
	}

	[SerializeField] private string[] keys;
	[SerializeField] private SpriteController[] values;

	private Dictionary<string, SpriteController> spriteControllers;

	void Awake() {
		spriteControllers = new Dictionary<string, SpriteController> ();
		for (int i = 0; i < (keys.Length < values.Length ? keys.Length : values.Length); ++i)
			spriteControllers.Add (keys [i], values [i]);
	}
	public Sprite getSprite(string s) { return spriteControllers [s].defaultSprite; }
	public RuntimeAnimatorController getController(string s) { return spriteControllers [s].animatorController; }
}