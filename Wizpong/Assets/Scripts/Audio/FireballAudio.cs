using UnityEngine;
using System.Collections;

public class FireballAudio : BallAudio
{
    public WorldSound stun_sound_prefab;
    public AudioSource fire_loop;
    
    public ParticleSystem fire_trail;
    public float fire_loop_base_volume = 0.5f;


    new public void Awake()
    {
        ObjectPool.Instance.RequestObjects(stun_sound_prefab, 1, true);
        base.Awake();

        fire_loop.volume = 0;
        fire_loop.Play();
    }
    public void Update()
    {
        fire_loop.volume = fire_loop_base_volume * ((float)fire_trail.particleCount / fire_trail.maxParticles);
    }

    public void PlayStunSound()
    {
        WorldSound s = ObjectPool.Instance.GetObject(stun_sound_prefab, false);

        s.transform.position = transform.position;
        s.Play();
    }
}
