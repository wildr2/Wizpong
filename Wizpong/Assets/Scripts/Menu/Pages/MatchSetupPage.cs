using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatchSetupPage : UIMenuPage
{
    public Text bttn_music_text;
    public Text bttn_match_type_text;


    public void Start()
    {
        ResetButtonMusic(GameSettings.Instance.music_on);
        ResetButtonMatchType(GameSettings.Instance.match_type);
    }

    public void ButtonMusic()
    {
        GameSettings.Instance.music_on = !GameSettings.Instance.music_on;
        ResetButtonMusic(GameSettings.Instance.music_on);
    }
    public void ButtonMatchType()
    {
        GameSettings.Instance.match_type = (GameSettings.Instance.match_type + 1) % 3;
        ResetButtonMatchType(GameSettings.Instance.match_type);
    }


    private void ResetButtonMusic(bool music_on)
    {
        bttn_music_text.text = (music_on ? "Music On" : "Music Off");
    }
    private void ResetButtonMatchType(int match_type)
    {
        bttn_match_type_text.text =
            match_type == 0 ? "5 Minute Match" :
            match_type == 1 ? "10 Minute Match" : "1 Minute Match";
    }
}
