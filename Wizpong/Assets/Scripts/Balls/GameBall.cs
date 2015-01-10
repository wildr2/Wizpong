using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameBall : MonoBehaviour
{
    // References
    public SpriteRenderer ball_renderer;
    public ParticleSystem particle_sys;
    private CameraShake cam_shake;

    // Shrink
    private bool shrinking = false;
    private float life_time, life_time_initial = 9f;
    private float scale;
    private float particle_size_initial;


    // Wall
    // whether there was a wall bounce since the last racquet touc
    private bool hit_wall_since_racquet = false; 
    private Wall last_wall; // the last wall hit by the ball


    // Events
    public event EventHandler<EventArgs<Wall>> event_collide_wall;
    public event EventHandler<EventArgs> event_on_death; // on death is when the ball has shrunk to nothing


    // PUBLIC MODIFIERS

    public void Start()
    {   
        life_time = life_time_initial;

        // cam shake reference
        cam_shake = Camera.main.GetComponent<CameraShake>();
        if (!cam_shake) Debug.LogError("main camera has no CameraShake component");

        // particle system
        particle_size_initial = particle_sys.startSize;
    }
    public void Update()
    {
        // Drag
        rigidbody2D.velocity /= (1 + 1.1f * Time.deltaTime);


        // Shrink
        if (shrinking)
        {
            life_time -= Time.deltaTime;
            UpdateSize();

            // on death (shrink to nothing) fire an event
            if (life_time <= 0)
            {
                shrinking = false;
                SetTrailEnabled(false);
                if (event_on_death != null) event_on_death(this, new EventArgs());
            }
        }
    }
   
    public void OnCollisionEnter2D(Collision2D col)
    {
        // VS Wall
        if (col.collider.CompareTag("Wall"))
        {
            Wall wall = col.collider.GetComponent<Wall>();
            
            // audio
            SoundManager.PlayGameBallBump(transform.position, rigidbody2D.velocity.magnitude / 100f);

            // fire event
            if (event_collide_wall != null) event_collide_wall(this, new EventArgs<Wall>(wall));

            // wall info
            last_wall = wall;
            hit_wall_since_racquet = true;
        }
        else if (col.collider.CompareTag("Ball"))
        {
            // audio
            SoundManager.PlayGameBallBump(transform.position, rigidbody2D.velocity.magnitude / 100f);
        }
       
    }
    public void RacquetTouch()
    {
        hit_wall_since_racquet = false;
    }

    public void Hide()
    {
        rigidbody2D.isKinematic = true;
        collider2D.enabled = false;
        ball_renderer.enabled = false;
        SetTrailEnabled(false);
    }
    public void Reset()
    {
        // visual
        ball_renderer.enabled = true;
        SetTrailEnabled(false);
        SetTrailColor(Color.white);
        StartCoroutine("ReenableParticles");

        // movement and position
        rigidbody2D.isKinematic = false;
        collider2D.enabled = true;
        transform.position = new Vector2(0, 0);
        rigidbody2D.velocity = Vector2.zero;

        // size
        shrinking = false;
        ResetLifeTime();

        // wall info
        last_wall = null;
    }

    public void SetTrailColor(Color color)
    {
        particle_sys.startColor = color;
    }
    public void SetTrailEnabled(bool enabled)
    {
        particle_sys.enableEmission = enabled;
    }
    public void ResetLifeTime()
    {
        life_time = life_time_initial;
        UpdateSize();
    }
    public void SetShrinking(bool shrink)
    {
        this.shrinking = shrink;
    }


    // PRIVATE MODIFIERS

    private void UpdateSize()
    {
        scale = life_time / life_time_initial;
        ball_renderer.transform.localScale = new Vector3(scale, scale, 0);
        particle_sys.startSize = particle_size_initial * (0.5f + scale / 2f);
    }
    private IEnumerator ReenableParticles()
    {
        yield return null;
        particle_sys.enableEmission = true;
    }


    // PUBLIC ACCESSORS

    public bool Alive()
    {
        return life_time > 0;
    }
    public Wall LastWall()
    {
        return last_wall;
    }
    public bool HitWallSinceRacquetTouch()
    {
        return hit_wall_since_racquet;
    }
    public float GetLifeTimeRemaining()
    {
        return life_time;
    }
}
