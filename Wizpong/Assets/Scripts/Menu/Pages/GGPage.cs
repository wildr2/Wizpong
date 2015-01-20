using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GGPage : InGameMenuPage
{
    private MatchManager match;
    public Text heading;


    new public void Start()
    {
        match = Object.FindObjectOfType<MatchManager>();
        if (match == null) Debug.LogError("MatchManager missing.");

        base.Start();
    }
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
