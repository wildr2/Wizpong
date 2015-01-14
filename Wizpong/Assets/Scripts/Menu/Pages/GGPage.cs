﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GGPage : InGameMenuPage
{
    public MatchManager match;
    public Text heading;


    public void OnEnable()
    {
        UIAudio.Instance.PlayPause();

        int result = match.GetWinningPlayer();
        if (result == 1)
        {
            heading.text = GameSettings.Instance.player_name[0] + " Victory";
        }
        else if (result == 2)
        {
            heading.text = GameSettings.Instance.player_name[1] + " Victory";
        }
        else
            heading.text = "Draw";
    }
}
