using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchSetupPage : UIMenuPage
{
    public Text bttn_music_text;
    public Text bttn_match_type_text;


    public void Start()
    {
        ResetButtonMusic(GameSettings.music_on);
        ResetButtonMatchType(GameSettings.match_type);
    }

    public void ButtonMusic()
    {
        GameSettings.music_on = !GameSettings.music_on;
        ResetButtonMusic(GameSettings.music_on);
    }
    public void ButtonMatchType()
    {
        GameSettings.match_type = (GameSettings.match_type + 1) % 2;
        ResetButtonMatchType(GameSettings.match_type);
    }


    private void ResetButtonMusic(bool music_on)
    {
        bttn_music_text.text = (music_on ? "Music On" : "Music Off");
    }
    private void ResetButtonMatchType(int match_type)
    {
        bttn_match_type_text.text =
            match_type == 0 ? "5 Minute Match" :
            "10 Minute Match";
    }
}
