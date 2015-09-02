using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class LeaderboardUIManager : MonoBehaviour {
   public GameObject[] players;

   static Animator anim;
   static bool fetchFinished;
   /*static string text = @"SQL Response[{""result"":""success""},{""message"":[" +
                     @"[{""id"":""36""},{""name"":""MEL""},{""score"":""1600""},{""shipsdestroyed"":""2""}]," +
                     @"[{""id"":""35""},{""name"":""CCC""},{""score"":""1100""},{""shipsdestroyed"":""2""}]," +
                     @"[{""id"":""8""},{""name"":""ABC""},{""score"":""300""},{""shipsdestroyed"":""3""}]" +
                     @"]},{""checksum"":""03f45d0620fea71f4b951f241cebf64948e50491642a2e0474d1cc5dc9a8e0b2""}]";*/
   static string text;
   static string patternStart = @"^.*""message"":\[";
   static string patternEnd = @"\]\},\{""checksum"".*$";
   static string patternID = @"{""id"":""\d+""\}";
   static string patternToken = @"\[?,?\{""\w+"":""(\w+)""\},?\]?";
   string [] splitTokens;
   
   bool transitionOut;
   string initials;
   int finalScore;
   int totalKills;

	void Start() {
      anim = GetComponent<Animator>();
      transitionOut = false;
      fetchFinished = false;
      StartCoroutine(ListPlayers());
      StartCoroutine(LoadMenu());
	}

   void Update() {
      if(Input.anyKey && !transitionOut) {
         anim.SetTrigger("transitionOut");
         transitionOut = true;
      }
   }

   // Called from DBFetchScores.cs once query is finished.
   public static void FetchFinished(string response) {
      fetchFinished = true;
      text = response;
   }

   IEnumerator ListPlayers() {
      while(!fetchFinished) {
         yield return new WaitForSeconds(.25f);
      }
      yield return null;
      text = Regex.Replace(text, patternStart, "");
      text = Regex.Replace(text, patternEnd, "");
      text = Regex.Replace(text, patternID, "");
      text = Regex.Replace(text, patternToken, "$1:");
      string [] splitTokens = text.Split(',');
      for(int i = 0; i < 10; i++) {
         PlayerTextManager pTextScript = players[i].GetComponent<PlayerTextManager>();
         splitTokens[i] = splitTokens[i].Substring(0, splitTokens[i].Length - 1);
         string [] tokens = splitTokens[i].Split(':');
         pTextScript.setTexts(tokens[0], tokens[1], tokens[2]);
      }
      anim.SetTrigger("listPlayers");
   }

   IEnumerator LoadMenu() {
      while(!transitionOut) {
         yield return new WaitForSeconds(.5f);
      }
      yield return new WaitForSeconds(4f);
      Application.LoadLevel("MainMenu");
   }
}