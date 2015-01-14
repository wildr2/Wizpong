using UnityEngine;
using System.Collections;

public class PauseMenuPage : InGameMenuPage
{
    public PauseController pause_controller;


    public void OnEnable()
    {
        UIAudio.Instance.PlayPause();
    }
    public void ButtonResume()
    {
        UIAudio.Instance.PlayUnPause();
        on_transitioned_out = () => pause_controller.UnPause();
        TransitionOut();
    }
}
