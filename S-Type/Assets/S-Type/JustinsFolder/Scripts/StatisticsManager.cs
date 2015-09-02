using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class StatisticsManager : MonoBehaviour {
   private static StatisticsManager instance;					// A reference to this StatisticsManager.
   private static int highScore;                    			// Player's score.
   private static string playerInitials;                    // Player's initials.
   private static Dictionary<string, int> enemiesDowned 
      = new Dictionary<string, int>();                      // Dictionary containing number of enemies killed by player.
   private static int totalDowns;                           // Total number of enemies killed.

   void Awake () {
      // Keep StatisticsCollector from being destroyed in between levels if this is the first occurrence.
      if(instance == null) {
         instance = this;
         DontDestroyOnLoad(this);
      }
      else {
         if(this != instance) {
            Destroy(this.gameObject);
         }
      }
   }
   
   public static void ResetStats() {
      Debug.Log("Reseting stats.");
      List<string> keys = new List<string>(enemiesDowned.Keys);
      foreach(string key in keys) {
         enemiesDowned[key] = 0;
      }
      highScore = 0;
      playerInitials = "";
      totalDowns = 0;
      Debug.Log("Stats reset.");
   }

   public static void IncrementKills(string enemyName) {
      // If the dictionary does not have an entry for this enemy, we have killed it for the first time.
      if(!enemiesDowned.ContainsKey(enemyName)) {
         enemiesDowned.Add(enemyName, 1);
      }
      else {
         enemiesDowned[enemyName]++;
      }
      totalDowns++;
      // Debugging purposes.
      //PrintKills();
   }

   public static void PrintKills() {
      foreach(string key in enemiesDowned.Keys) {
         print("Downs for enemy " + key + ": " + enemiesDowned[key] + "\n");
      }
   }

   public static int GetFinalKillCount(string enemyName) {
      if(enemiesDowned.ContainsKey(enemyName)) {
         return enemiesDowned[enemyName];
      }
      else {
         return 0;
      }
   }

   public static int GetFinalKillCountSum() {
      return totalDowns;
   }

   public static void SetFinalScore(int score) {
      highScore = score;
   }

   public static int GetFinalScore() {
      return highScore;
   }

   public static void SetInitials(string initials) {
      playerInitials = initials;
   }

   public static string GetInitials() {
      return playerInitials;
   }
}