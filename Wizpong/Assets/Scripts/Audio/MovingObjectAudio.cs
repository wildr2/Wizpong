using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MovingObjectAudio : MonoBehaviour 
{
    public float max_move_speed = 10;
    public float max_pitch = 2;
    public float pitch_offset = 0;

    public Rigidbody2D rigidbody;


    public void Update()
    {
        float speed_factor = Mathf.Clamp(rigidbody.velocity.magnitude / max_move_speed, 0, 1);

        // louder volume when moving faster
        audio.volume = speed_factor; 

        // faster playback when moving faster
        audio.pitch = pitch_offset + speed_factor * Time.timeScale * max_pitch;
    }
}
