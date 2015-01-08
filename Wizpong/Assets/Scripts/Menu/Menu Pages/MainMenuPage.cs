using UnityEngine;
using System.Collections;

public class MainMenuPage : MenuPage 
{
    public GUISkin skin1;
    public MenuPage learn_page;
    public MenuPage game_setup_page;
    public YesNoPopup yes_no_pu;


    private bool test = false;


    public void OnGUI()
    {
        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();

        GiveControlToPopups(new MenuPage[] { yes_no_pu });
        GUILayout.BeginArea(new Rect(50, 230, 500, 800));
        GUILayout.BeginVertical();

        // header
        MenuHelper.GUILayoutHeader("", t);
        
        // buttons
        GUILayout.BeginHorizontal();
        NextVerticalKeyboardControl("Learn");
        if (GUILayout.Button("Learn", GUILayout.Width(120*t)) || KBControlPressed("Learn"))
        {
            TransitionOut(null);
            learn_page.TransitionIn(null);
        }
        if (LastControlHover("Learn")) { SetKeyBoardFocus("Learn"); }
        NextVerticalKeyboardControl("Practice");
        if (GUILayout.Button("Practice", GUILayout.Width(150*t)) || KBControlPressed("Practice"))
        {
            yes_no_pu.question_text = "Are you sure you want to practice?";
            yes_no_pu.TransitionIn(null);
        }
        if (LastControlHover("Practice")) { SetKeyBoardFocus("Practice"); }
        GUILayout.EndHorizontal();
        NextVerticalKeyboardControl("Settings");
        if (GUILayout.Button("Settings", GUILayout.Width(150*t)) || KBControlPressed("Settings"))
        {
        }
        if (LastControlHover("Settings")) { SetKeyBoardFocus("Settings"); }
        NextVerticalKeyboardControl("Exit");
        if (GUILayout.Button("Exit", GUILayout.Width(100*t)) || KBControlPressed("Exit"))
        {
            yes_no_pu.question_text = "Are you sure you want to exit?";
            yes_no_pu.TransitionIn(null);
            yes_no_pu.on_no = null;
            yes_no_pu.on_yes = () => Application.Quit();
        }
        if (LastControlHover("Exit")) { SetKeyBoardFocus("Exit"); }

        GUILayout.EndVertical();
        GUILayout.EndArea();


        // play button
        NextVerticalKeyboardControl("Play");
        if (MenuHelper.GUINextButton("Play", 50, 100 * t) || KBControlPressed("Play"))
        {
            TransitionOut(null);
            game_setup_page.TransitionIn(null);
        }
        //if (LastControlHover("Play")) { SetKeyBoardFocus("Play"); }


        EndKeyboardControlSetup();



        // TESTING 
        if (test == false)
        {
            //DebugLogKBNodes();
            test = true;
        }

        
    }
}
