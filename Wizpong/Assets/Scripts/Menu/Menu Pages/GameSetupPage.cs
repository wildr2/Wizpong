using UnityEngine;
using System.Collections;

public class GameSetupPage : MenuPage 
{
    public GUISkin skin1;
    public MenuPage main_menu_page;
    public MenuPage player_setup_page;


    void OnGUI()
    {
        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();


        GUILayout.BeginArea(new Rect(50, 230, 800, 500));
        GUILayout.BeginVertical();

        // header
        MenuHelper.GUILayoutHeader("Game Setup", t);

        // buttons
        //GUILayout.BeginHorizontal();
        if (GUILayout.Button((GameSettings.match_type == 0 ? "5 Minute Match" : "10 Minute Match"), GUILayout.Width(320 * t)))
        {
            GameSettings.match_type = (GameSettings.match_type + 1) % 2;
        }
        //GUILayout.EndHorizontal();
        if (GUILayout.Button((GameSettings.music_on ? "Music On" : "Music Off"), GUILayout.Width(250 * t)))
        {
            GameSettings.music_on = !GameSettings.music_on;
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();


        // next button
        if (MenuHelper.GUINextButton("Next", 50, 100 * t))
        {
            TransitionOut(null);
            player_setup_page.TransitionIn(null);
        }

        // back button
        if (MenuHelper.GUIBackButton("Back", 50, 100 * t))
        {
            TransitionOut(null);
            main_menu_page.TransitionIn(null);
        }
    }
}
