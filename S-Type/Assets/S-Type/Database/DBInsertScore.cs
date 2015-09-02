using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Security.Cryptography; 

/**
 * This is how to connect to Database. Used as a
 * template
 **/

public class DBInsertScore : MonoBehaviour {
   
   public static string session_id; 
   public static string session_salt;
   private static string password;
   private static string login_response; 

   private static int score; 
   private static string name; 
   private static int shipsDestroyed;
   
   
   void Start () {
      
      password = getHashSha256("TdhXsT52vGXxdabX"); 
      
      //Create JSON encoding
      string json = "[{\"action\":\"login\"}," +
         "{\"login\":\"ws_team02\"}," +
            "{\"password\":\""+password+"\"}," +
            "{\"app_code\":\"wFtYeD7UJm\"}]";
      
      //Calculate the checksum
      string checksum = getHashSha256 (json); 
      
      //Format JSON encoding with checksum and remove duplicate brackets
      string jsonForURL = "{\"action\":\"login\"}," +
         "{\"login\":\"ws_team02\"}," +
            "{\"password\":\""+password+"\"}," +
            "{\"app_code\":\"wFtYeD7UJm\"},"+
            "{\"checksum\":\""+checksum+"\"}";
      
      
      string url = "https://devcloud.fulgentcorp.com/bifrost/ws.php?json=[" + jsonForURL + "]";
      StartCoroutine(WaitForRequest(url));

   }
   
   IEnumerator WaitForRequest(string url)
   {
      
      WWW www = new WWW (url);
      yield return www;
      
      // check for errors
      if (www.error == null)
      {
         string response = www.text;
         
         if (response.Contains ("session_salt")){
            Debug.Log("WWW Ok!: " + response);
            
            string[] sarr = response.Split (','); 
            session_id = sarr[3];
            session_salt = sarr[4];
            
            string[] sidarr = session_id.Split (':');
            session_id = sidarr [1].TrimEnd ('}'); 
            
            string[] saltarr = session_salt.Split (':'); 
            session_salt = saltarr [1].TrimEnd ('}'); 
            session_salt = session_salt.TrimStart ('\"');
            session_salt = session_salt.TrimEnd ('\"');
            
            www.Dispose (); 
            insertScore(); 
            
         } else {
            Debug.Log ("URL: "+www.url); 
            Debug.Log ("SQL Response"+response);
            DatabaseManager.InsertionFinished();
         }
         
      } else {
         Debug.Log("WWW Error: "+ www.error);
         DatabaseManager.InsertionFinished();
      }    
   }
   
   /**
       * This function uses the run_sql action on the web server
       * to execute a MySQL query. 
       **/
   public void insertScore() {
      
      string checksum = ""; 
      
      long sid = Convert.ToInt64 (session_id); 
      name = StatisticsManager.GetInitials();
      score = StatisticsManager.GetFinalScore();
      shipsDestroyed = StatisticsManager.GetFinalKillCountSum();
      Debug.Log ("INITIALS IN DBINSERTSCORE: " + name);
      Debug.Log ("SCORE IN DBINSERTSCORE: " + score);
      Debug.Log ("SOMETHING IN DBINSERTSCORE: " + shipsDestroyed); 

      string query = "insert into scores (name, score, shipsdestroyed) values (:un, :hs, :sd)";
      
      string json = "[{\"action\":\"run_sql\"}," +
         "{\"session_id\":" + sid + "}," +
            "{\"query\":\"" + query + "\"}," +
            "{\"args\":[{\":un\":\""+name+"\"},{\":hs\":"+score+"},{\":sd\":"+shipsDestroyed+"}]}" +
            "]";
      
      checksum = getHashSha256 (json + session_salt); 
      Debug.Log ("JSON+SESSION_SALT:"+json+session_salt); 
      Debug.Log ("Checksum SQL: " + checksum); 
      
      query = query.Replace (" ", "%20");
      
      string url = "https://devcloud.fulgentcorp.com/bifrost/ws.php?json=[" +
         "{\"action\":\"run_sql\"}," +
            "{\"session_id\":"+sid+"}," +
            "{\"query\":\""+query+"\"}," +
            "{\"args\":[{\":un\":\""+name+"\"},{\":hs\":"+score+"},{\":sd\":"+shipsDestroyed+"}]}," +
            "{\"checksum\":\""+checksum+"\"}]";
      
      
      StartCoroutine(WaitForRequest(url));
      
   }
   
   /**
       * This function calculates the Sha 256 of the text
       * passed to it. 
       **/
   public static string getHashSha256(string text)
   {
      byte[] bytes = Encoding.UTF8.GetBytes(text);
      SHA256Managed hashstring = new SHA256Managed();
      byte[] hash = hashstring.ComputeHash(bytes);
      string hashString = string.Empty;
      foreach (byte x in hash)
      {
         hashString += String.Format("{0:x2}", x);
      }
      return hashString;
   }
}