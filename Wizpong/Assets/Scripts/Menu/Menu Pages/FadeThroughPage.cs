using UnityEngine;
using System.Collections;

public class FadeThroughPage : MenuPage 
{
    public GUISkin skin1;
    public Color fade_through_color = Color.black;
    public bool start_going_out = false;


    public void Awake()
    {
        SetFadeSeconds(1);
        if (start_going_out)
        {
            transition = 1;
            TransitionOut(null);
        }
    }

    public void OnGUI()
    {
        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();


        GUI.color = fade_through_color;
        MenuHelper.SetGUIAlpha(t);
        GUI.Box(default_screen, "");
        GUI.color = Color.white;
    }

    public void SetFadeSeconds(float seconds)
    {
        transition_seconds = seconds;
    }

	
}
