﻿using UnityEngine;
using System.Collections;

public class ShockBall : MonoBehaviour
{
    public BallAudio ball_audio;

    private int controlling_player = 0; // 0 is noone, 1 is player 1...

    public SpriteRenderer sprite_renderer;
    public TextMesh charge_text;
    private Color color_uncontrolled;

    private int charges = 0;
    private const int max_charges = 2;


    public void Start()
    {
        color_uncontrolled = sprite_renderer.color;

        // BUG FIX - bug in which the collider will not move with the ball when the ball is parented to an empty gameobject...
        collider2D.enabled = false;
        collider2D.enabled = true;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Wall"))
        {
            // audio
            ball_audio.PlayBumpSound(rigidbody2D.velocity.magnitude / 50f);
        }
        else if (col.collider.CompareTag("Ball"))
        {
            // audio
            ball_audio.PlayBumpSound(rigidbody2D.velocity.magnitude / 50f);
        }
    }

    public void TakeControlOfBall(Racquet racquet)
    {
        // set controlling player
        controlling_player = racquet.player_number;
        sprite_renderer.color = Color.Lerp(color_uncontrolled, racquet.player_color, 0.5f);

        charges = max_charges;
        charge_text.text = charges.ToString();

        // audio
        ball_audio.PlayPossessSound();
    }
    public void UseCharge()
    {
        charges -= 1;
        if (charges == 0)
        {
            // set as uncontrolled
            controlling_player = 0;
            sprite_renderer.color = color_uncontrolled;
            charge_text.text = "";
        }
        else
            charge_text.text = charges.ToString();
    }


    public int ControllingPlayer()
    {
        return controlling_player;
    }

}
