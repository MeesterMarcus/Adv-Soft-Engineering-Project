using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameOverUIManager : MonoBehaviour {

   public GameObject[] enemies;
   public GameObject continuePrompt;
   public float timeToWait = .3f;
   public AudioClip listSound;
   public AudioClip continueSound;
   [HideInInspector] public Animator anim;

   AudioSource audioSource;
   AnimatorStateInfo info;

   void Awake () {
      anim = GetComponent<Animator>();
      audioSource = GetComponent<AudioSource>();
   }

   void Start () {
      continuePrompt.SetActive(false);
      StartCoroutine(ListKills());
   }

   void Update () {
      if(continuePrompt.activeSelf && Input.anyKey) {
         audioSource.clip = continueSound;
         audioSource.Play();
         anim.SetTrigger("playerContinue");
         continuePrompt.SetActive(false);
      }
      /*if(anim.GetCurrentAnimatorStateInfo(0).IsName("BlindsScreenFadeOut")) {
         Debug.Log (anim.GetCurrentAnimatorStateInfo(0).length);
         Debug.Log("Lol.");
      }*/
   }

   IEnumerator ListKills() {
      yield return new WaitForSeconds(2.5f);
      foreach(GameObject enemy in enemies) {
         //Debug.Log(enemy.GetComponent<EnemyTextManager>().enemyName);
         enemy.GetComponent<EnemyTextManager>().ShowKillCount();
         audioSource.Play();
         yield return new WaitForSeconds(timeToWait);
      }
      yield return new WaitForSeconds(1);
      continuePrompt.SetActive(true);
   }
}