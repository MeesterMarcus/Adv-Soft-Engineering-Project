using UnityEngine;
using System.Collections;

public class GameStatus : MonoBehaviour {
	[HideInInspector] public bool invincibility;
	[HideInInspector] public float checkpointPos;
	[HideInInspector] public int weaponType;
	[HideInInspector] public int speedLevel;
	[HideInInspector] public int menuScene;
	[HideInInspector] public int continues;
	[HideInInspector] public int baseLives;
	[HideInInspector] public int level;
	[HideInInspector] public int lives;
	[HideInInspector] public int score;

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
		checkpointPos = 0.0f;
		menuScene = Application.loadedLevel;
		continues = 0;
		level = 0;
	}
}