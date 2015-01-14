using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSetupPage : UIMenuPage 
{
    public Text[] bttn_control_type_text;
    public Text[] bttn_color_text;
    public InputField[] name_input;
    public Image[] racquet_previews;

    public FadeScreenPage fadescreen_page;


    public void Start()
    {
        for (int pn = 1; pn < 3; ++pn)
        {
            ResetButtonControlType(pn, GameSettings.Instance.ai_controlled[pn - 1]);
            ResetInputFieldPlayerName(pn, GameSettings.Instance.player_name[pn - 1]);
            ResetColorPreviewAndButton(pn, GameSettings.Instance.GetPlayerColor(pn, true),
                GameSettings.Instance.player_color_names[GameSettings.Instance.player_color_ID[pn - 1]]);
        }
            
    }

    public void ButtonControlType(int player_number)
    {
        GameSettings.Instance.ai_controlled[player_number - 1] = ! GameSettings.Instance.ai_controlled[player_number - 1];
        ResetButtonControlType(player_number, GameSettings.Instance.ai_controlled[player_number - 1]);
    }
    public void InputFieldEnterPlayerName(int player_number)
    {
        GameSettings.Instance.player_name[player_number - 1] = name_input[player_number - 1].text;
    }
    public void ButtonBeginYes()
    {
        fadescreen_page.TransitionIn();
        fadescreen_page.on_transitioned_in = () => Application.LoadLevel("Game");

        TransitionOut();
    }


    public void ResetColorPreviewAndButton(int player_number, Color color, string color_name)
    {
        racquet_previews[player_number - 1].color = color;
        bttn_color_text[player_number - 1].text = color_name;
    }

    private void ResetButtonControlType(int player_number, bool ai_controlled)
    {
        bttn_control_type_text[player_number - 1].text = (ai_controlled ? "AI" : "Human");
    }
    private void ResetInputFieldPlayerName(int player_number, string name)
    {
        name_input[player_number - 1].placeholder.GetComponent<Text>().text = name;
    }
    
}
