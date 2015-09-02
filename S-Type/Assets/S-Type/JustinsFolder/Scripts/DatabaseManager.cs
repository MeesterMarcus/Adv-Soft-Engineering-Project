using UnityEngine;
using System.Collections;

public class DatabaseManager : MonoBehaviour {
   static bool insertionFinished;
   DBInsertScore insertScript;

   void Start() {
      insertScript = GetComponent<DBInsertScore>();
      insertionFinished = false;
      StartCoroutine(LoadMenu());
   }

   // Called from player entering initials.
   public void DBUpdateStats() {
      if(InitialsTextManager.finalInputValid) {
		   insertScript.enabled = true; // Begin DB insertion.
      }
   }

   // Called from DBInsertScore.cs once query is finished.
   public static void InsertionFinished() {
      insertionFinished = true;
   }

   IEnumerator LoadMenu() {
      while(!insertionFinished) {
         yield return new WaitForSeconds(.5f);
      }
      StatisticsManager.ResetStats();
      yield return new WaitForSeconds(4f);
      Application.LoadLevel("MainMenu");
   }
}