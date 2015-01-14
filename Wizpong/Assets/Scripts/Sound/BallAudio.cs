using UnityEngine;
using System.Collections;

public class BallAudio : MonoBehaviour
{
    public WorldSound bump_sound;
    public int bump_sound_pool_buffer = 2;

    public void Awake()
    {
        ObjectPool.Instance.RequestObjects(bump_sound, bump_sound_pool_buffer, true);
    }

    public void PlayBumpSound(Vector2 position, float force)
    {
        WorldSound s = ObjectPool.Instance.GetObject(bump_sound, false);

        s.transform.position = position;
        s.base_volume = force;
        s.SetPitchOffset(Random.Range(-0.05f, 0.05f));
        s.Play();
    }
	
}
