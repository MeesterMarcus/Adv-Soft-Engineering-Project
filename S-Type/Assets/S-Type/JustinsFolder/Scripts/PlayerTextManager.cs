using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTextManager : MonoBehaviour { Text initialsText;
   Text scoreText;
   Text killsText;

   void Start() {
      initialsText = transform.GetChild(0).GetComponent<Text>();
      scoreText = transform.GetChild(1).GetComponent<Text>();
      killsText = transform.GetChild(2).GetComponent<Text>();
   }

   public void setTexts(string initials, string finalScore, string totalKills) {
      initialsText.text = "" + initials;
      scoreText.text = "" + finalScore;
      killsText.text = "" + totalKills;
   }
}