using UnityEngine;
using System.Collections;

public class RacquetAudio : MonoBehaviour
{
    public WorldSound stun_sound;

    public void PlaySelfStunSound()
    {
        stun_sound.transform.position = transform.position;
        stun_sound.Play();
    }
}
