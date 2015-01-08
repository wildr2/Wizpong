using UnityEngine;
using System.Collections;

public class YesNoPopup : MenuPage
{
    public GUISkin skin1;
    public string question_text;
    public System.Action on_yes, on_no;


    public void OnGUI()
    {
        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();


        BackgroundDarkness(t);
        GUI.color = new Color(0.1f, 0.1f, 0.3f, 1); // SHOULD COLOR BE SET HERE? DIFFERENT GUI TEXTURE?
        MenuHelper.SetGUIAlpha(t);
        GUI.Window(0, new Rect(default_screen.width / 2f - 160, default_screen.height / 2f - 120, 400, 210), Window, "");
        GUI.color = Color.white;
    }

    private void Window(int windowID)
    {
        float t = TransitionPow();


        GUILayout.BeginVertical();

        MenuHelper.SetGUIAlpha(t);
        GUILayout.Label(question_text);
        MenuHelper.SetGUIAlpha(1);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Yes", GUILayout.Width(80 * t)))
        {
            if (on_yes != null) on_yes();
            TransitionOut(null);
        }
        if (GUILayout.Button("No", GUILayout.Width(80 * t)))
        {
            if (on_no != null) on_no();
            TransitionOut(null);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void BackgroundDarkness(float t)
    {
        GUI.color = Color.black;
        MenuHelper.SetGUIAlpha(t / 2f);
        GUI.Box(default_screen, "");
        GUI.color = Color.white;
    }
}
