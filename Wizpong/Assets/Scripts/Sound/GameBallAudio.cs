using UnityEngine;
using System.Collections;

public class GameBallAudio : MonoBehaviour
{
    public WorldSound bump_sound;


    public void Awake()
    {
        PooledObject p = bump_sound.GetComponent<PooledObject>();
        ObjectPool.Instance.RequestObjects(p, 3, true);
    }

    public void PlayBumpSound(Vector2 position, float force)
    {
        WorldSound s = ObjectPool.Instance.GetObject<WorldSound>(bump_sound, false);
        //WorldSound s = ObjectPool.Instance.GetPooledObject(bump_sound.GetComponent<PooledObject>(), false).GetComponent<WorldSound>();

        s.transform.position = position;
        s.base_volume = force;
        s.SetPitchOffset(Random.Range(-0.05f, 0.05f));
        s.Play();
    }
	
}
