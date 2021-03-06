﻿using UnityEngine;
using System.Collections;

public class Help2Page : UIMenuPage 
{
    public Racquet ai_racquet;
    public Transform court_ui;
    public Help1Page back_page;
    private MatchManager match;


    public void Awake()
    {
        match = Object.FindObjectOfType<MatchManager>();
        if (match == null) Debug.LogError("MatchManager missing.");
    }
    public void ButtonBack()
    {
        ai_racquet.gameObject.SetActive(false);
        court_ui.gameObject.SetActive(false);

        TransitionOut();
        back_page.TransitionIn();
    }

    protected override void OnStartTransitionIn()
    {
        ai_racquet.gameObject.SetActive(true);
        court_ui.gameObject.SetActive(true);

        // customize the match
        match.SetMatchClockEnabled(false);
        match.SetRewallingEnabled(false);
        match.SetInterPointWaitEnabled(true);
        match.SetScoringEnabled(true);
        match.BeginMatch();
 	    base.OnStartTransitionIn();
    }
}
