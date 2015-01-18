using UnityEngine;
using System.Collections;

public class MatchBallSpawner : MonoBehaviour
{
    public Ball[] spawn_queue;
    public MatchAudio match_audio;
    private int spawn_index = 0;

    private float spawn_interval;
    private float[] match_type_spawn_intervals = new float[]
    { 
        40,    // 5 min match
        80,    // 10 min match
        10     // 1 min match
    };


    public void Start()
    {
        spawn_interval = match_type_spawn_intervals[GameSettings.Instance.match_type];
        SpawnNext();
    }

    public void SpawnNext()
    {
        // spawn
        spawn_queue[spawn_index].Initialize();
        spawn_queue[spawn_index].gameObject.SetActive(true);

        // audio
        if (spawn_index != 0) match_audio.PlaySpawnBall(); // don't whistle for the first ball

        // prepare to spawn the next ball
        ++spawn_index;

        if (spawn_index >= spawn_queue.Length) return;
        StartCoroutine("SpawnNextAfterWait");
    }
    public IEnumerator SpawnNextAfterWait()
    {
        yield return new WaitForSeconds(spawn_interval);

        SpawnNext();
    }
}
