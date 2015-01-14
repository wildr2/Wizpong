using UnityEngine;
using System.Collections;

public class ControlZoneAudio : MonoBehaviour 
{
    public AudioSource source;
    public float pitch_offset = 1;
    public float base_max_volume = 0.1f;
    private float max_volume = 0.1f;

    private bool playing = false;
    private float fade_speed = 8f;


    public void Update()
    {
        max_volume = base_max_volume * GameSettings.volume_fx;

        if (playing)
        {
            // fade in
            if (source.volume < max_volume)
            {
                source.volume += Time.deltaTime * fade_speed * max_volume;
                source.volume = Mathf.Min(max_volume, source.volume);
            }

            // adjust pitch to time scale
            source.pitch = (1 + pitch_offset) * Time.timeScale;
        }
        else
        {
            // fade out
            if (source.volume > 0)
            {
                source.volume -= Time.deltaTime * fade_speed * max_volume;
                if (source.volume <= 0)
                {
                    source.Pause();
                    source.volume = 0;
                }
            }
        }
        
    }
    public void StartEffect()
    {
        playing = true;
        source.Play();
    }
    public void StopEffect()
    {
        playing = false;
    }
}
