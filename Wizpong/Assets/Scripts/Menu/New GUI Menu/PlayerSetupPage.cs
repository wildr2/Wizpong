using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSetupPage : UIMenuPage 
{
    public Text[] bttn_control_type_text;
    public Text[] bttn_color_text;


    public void ButtonControlType(int player_number)
    {
        GameSettings.ai_controlled[player_number-1] = !GameSettings.ai_controlled[player_number-1];
        bttn_control_type_text[player_number-1].text = (GameSettings.ai_controlled[player_number-1] ? "AI" : "Human");
    }
    public void ButtonBegin()
    {
        TransitionOut();
        on_transitioned_out = () => Application.LoadLevel("Game");
    }
}
