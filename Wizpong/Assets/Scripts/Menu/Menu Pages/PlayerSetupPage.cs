using UnityEngine;
using System.Collections;

public class PlayerSetupPage : MenuPage
{
    public GUISkin skin1;
    public MenuPage game_setup_page;
    public ColorSelectionPage color_selection_page;
    public FadeThroughPage fade_page;
    public YesNoPopup yes_no_pu;
    public OkPopup ok_pu;



    void OnGUI()
    {
        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();

        GiveControlToPopups(new MenuPage[] { yes_no_pu, ok_pu });
        GUILayout.BeginArea(new Rect(50, 230, 1200, 900));
        GUILayout.BeginVertical();

        // header
        MenuHelper.GUILayoutHeader("Player Setup", t);

        // player settings
        GUILayout.BeginHorizontal();
        PlayerGUIColumn(1, t);
        PlayerGUIColumn(2, t);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndArea();
        

        // begin button
        if (MenuHelper.GUINextButton("Begin Match", 50, 200 * t))
        {
            if (GameSettings.PlayerSameColors())
            {
                ok_pu.text = "Each player must have a different color.";
                ok_pu.TransitionIn(null);
            }
            else
            {
                yes_no_pu.question_text = "Are you ready to begin?";
                yes_no_pu.on_no = null;
                yes_no_pu.on_yes = () => TransitionToGame();
                yes_no_pu.TransitionIn(null);
            }
        }

        // back button
        if (MenuHelper.GUIBackButton("Back", 50, 100 * t))
        {
            TransitionOut(null);
            game_setup_page.TransitionIn(null);
        }
    }
    private void PlayerGUIColumn(int player_number, float t)
    {
        GUILayout.BeginVertical();
        
        // heading
        GUIStyle header_style = skin1.GetStyle("label2");
        MenuHelper.SetGUIAlpha(t);
        GUILayout.Label("Player " + player_number, header_style);
        MenuHelper.SetGUIAlpha(1);
        

        // ai controlled?
        if (GUILayout.Button(GameSettings.ai_controlled[player_number-1] ? "AI" : "Human", GUILayout.Width(200 * t)))
        {
            GameSettings.ai_controlled[player_number-1] = !GameSettings.ai_controlled[player_number-1];
        }

        // racquet choice
        if (GUILayout.Button("random racquet", GUILayout.Width(340 * t)))
        {
        }

        // color choice
        if (GUILayout.Button(GameSettings.player_color_names[GameSettings.player_color_ID[player_number-1]], GUILayout.Width(340 * t)))
        {
            TransitionOut(null);
            color_selection_page.SetSelectingPlayer(player_number);
            color_selection_page.TransitionIn(null);
        }

        GUILayout.EndVertical();
    }

    private void TransitionToGame()
    {
        TransitionOut(null);
        fade_page.TransitionIn(() => LoadGame());
    }
    private void LoadGame()
    {
        Application.LoadLevel("Game");
    }
}
