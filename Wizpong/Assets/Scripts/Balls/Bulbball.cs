using UnityEngine;
using System.Collections;

public class Bulbball : Ball 
{
    public BallAudio ball_audio;
    public Lightning lightning;
    public SpriteRenderer light_sprite;
    public SpriteRenderer timer_ring;

    private const float max_ring_radius = 3;

    private const float base_countdown = 1;
    private float current_countdown;
    private const float countdown_increment = 2;
    private const float base_stun_duration = 0.5f;
    private float current_stun_duration;
    private const float stun_duration_increment = 0.5f;

    private float countdown_timer;

    public Racquet[] racquets;


    public override void Initialize()
    {
        current_countdown = base_countdown;
        current_stun_duration = base_stun_duration;
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
        countdown_timer = current_countdown;

        if (controlling_racquet != racquet)
        {
            StopCoroutine("ShockCountDown");
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
            countdown_timer -= Time.deltaTime;
            if (countdown_timer <= 0) break;

            timer_ring.transform.localScale = new Vector3(max_ring_radius, max_ring_radius, max_ring_radius) * (countdown_timer / current_countdown); 

            yield return null;
        }

        foreach (Racquet r in racquets)
        {
            if (r != controlling_racquet)
            {
                lightning.Initialize(controlling_racquet);
                lightning.Fire(transform, r.transform, current_stun_duration);

                current_stun_duration += stun_duration_increment;
                current_countdown += countdown_increment;
            }
        }

        // reset ownership of ball
        ResetOwnership();
    }
	
}
