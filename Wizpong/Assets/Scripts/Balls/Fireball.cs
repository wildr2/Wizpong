using UnityEngine;
using System.Collections;

public class Fireball : Ball 
{
    // Other references
    public FireballAudio ball_audio;
    public FireballTrailSegment trail_segment_prefab;

    private float distance_per_segment = 0.5f;
    private float distance = 0;


    new public void Start()
    {
        ObjectPool.Instance.RequestObjects(trail_segment_prefab, 60, true);

        base.Start();
    }
    public void Update()
    {
        distance += rigidbody2D.velocity.magnitude * Time.deltaTime;
        if (distance > distance_per_segment)
        {
            distance = 0;

            // create trail segment
            FireballTrailSegment segment = ObjectPool.Instance.GetObject(trail_segment_prefab, false);
            segment.transform.position = transform.position;
            segment.Initialize(this);
        }
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Wall"))
        {
            ball_audio.PlayBumpSound(rigidbody2D.velocity.magnitude / 50f);
        }
        else if (col.collider.CompareTag("Ball"))
        {
            ball_audio.PlayBumpSound(rigidbody2D.velocity.magnitude / 50f);
        }
    }

    public override void TakeControl(Racquet racquet)
    {
        ball_audio.PlayPossessSound();
        base.TakeControl(racquet);
    }

    public Racquet ControllingRacquet()
    {
        return controlling_racquet;
    }
	
}
