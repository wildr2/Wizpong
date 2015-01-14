using UnityEngine;
using System.Collections;

public class UIAudio : MonoBehaviour 
{
    private static UIAudio _instance = null;
    public static UIAudio Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIAudio>();

                if (_instance == null) Debug.LogError("Missing UIAudio");
            }
            return _instance;
        }
    }

    public AudioSource source_button_hover;
    public AudioSource source_button_click;
    public AudioSource source_alert;
    public AudioSource source_pause;
    public AudioSource source_unpause;


    public void Update()
    {
        Instance.source_alert.volume = GameSettings.Instance.volume_fx;
    }

    public void PlayButtonHover()
    {
        Instance.source_button_hover.volume = 0.1f * GameSettings.Instance.volume_fx;
        Instance.source_button_hover.Play();
    }
    public void PlayButtonClick()
    {
        Instance.source_button_click.volume = GameSettings.Instance.volume_fx;
        Instance.source_button_click.Play();
    }
    public void PlayAlert()
    {
        Instance.source_alert.volume = GameSettings.Instance.volume_fx;
        Instance.source_alert.Play();
    }
    public void PlayPause()
    {
        Instance.source_pause.volume = GameSettings.Instance.volume_fx;
        Instance.source_pause.Play();
    }
    public void PlayUnPause()
    {
        Instance.source_unpause.volume = GameSettings.Instance.volume_fx;
        Instance.source_unpause.Play();
    }
    public void StartSoundTest()
    {
        Instance.source_alert.volume = GameSettings.Instance.volume_fx;
        Instance.source_alert.loop = true;
        Instance.source_alert.Play();
    }
    public void StopSoundTest()
    {
        Instance.source_alert.loop = false;
    }
}
