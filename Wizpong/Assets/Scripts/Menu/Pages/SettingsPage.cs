using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsPage : UIMenuPage
{
    public Slider sounds_volume_slider;
    public Slider music_volume_slider;


    new public void Start()
    {
        sounds_volume_slider.value = GameSettings.Instance.volume_fx;
        music_volume_slider.value = GameSettings.Instance.volume_music;

        base.Start();
    }
	public void SliderChangeSoundsVolume()
    {
        GameSettings.Instance.volume_fx = sounds_volume_slider.value;
    }
    public void SliderChangeMusicVolume()
    {
        GameSettings.Instance.volume_music = music_volume_slider.value;
    }
}
