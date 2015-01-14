using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class WorldSound : MonoBehaviour
{
    public bool timescaled_pitch = true;
    private float initial_volume;
    private float initial_pitch;


    public void Start()
    {
        initial_volume = audio.volume;
        initial_pitch = audio.pitch;
    }
    public void Update()
    {
        // volume
        audio.volume = initial_volume;
        if (timescaled_pitch) audio.volume *= Time.timeScale * GameSettings.volume_fx;

        // pitch
        audio.pitch = initial_pitch;
        if (timescaled_pitch) audio.volume *= Time.timeScale;

        // disable when finished
        if (!audio.isPlaying) gameObject.SetActive(false);
    }

    public void Play()
    {
        gameObject.SetActive(true);
        audio.Play();
        Update();
    }
	
}
