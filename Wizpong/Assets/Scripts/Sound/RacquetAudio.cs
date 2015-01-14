using UnityEngine;
using System.Collections;

public class RacquetAudio : MonoBehaviour
{
    public WorldSound stun_sound;

    public void PlayStunSound()
    {
        stun_sound.transform.position = transform.position;
        stun_sound.Play();
    }
}
