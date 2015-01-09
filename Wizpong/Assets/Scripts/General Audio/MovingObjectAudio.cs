using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingObjectAudio : MonoBehaviour 
{
    public float max_move_speed = 10;
    public float max_pitch = 2;
    public float pitch_offset = 0;

    public void Update()
    {
        float speed_factor = Mathf.Clamp(rigidbody2D.velocity.magnitude / max_move_speed, 0, 1);

        // louder volume when moving faster
        audio.volume = speed_factor; 

        // faster playback when moving faster
        audio.pitch = pitch_offset + speed_factor * Time.timeScale * max_pitch;
    }
}
