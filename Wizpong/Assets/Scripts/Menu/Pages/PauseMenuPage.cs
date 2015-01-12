using UnityEngine;
using System.Collections;

public class PauseMenuPage : UIMenuPage
{
    public FadeScreenPage fadepage;
    public PauseController pause_controller;

    private bool hidden = false;


    public void ButtonResume()
    {
        on_transitioned_out = () => pause_controller.UnPause();
        TransitionOut();
    }
    public void ButtonHideMenu()
    {
        if (hidden) return;

        transition = 0;
        foreach (UITransition obj in transition_objects)
        {
            obj.UpdateTransition(transition, false);
        }

        StartCoroutine("CheckForUnHideInput");
    }
    public void ButtonRestart()
    {
        Application.LoadLevel("Game");
    }
    public void ButtonQuit()
    {
        TransitionOut();
        fadepage.TransitionIn();
        fadepage.on_transitioned_in = () => Quit();
    }
    

    private void Quit()
    {
        SoundManager.Instance.music.Stop();
        Application.LoadLevel("Main Menu");
    }
    private IEnumerator CheckForUnHideInput()
    {
        while (true)
        {
            if (Input.anyKey)
            {
                hidden = false;
                TransitionIn();
                break;
            }

            yield return null;
        }
    }
}
