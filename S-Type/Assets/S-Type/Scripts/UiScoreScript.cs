using UnityEngine;
using System;
using System.Collections;

public class UiScoreScript:MonoBehaviour {
	public Color color1 = Color.cyan;
	[HideInInspector] public int scoreGet;  // Real score sent by "PlayerScript"
	int scoreDisplay; // Displayed text value - 8 digits (max 99 999 999)
	int maxScore;
	TextMesh[] textMeshes; // 3D Text (TextMesh)
	bool[] scoreTileTime;
	public int maxDigits;
	public GameObject meshObject;
	
	[HideInInspector] public float pixelLeftSize;
	[HideInInspector] public float screenLeftSize;
	[HideInInspector] public float pixelRightSize;
	[HideInInspector] public float screenRightSize;
	[HideInInspector] public float pixelLeftBound;
	[HideInInspector] public float screenLeftBound;
	[HideInInspector] public float pixelRightBound;
	[HideInInspector] public float screenRightBound;
	public int maxBossNameLength = 9;
	public float charScreenSize = .06f;

	public void Start() {
		textMeshes = new TextMesh[maxDigits];
		scoreTileTime = new bool[maxDigits];
		scoreDisplay = 0;
		maxScore = 1;

		screenLeftSize = charScreenSize * maxDigits;
		screenRightSize = charScreenSize * maxBossNameLength;
		pixelLeftSize = screenLeftSize * 100f;
		pixelRightSize = screenRightSize * 100f;
		screenLeftBound = Camera.main.aspect - screenLeftSize;
		screenRightBound = Camera.main.aspect - screenRightSize;
		pixelLeftBound = screenLeftBound * 100f;
		pixelRightBound = screenRightBound * 100f;

		for (int i = 0; i < maxDigits; ++i) {
			GameObject temp = (GameObject)GameObject.Instantiate(meshObject);
			temp.transform.parent = transform;
			temp.transform.localPosition = new Vector3(i * 0.06f, 0, 0);
			textMeshes[i] = temp.GetComponent<TextMesh>();
			textMeshes[i].text = "";
			textMeshes[i].color = color1;
			maxScore *= 10;
		}
		maxScore -= 1;
	}

	public void Update() {
		int scoreGetTemp = scoreGet;
		int scoreDisplayTemp = scoreDisplay;
		scoreDisplay = scoreGet;
		for (int j = maxDigits - 1; j >= 0; scoreGetTemp /= 10, scoreDisplayTemp /= 10, --j) {
			int scoreGetDigit = scoreGetTemp % 10;
			int scoreDisplayDigit = scoreDisplayTemp % 10;

			if(scoreTileTime[j] == false) {
				if(scoreDisplayDigit != scoreGetDigit) {
					scoreTileTime[j] = true;
					StartCoroutine("RandomizeDigit", j);
				}
			}
			if(scoreTileTime[j] == true) {
				textMeshes[j].text = "" + UnityEngine.Random.Range(0, 10);
				textMeshes[j].color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
			}
			else {
				textMeshes[j].text = "" + scoreGetDigit;
				textMeshes[j].color = color1;
			}
		}
	}
	public IEnumerator RandomizeDigit(int digit) { yield return new WaitForSeconds (1.0f); scoreTileTime [digit] = false; }

	public void ProcessScoreEntry(int playerScore) { scoreGet = playerScore < maxScore ? playerScore : maxScore; }
	public void UpdateScoreEntryDirect() { // Displays directly score increment without visual effect
		int scoreGetTemp = scoreDisplay = scoreGet;
		for (int j = maxDigits - 1; j >= 0; scoreGetTemp /= 10, --j) {
			textMeshes[j].text = "" + scoreGet % 10;
			textMeshes[j].color = color1;
			scoreTileTime[j] = true;
			StartCoroutine("RandomizeDigit", j);
		}

		screenLeftSize = charScreenSize * maxDigits;
		screenRightSize = charScreenSize * maxBossNameLength;
		pixelLeftSize = screenLeftSize * 100f;
		pixelRightSize = screenRightSize * 100f;
		screenLeftBound = Camera.main.aspect - screenLeftSize;
		screenRightBound = Camera.main.aspect - screenRightSize;
		pixelLeftBound = screenLeftBound * 100f;
		pixelRightBound = screenRightBound * 100f;
	}
}