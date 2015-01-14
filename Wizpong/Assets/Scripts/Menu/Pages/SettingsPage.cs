using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsPage : UIMenuPage
{
    public Slider sounds_volume_slider;
    public Slider music_volume_slider;


	public void SliderChangeSoundsVolume()
    {
        GameSettings.Instance.volume_fx = sounds_volume_slider.value;
    }
    public void SliderChangeMusicVolume()
    {
        GameSettings.Instance.volume_fx = sounds_volume_slider.value;
    }
}
