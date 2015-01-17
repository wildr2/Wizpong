using UnityEngine;
using System.Collections;

public class Snowball : Ball
{
    // Other references
    public SpriteRenderer sprite_renderer;
    public BallAudio ball_audio;
    public Collider2D slow_area_collider;


    new public void Start()
    {
        // BUG FIX - bug in which the collider will not move with the ball when the ball is parented to an empty gameobject...
        slow_area_collider.enabled = false;
        slow_area_collider.enabled = true;

        base.Start();
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Wall"))
        {
            ball_audio.PlayBumpSound(rigidbody2D.velocity.magnitude / 50f);
            collider2D.rigidbody2D.velocity *= 0.3f;
        }
        else if (col.collider.CompareTag("Ball"))
        {
            ball_audio.PlayBumpSound(rigidbody2D.velocity.magnitude / 50f);
            collider2D.rigidbody2D.velocity *= 0.3f;
        }
    }
    public void OnTriggerStay2D(Collider2D collider)
    {
        // Slow area vs Racquet
        if (collider.CompareTag("Racquet"))
        {
            Racquet r = collider.GetComponent<Racquet>();
            if (r != null)
            {
                r.Slow(0.1f);
            }
        }
        else if (collider.CompareTag("Ball"))
        {
            collider.rigidbody2D.velocity *= 0.8f;
        }
    }

    public override void TakeControl(Racquet racquet)
    {
        ball_audio.PlayPossessSound();
        base.TakeControl(racquet);
    }

}
