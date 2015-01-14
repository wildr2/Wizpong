using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class WorldSound : MonoBehaviour
{
    public bool timescaled_pitch = true;
    public float base_volume = 1;
    public float base_pitch = 1;
    private float pitch_offset = 0;


    public void Update()
    {
        // volume
        audio.volume = base_volume * GameSettings.Instance.volume_fx;

        // pitch
        audio.pitch = Mathf.Max(base_pitch + pitch_offset, 0);
        if (timescaled_pitch) audio.pitch *= Time.timeScale;

        // disable when finished
        if (!audio.isPlaying) gameObject.SetActive(false);
    }

    public void Play()
    {
        gameObject.SetActive(true);
        audio.Play();
        Update();
    }

    public void SetPitchOffset(float offset)
    {
        pitch_offset = offset;
    }
	
}
