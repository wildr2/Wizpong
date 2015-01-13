using UnityEngine;
using System.Collections;

public class PauseMenuPage : InGameMenuPage
{
    public PauseController pause_controller;


    public void ButtonResume()
    {
        on_transitioned_out = () => pause_controller.UnPause();
        TransitionOut();
    }
}
