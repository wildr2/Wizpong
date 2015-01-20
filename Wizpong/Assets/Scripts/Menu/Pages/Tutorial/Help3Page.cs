using UnityEngine;
using System.Collections;

public class Help3Page : UIMenuPage 
{
    public Racquet ai_racquet;
    private MatchManager match;


    public void Awake()
    {
        match = Object.FindObjectOfType<MatchManager>();
        if (match == null) Debug.LogError("MatchManager missing.");
    }

    protected override void OnStartTransitionIn()
    {
        // customize the match
        match.SetMatchClockEnabled(false);
        match.SetRewallingEnabled(true);
        match.SetInterPointWaitEnabled(true);
        match.SetScoringEnabled(true);
        match.BeginMatch();

        base.OnStartTransitionIn();
    }
}
