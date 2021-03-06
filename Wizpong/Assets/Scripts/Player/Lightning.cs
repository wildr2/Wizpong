﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Lightning : MonoBehaviour 
{
    // Visual
    private int redraws_per_stun_second = 2;
    private int redraws;
    private int redraw_count = 0;

    private const int max_num_positions = 30;
    private const float zigzag_intensity = 0.5f;
    public LineRenderer line;

    private Transform bolt_start, bolt_end;
    private Vector2 direction;

    // stun
    //private const float stun_duration = 0.75f;
    //private int power = 1; // stun_duration is multiplied by power
    private float stun_duration;

    // audio
    public WorldSound shock_sound;

    // References
    private Racquet racquet;
    private CameraShake cam_shake;
    public LayerMask racquets_layer;



    // PUBLIC MODIFIERS

    public void Initialize(Racquet racquet)
    {
        this.racquet = racquet;
        line.SetColors(Color.Lerp(racquet.player_color, Color.white, 0.5f), racquet.player_color);

        cam_shake = Camera.main.GetComponent<CameraShake>();
        if (!cam_shake) Debug.LogError("no CameraShake found");
    }
    public void Fire(Transform bolt_start, Transform bolt_end, float stun_duration)
    {
        this.bolt_start = bolt_start;
        this.bolt_end = bolt_end;

        // visual
        cam_shake.Shake(new CamShakeInstance(0.3f * stun_duration, 0.1f));
        line.enabled = true;
        redraw_count = 0;
        redraws = (int)(redraws_per_stun_second * stun_duration);
        
        // audio
        shock_sound.Play();

        // draw and collision (stun players)
        this.stun_duration = stun_duration;
        StartCoroutine(ReCreateBolt());
    }


    // PRIVATE MODIFIERS

    private IEnumerator ReCreateBolt()
    {
        while (redraw_count < redraws)
        {
            // redraw and collide
            ReDrawLine();
            HandleCollision();

            ++redraw_count;
            yield return new WaitForSeconds(0.08f);
        }

        // finish bolt
        redraw_count = 0;
        line.enabled = false;
    }
    private void ReDrawLine()
    {
        Vector2 pos1 = bolt_start.position;
        Vector2 pos2 = bolt_end.position;

        float dist = (pos2 - pos1).magnitude;
        int num_positions = (int)(max_num_positions *  Mathf.Min(dist / 20f, 1));

        for (int i = 0; i <= num_positions; ++i)
        {
            line.SetPosition(i, pos1);
            pos1 = Vector2.Lerp(pos1, pos2, i / (float)num_positions);
            pos1 += new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * zigzag_intensity;
        }

        // clear the unused positions
        for (int i = num_positions; i <= max_num_positions; ++i)
        {
            line.SetPosition(i, pos2);
        }
    }
    private void HandleCollision()
    {
        Vector2 pos1 = bolt_start.position;
        Vector2 pos2 = bolt_end.position;

        // see if the player is hit (and hit them)
        RaycastHit2D[] hits = Physics2D.LinecastAll(pos1, pos2, racquets_layer.value);

        foreach (RaycastHit2D hit in hits)
        {
            // stun opponent racquet
            Racquet r = hit.collider.GetComponent<Racquet>();
            if (r != null && r.player_number != racquet.player_number) // affect only the opponent
            {
                r.Stun(stun_duration);
            }
        }
    }
}
