using UnityEngine;
using System.Collections;

public class MusicStartScript : MonoBehaviour {

   public AudioClip clip;
   public bool musicLoop;
   public float musicDelay;

	public bool camScrollVertical = false;
	public float camScrollSpeed = .1f;

	// Use this for initialization
	public IEnumerator Start () {
		MainScript mainScript =  Camera.main.GetComponent<MainScript>();

		GameObject playerGO = GameObject.FindWithTag ("Player");
		if (playerGO != null) {
			PlayerScript player = playerGO.GetComponent<PlayerScript>();
			if(player != null) {
				player.camScrollVertical = camScrollVertical;
				player.camScrollSpeed = camScrollSpeed;
			}
		}

		yield return new WaitForSeconds(musicDelay);
		StartCoroutine(mainScript.MusicPlay(clip, musicLoop, false, 0f));
	}
}
