using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyTextManager : MonoBehaviour {

   public string enemyName;

   Text text;
   int killCount = 0;

   void Awake () {
      text = GetComponent<Text>();
      text.text = "";
   }

   void Start () {
      killCount = StatisticsManager.GetFinalKillCount(enemyName);
   }

   public void ShowKillCount() {
      text.text = "x" + killCount;
   }
}
