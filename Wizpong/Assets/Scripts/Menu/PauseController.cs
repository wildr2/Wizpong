using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour 
{
    public PauseMenuPage pause_menu;
    public bool Paused { get; private set; }


    public void Start()
    {
        Paused = false;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused) pause_menu.ButtonResume();
            else
            {
                Pause();
                pause_menu.TransitionIn();
            }
        }
    }
    public void Pause()
    {
        TimeScaleManager.AddMultiplier("pause", 0);
        Paused = true;
    }
    public void UnPause()
    {
        TimeScaleManager.RemoveMultiplier("pause");
        Paused = false;
    }

}
