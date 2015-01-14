using UnityEngine;
using System.Collections;

public class InGameMenuPage : UIMenuPage 
{
    public FadeScreenPage fadepage;

    private bool hidden = false;


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
