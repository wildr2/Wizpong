using UnityEngine;
using System.Collections;

public class Help4Page : UIMenuPage
{
    private MatchManager match;
    public FadeScreenPage fade_page;
    public MatchBallSpawner ball_spawner;
    public Help3Page back_page;


    public void Awake()
    {
        match = Object.FindObjectOfType<MatchManager>();
        if (match == null) Debug.LogError("MatchManager missing.");
    }
    public void ButtonBeginMatch()
    {
        // match settings
        GameSettings.Instance.match_type = 0; // normal length match
        GameSettings.Instance.ai_controlled[1] = true; // against ai
        GameSettings.Instance.player_color_ID[1] = 0; // random color


        fade_page.TransitionIn();
        fade_page.on_transitioned_in = () => Application.LoadLevel("Game");
    }
    public void ButtonBack()
    {
        // disable all special balls
        ball_spawner.DespawnAll();

        TransitionOut();
        back_page.TransitionIn();
    }

    protected override void OnStartTransitionIn()
    {
        // customize the match
        ball_spawner.SpawnAll();
        match.BeginMatch();

        base.OnStartTransitionIn();
    }
    
}
