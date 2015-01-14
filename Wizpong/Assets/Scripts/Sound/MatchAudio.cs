using UnityEngine;
using System.Collections;

public class MatchAudio : MonoBehaviour
{
    public AudioSource source_rewall;
    public AudioSource source_begin_point;
    public AudioSource source_point;
    public AudioSource source_bells;
    public AudioSource source_game_over;
    public AudioSource source_live_wall;
    public AudioSource source_possesion_change;
    public AudioSource source_alert_loop;


    public void PlayRewall()
    {
        source_rewall.volume = GameSettings.volume_fx;
        source_rewall.Play();
    }
    public void PlayBeginPoint()
    {
        source_begin_point.volume = GameSettings.volume_fx;
        source_begin_point.Play();
    }
    public void PlayPoint()
    {
        source_point.volume = GameSettings.volume_fx;
        source_bells.volume = GameSettings.volume_fx;
        source_point.Play();
        source_bells.Play();
    }
    public void PlayGameOver()
    {
        source_game_over.volume = GameSettings.volume_fx;
        source_game_over.Play();
    }
    public void PlayPosessionChange()
    {
        source_possesion_change.volume = GameSettings.volume_fx;
        source_possesion_change.Play();
    }
    public void PlayLiveWall()
    {
        source_live_wall.volume = GameSettings.volume_fx;
        source_live_wall.Play();
    }

    public void PlayAlertLoop()
    {
        source_alert_loop.volume = GameSettings.volume_fx;
        source_alert_loop.Play();
    }
    public void StopAlertLoop()
    {
        source_alert_loop.Stop();
    }
}
