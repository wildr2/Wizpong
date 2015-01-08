using UnityEngine;
using System.Collections;

public class YesNoPopupJokes : YesNoPopup
{
    private Timer hahaha_timer = new Timer(2);
    private Timer hahaha_timer2 = new Timer(1.1f);
    private string[] negative_lines = { "I'm very sorry, but you didn't pay.", "Nope. \n", "Afraid I can't do that. \n", "Too bad. \n", "Sucks to suck. \n", "None shall pass. \n" };
    private string[] positive_lines = { "Oh alright. \n", "Ok ok ok. \n", "Well if you insist \n" };
    private float success_chance = 0.2f;
    private bool success = false;


    public void Start()
    {
        hahaha_timer.on_time_up = LOL;
        hahaha_timer2.on_time_up = LOL2;
    }
    private void LOL()
    {
        if (Random.Range(0f, 1f) < success_chance)
        {
            success = true;
            question_text = positive_lines[Random.Range(0, positive_lines.Length)];
        }
        else
        {
            success = false;
            question_text = negative_lines[Random.Range(0, negative_lines.Length)];
        }

        hahaha_timer2.StartTicking();
    }
    private void LOL2()
    {
        if (success) on_yes();
        TransitionOut(null);
    }


    

    public void OnGUI()
    {
        // #jokes
        hahaha_timer.UpdateIfTicking();
        hahaha_timer2.UpdateIfTicking();
        //

        GUI.skin = skin1;
        EnableGUIScale();
        float t = TransitionPow();


        BackgroundDarkness(t);
        GUI.color = new Color(0.1f, 0.1f, 0.3f, 1); // SHOULD COLOR BE SET HERE? DIFFERENT GUI TEXTURE?
        MenuHelper.SetGUIAlpha(TransitionPow());
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
            hahaha_timer.StartTicking();
        }
        if (GUILayout.Button("No", GUILayout.Width(80 * t)))
        {
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
