using UnityEngine;
using System.Collections;

public class StunBallAudio : BallAudio
{
    public WorldSound stun_sound_prefab;

    new public void Awake()
    {
        ObjectPool.Instance.RequestObjects(stun_sound_prefab, 1, true);
        base.Awake();
    }

    public void PlayStunSound()
    {
        WorldSound s = ObjectPool.Instance.GetObject(stun_sound_prefab, false);

        s.transform.position = transform.position;
        s.Play();
    }
}
