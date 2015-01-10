using UnityEngine;
using System.Collections;

public class LearnPage : MenuPage 
{
    public GUISkin skin1;
    public MenuPage main_menu_page;


    void OnGUI()
    {
        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();


        GUILayout.BeginArea(new Rect(50, 230, 800, 500));
        GUILayout.BeginVertical();

        // header
        MenuHelper.GUILayoutHeader("Learn", t);

        // buttons
        if (GUILayout.Button("Play Tutorial Match", GUILayout.Width(320 * t)))
        {
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Keybindings", GUILayout.Width(320 * t)))
        {
        }
        if (GUILayout.Button("Advanced Tips", GUILayout.Width(320 * t)))
        {
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndArea();


        // back button
        if (MenuHelper.GUIBackButton("Back", 50, 100 * t))
        {
            TransitionOut(null);
            main_menu_page.TransitionIn(null);
        }
    }
}
