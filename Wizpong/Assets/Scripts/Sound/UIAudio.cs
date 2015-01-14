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


    public void PlayButtonHover()
    {
        Instance.source_button_hover.volume = 0.1f * GameSettings.volume_fx;
        Instance.source_button_hover.Play();
    }
    public void PlayButtonClick()
    {
        Instance.source_button_click.volume = GameSettings.volume_fx;
        Instance.source_button_click.Play();
    }
    public void PlayAlert()
    {
        Instance.source_alert.volume = GameSettings.volume_fx;
        Instance.source_alert.Play();
    }
}
