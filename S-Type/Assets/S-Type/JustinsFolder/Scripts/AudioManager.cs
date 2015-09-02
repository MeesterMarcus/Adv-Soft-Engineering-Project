using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

   public float timeToFade = .5f;

   AudioSource audioSource;
   bool fadeOut = false;

   void Awake () {
      audioSource = GetComponent<AudioSource>();
   }
	
   // Update is called once per frame
   void Update () {
      if(fadeOut && audioSource.volume > 0) {
         audioSource.volume -= (Time.deltaTime * timeToFade);
         //Debug.Log (audioSource.volume);
      }
   }

   public void FadeMusic() {
      if(InitialsTextManager.finalInputValid) {
         fadeOut = true;
      }
   }
}