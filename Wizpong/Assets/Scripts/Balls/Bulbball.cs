using UnityEngine;
using System.Collections;

public class Bulbball : Ball 
{
    public BallAudio ball_audio;
    public Lightning lightning;
    public SpriteRenderer light_sprite;
    public SpriteRenderer timer_ring;

    private const float max_ring_radius = 3;

    private const float countdown_max = 8;
    private float countdown;
    private bool counting_down = false;
    private const float stun_duration = 3;

    public Racquet[] racquets;


    public override void Initialize()
    {
        countdown = countdown_max;
        counting_down = false;
        base.Initialize();
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
        if (racquet == controlling_racquet) return;

        // audio
        ball_audio.PlayPossessSound();

        // visual
        light_sprite.color = Color.Lerp(racquet.player_color, Color.white, 0.25f);
        light_sprite.gameObject.SetActive(true);

        Color c = Color.Lerp(racquet.player_color, Color.white, 0.25f);
        c.a = 0.2f;
        //timer_ring.color = c;
        timer_ring.gameObject.SetActive(true);


        // countdown
        if (!counting_down)
        {
            counting_down = true;
            countdown = countdown_max;
            StartCoroutine("ShockCountDown");
        }

        base.TakeControl(racquet);
    }

    private void ResetOwnership()
    {
        controlling_racquet = null;
        light_sprite.gameObject.SetActive(false);
        timer_ring.gameObject.SetActive(false);
    }
    private IEnumerator ShockCountDown()
    {
        while (true)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0) break;

            timer_ring.transform.localScale = new Vector3(max_ring_radius, max_ring_radius, max_ring_radius) * (countdown / countdown_max); 

            yield return null;
        }

        foreach (Racquet r in racquets)
        {
            if (r != controlling_racquet)
            {
                lightning.Initialize(controlling_racquet);
                lightning.Fire(transform, r.transform, stun_duration);
            }
        }


        counting_down = false;

        // reset ownership of ball
        ResetOwnership();
    }
	
}
