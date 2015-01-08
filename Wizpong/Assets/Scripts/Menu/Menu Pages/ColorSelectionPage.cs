using UnityEngine;
using System.Collections;

public class ColorSelectionPage : MenuPage
{
    public GUISkin skin1;
    public MenuPage player_setup_page;

    private int player_number = 1; // which player are we selecting a color for
    private int highlighted_index = 0; // which button is highlighted
    



    void OnGUI()
    {
        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();


        GUILayout.BeginArea(new Rect(50, 230, 1000, 800));

        // header
        MenuHelper.GUILayoutHeader("Choose a racquet color", t);

        
        ColorTable2(t);

        GUILayout.EndArea();
    }

    private void ColorTable(float t)
    {
        GUIStyle color_button = skin1.GetStyle("color button");

        int rows = 4;
        int cols = (int)Mathf.Ceil(GameSettings.player_colors.Length / (float)rows);


        GUILayout.BeginHorizontal();
        for (int x = 0; x < cols; ++x)
        {
            GUILayout.BeginVertical();
            for (int y = 0; y < rows; ++y)
            {
                int color_index = x * rows + y;
                if (color_index >= GameSettings.player_colors.Length) break;
                GUI.color = GameSettings.player_colors[color_index];

                
                // button
                bool focused = color_index == highlighted_index;
                if (!focused) GUI.color = Color.Lerp(GUI.color, Color.black, 0.4f);
                MenuHelper.SetGUIAlpha(t);

                GUI.SetNextControlName(color_index.ToString());
                if (GUILayout.Button(color_index == 0 ? "?" : "", color_button, GUILayout.Width(110), GUILayout.Height(110)))
                {
                    GameSettings.player_color_ID[player_number-1] = color_index;
                    TransitionOut(null);
                    player_setup_page.TransitionIn(null);
                }


                // mouse over
                if (Event.current.type == EventType.Repaint && !IsGoingOut() &&
                    GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    highlighted_index = color_index;
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
    private void ColorTable2(float t)
    {
        GUIStyle color_button = skin1.GetStyle("color button");

        int rows = 4;
        int cols = (int)Mathf.Ceil(GameSettings.player_colors.Length / (float)rows);


        for (int x = 0; x < cols; ++x)
        {
            for (int y = 0; y < rows; ++y)
            {
                int color_index = x * rows + y;
                if (color_index >= GameSettings.player_colors.Length) break;
                GUI.color = GameSettings.player_colors[color_index];


                // button
                bool focused = color_index == highlighted_index;
                if (!focused) GUI.color = Color.Lerp(GUI.color, Color.black, 0.4f);
                MenuHelper.SetGUIAlpha(t);

                GUI.SetNextControlName(color_index.ToString());
                Rect rect = focused ? new Rect(20 + x*(110 + 20) - 10, 80 + y*(110 + 20) - 10, 120, 120)
                    :                 new Rect(20 + x*(110 + 20),      80 + y*(110 + 20),       100,  100);

                if (GUI.Button(rect, color_index == 0 ? "?" : "", color_button))
                {
                    GameSettings.player_color_ID[player_number - 1] = color_index;
                    TransitionOut(null);
                    player_setup_page.TransitionIn(null);
                }


                // mouse over
                if (Event.current.type == EventType.Repaint && !IsGoingOut() &&
                     rect.Contains(Event.current.mousePosition))
                {
                    highlighted_index = color_index;
                }
            }
        }
    }


    public void SetSelectingPlayer(int player_number)
    {
        this.player_number = player_number;
    }

}
