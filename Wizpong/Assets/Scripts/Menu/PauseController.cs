using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour 
{
    public PauseMenuPage pause_menu;
    public FadeScreenPage fade_page;
    public MatchManager match_manager;
    public bool Paused { get; private set; }


    public void Start()
    {
        Pause();
        fade_page.on_transitioned_out = () => UnPause();
    }
    public void Update()
    {
        // only allow pausing and unpausing when the game is not over
        //   and a transition (restart or quit) is not taking place
        if (Input.GetKeyDown(KeyCode.Escape) && !fade_page.IsTransitioning() && !match_manager.GameOver())
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
