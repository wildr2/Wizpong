using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour 
{
    public Text player1_heading, player2_heading; 
    public ComparisonBar points_bar, possession_bar, stun_bar, rewalls_bar;
    private MatchManager match;


    public void Start()
    {
        match = Object.FindObjectOfType<MatchManager>();
        if (match == null) Debug.LogError("MatchManager missing.");
    }
    public void OnEnable()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        // player names
        player1_heading.text = GameSettings.Instance.player_name[0];
        player2_heading.text = GameSettings.Instance.player_name[1];


        // points bar
        float pct = 0;
        float total_points = match.GetPointsP1() + match.GetPointsP2();
        if (total_points == 0) pct = 0.5f;
        else pct = match.GetPointsP1() / total_points;

        points_bar.Set(match.GetPointsP1().ToString(), match.GetPointsP2().ToString(), pct);
        points_bar.SetColors(GameSettings.Instance.GetPlayerColor(1), GameSettings.Instance.GetPlayerColor(2));


        // possession bar
        pct = match.GetPosessionPercentageP1();
        string pct_left = ((int)(pct*100f)).ToString() + "%";
        string pct_right = ((int)((1-pct)*100f)).ToString() + "%";

        possession_bar.Set(pct_left, pct_right, pct);
        possession_bar.SetColors(GameSettings.Instance.GetPlayerColor(1), GameSettings.Instance.GetPlayerColor(2));


        // rewalls bar
        float total_rewalls = match.GetRewallsP1() + match.GetRewallsP2();
        if (total_rewalls == 0) pct = 0.5f;
        else pct = match.GetRewallsP1() / total_rewalls;

        rewalls_bar.Set(match.GetRewallsP1().ToString(), match.GetRewallsP2().ToString(), pct);
        rewalls_bar.SetColors(GameSettings.Instance.GetPlayerColor(1), GameSettings.Instance.GetPlayerColor(2));


        // stun bar
        float total_stun_time = match.GetTimeStunnedP1() + match.GetTimeStunnedP2();
        if (total_stun_time == 0) pct = 0.5f;
        else pct = match.GetTimeStunnedP1() / total_stun_time;

        stun_bar.Set(((int)match.GetTimeStunnedP1()).ToString() + "s", ((int)match.GetTimeStunnedP2()).ToString() + "s", pct);
        stun_bar.SetColors(GameSettings.Instance.GetPlayerColor(1), GameSettings.Instance.GetPlayerColor(2));
    }
    
}
