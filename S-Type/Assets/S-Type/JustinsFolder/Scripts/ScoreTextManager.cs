using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreTextManager : MonoBehaviour
{
   public float animationLength = 5f;
   public AudioClip scoreRisingSound;
   public AudioClip scoreFinishedSound;
   
   AudioSource audioSource;
   float minScore;
   float maxScore;
   float startTime;
   float currentTime;
   Text text;
	
   void OnEnable() {
      text = GetComponent<Text>();
      minScore = 0f;
      maxScore = (int)StatisticsManager.GetFinalScore();
      startTime = Time.time;
      audioSource = GetComponent<AudioSource>();
      audioSource.clip = scoreRisingSound;
      audioSource.Play();
   }

   void Update () {
      if(text.text.Equals(((int)maxScore).ToString())) {
         audioSource.Stop();
         audioSource.PlayOneShot(scoreFinishedSound);
         this.enabled = false;
      }
      else {
         currentTime = (Time.time - startTime) / animationLength;
         text.text = "" + (int)(Mathf.SmoothStep(minScore, maxScore, currentTime));
      }
   }

   public void EndScoreAnimation() {
      if(InitialsTextManager.finalInputValid) {
         audioSource.Stop();
         text.text = ((int)maxScore).ToString();
         this.enabled = false;
      }
   }
}