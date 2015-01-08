using UnityEngine;
using System.Collections;

public class MenuHelper : MonoBehaviour
{
    // instanced
    public GUISkin skin1;

    // static
    private static GUIStyle button_style = new GUIStyle();
    


    public void Awake()
    {
        button_style = skin1.GetStyle("button");
    }


    public static void SetGUIAlpha(float a)
    {
        Color c = GUI.color;
        c.a = a;
        GUI.color = c;
    }

    public static void GUILayoutHeader(string header_text, float t)
    {
        SetGUIAlpha(t);
        GUILayout.Label(header_text);
        GUILayout.Space(25);
        SetGUIAlpha(1);
    }
    public static bool GUINextButton(string text, float x, float width)
    {
        return GUI.Button(new Rect(x, 780 - button_style.fixedHeight - button_style.margin.top,
            width, button_style.fixedHeight), text);
    }
    public static bool GUIBackButton(string text, float x, float width)
    {
        return GUI.Button(new Rect(x, 780,
            width, button_style.fixedHeight), text);
    }
}
