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

public class DBTemplate : MonoBehaviour {

   public static string session_id; 
   public static string session_salt;
   private static string password;
   private static string login_response; 


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

         } 

      } else {
         Debug.Log("WWW Error: "+ www.error);
      }    
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
