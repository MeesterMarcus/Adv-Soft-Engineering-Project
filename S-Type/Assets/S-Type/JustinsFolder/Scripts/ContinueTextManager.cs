using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ContinueTextManager : MonoBehaviour {
   public float timeToWait = 0.5f;
   public bool arrowPattern = true;
   
   Text text;
   string arrows;
   string[] ellipses = {".", "..", "..."};

   void OnEnable() {
      text = GetComponent<Text>();
      // text.text = "Press any key to continue";
      StartCoroutine(DrawPattern());
   }

   IEnumerator DrawPattern() {
      // We are drawing an arrow pattern under the text.
      while(arrowPattern) {
         arrows = "<" + arrows + ">";
         text.text = "Press any key to continue\n" + arrows;
         if((arrows.Length % ("Press any key to continue\n".Length)/2) == 0) {
            arrows = "";
         }
         yield return new WaitForSeconds(timeToWait);
      }
      // We are drawing an ellipses pattern to the right of the text.
      for(int i = 0; ; i++) {
         text.text = "Press any key to continue" + ellipses[i%ellipses.Length];
         yield return new WaitForSeconds(timeToWait);
      }
   }
}