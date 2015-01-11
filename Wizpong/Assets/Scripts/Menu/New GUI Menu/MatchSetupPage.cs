using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchSetupPage : UIMenuPage
{
    public Text bttn_music_text;
    public Text bttn_match_type_text;


    public void ButtonMusic()
    {
        GameSettings.music_on = !GameSettings.music_on;
        bttn_music_text.text = (GameSettings.music_on ? "Music On" : "Music Off");
    }
    public void ButtonMatchType()
    {
        GameSettings.match_type = (GameSettings.match_type + 1) % 2;
        bttn_match_type_text.text =
            GameSettings.match_type == 0 ? "5 Minute Match" :
            "10 Minute Match";
    }
}
