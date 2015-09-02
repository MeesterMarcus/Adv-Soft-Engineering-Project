using UnityEngine;
using System.Collections;

public class LeaderboardTransition : ButtonScript {
   public override void PressButton() { if(menu != null) Application.LoadLevel ("Leaderboard"); }
}
