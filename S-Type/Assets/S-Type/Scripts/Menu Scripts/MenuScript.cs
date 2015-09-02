using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {
	[SerializeField] private Color normalColor;
	[SerializeField] private Color overColor;

	[SerializeField] public GameObject gameStatusGO;
	[SerializeField] private int weaponTypeMax;
	[SerializeField] private int speedLevelMax;
	[SerializeField] private int continues;
	[SerializeField] private int levelMax;
	[SerializeField] private int livesMax;
	
	private ButtonScript[] buttons;
	private GameStatus gameStatus;
	private Vector2 prevInputAxis;
	private int selected;
	
	public int weaponType;
	public int speedLevel;
	public int level;
	public int lives;
   public bool invincibility;
   public string levelBaseName;

	[SerializeField] private float buttonWidth;
	[SerializeField] private float buttonHeight;
	[SerializeField] private int buttonColumns;

	public int WeaponTypeMax { get { return weaponTypeMax; } }
	public int SpeedLevelMax { get { return speedLevelMax; } }
	public int LivesMax { get { return livesMax; } }
	public int LevelMax { get { return levelMax; } }
	public int Continues { get { return gameStatus.continues; } }

	private void Start() {
		float defaultButtonXPos = 0.0f;
		float buttonXPos = 0.0f;
		float buttonYPos = 0.0f;
		int buttonCount = 0;
		selected = 0;

		if (buttonColumns <= 0) {
			Debug.LogError ("Can't have 0 or less columns");
			buttonColumns = 1;
		}

		defaultButtonXPos = -buttonWidth * (buttonColumns / 2);
		if (buttonColumns % 2 == 0)
		    defaultButtonXPos -= buttonWidth / 2;
		buttonXPos = defaultButtonXPos;

		foreach (Transform child in transform) {
			ButtonScript button = child.GetComponent<ButtonScript>();
			if(button != null)
				++buttonCount;
		}
		if (buttonCount == 0) Debug.LogError ("No buttons found");

		buttons = new ButtonScript[buttonCount];
		buttonCount = 0;
		foreach (Transform child in transform) {
			//child.position = new Vector2(buttonXPos, buttonYPos);

			ButtonScript button = child.GetComponent<ButtonScript>();
			if(button != null) {
				child.position = new Vector2(buttonXPos, buttonYPos);

				buttons[buttonCount] = button;
				buttons[buttonCount].Register(this, buttonCount);
				buttons[buttonCount].SetColor(normalColor);

				++buttonCount;
				if(buttonCount % buttonColumns == 0) {
					buttonYPos -= buttonHeight;
					buttonXPos = defaultButtonXPos;
				} else
					buttonXPos += buttonWidth;
			}
		}

		if (gameStatusGO == null) Debug.LogError ("Prefab for the Game Status doesn't exist");
		GameObject temp = GameObject.Find (gameStatusGO.name);
		if (temp == null) {
			temp = GameObject.Instantiate (gameStatusGO);
			temp.name = gameStatusGO.name;
			gameStatus = temp.GetComponent<GameStatus> ();
			if (gameStatus == null) Debug.LogError ("Can't find game status script in game status object");
		} else {
			gameStatus = temp.GetComponent<GameStatus> ();
			if (gameStatus == null) Debug.LogError ("Can't find game status script in game status object");

			invincibility = gameStatus.invincibility;
			weaponType = gameStatus.weaponType;
			speedLevel = gameStatus.speedLevel;
			lives = gameStatus.baseLives;
			level = gameStatus.level;
		}
	}
	private void Update() {
		Vector2 InputAxis = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")); // Declare our input axis
		buttons [selected].SetColor (normalColor);

		if (InputAxis.y < 0 && prevInputAxis.y == 0) {
			selected += buttonColumns;
			if (selected >= buttons.Length)
				selected = selected % buttonColumns;
		} else if(InputAxis.y > 0 && prevInputAxis.y == 0) {
			selected -= buttonColumns;
			if(selected < 0) selected += buttons.Length;
		}

		if (InputAxis.x > 0 && prevInputAxis.x == 0) {
			int baseSelected = (selected / buttonColumns) * buttonColumns;
			int selectedMod = (selected + 1) % buttonColumns;
			selected = baseSelected + selectedMod;
		} else if (InputAxis.x < 0 && prevInputAxis.x == 0) {
			int baseSelected = (selected / buttonColumns) * buttonColumns;
			int selectedMod = (selected - 1) % buttonColumns;
			if(selectedMod < 0) selectedMod += buttonColumns;
			selected = baseSelected + selectedMod;
		}

		if(Input.anyKeyDown == true && InputAxis.x == 0f && InputAxis.y == 0f) {
			buttons[selected].PressButton();
		}

		buttons [selected].SetColor (overColor);
		prevInputAxis = InputAxis;
	}

	public void mouseSelect(int id) { 
		buttons [selected].SetColor (normalColor);
		buttons [id].SetColor (overColor);
		selected = id;
	}

	public void StartGame() {
		gameStatus.invincibility = invincibility;
		gameStatus.weaponType = weaponType;
		gameStatus.speedLevel = speedLevel;

		gameStatus.continues = continues;
		gameStatus.level = level;

		gameStatus.lives = gameStatus.baseLives = lives;
		gameStatus.score = 0;
		Application.LoadLevel (levelBaseName); 
	}
	public void ContinueGame() {
		gameStatus.invincibility = invincibility;
		gameStatus.weaponType = weaponType;
		gameStatus.speedLevel = speedLevel;

		if (gameStatus.continues > 9999) {
			gameStatus.level = level;
		} else if (gameStatus.continues > 0) {
			gameStatus.continues -= 1;
			gameStatus.level = level;
		} else {
			gameStatus.continues = continues;
			gameStatus.level = level;
		}

		gameStatus.lives = gameStatus.baseLives = lives;
		gameStatus.score = 0;
		Application.LoadLevel (levelBaseName);
	}

	public void ShowLeaderboard() { Application.LoadLevel (Application.loadedLevel + 1); }
	public void ExitGame() { Application.Quit (); }
}
