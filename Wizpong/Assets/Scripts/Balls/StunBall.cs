using UnityEngine;
using System.Collections;

public class StunBall : MonoBehaviour 
{
    // Other references
    private CameraShake cam_shake;
    public StunBallAudio ball_audio;

    private int controlling_player = 0; // 0 is noone, 1 is player 1...
    private const float duration_controlled = 2;
    private const float stun_duration = 2;

    public SpriteRenderer sprite_renderer;
    private Color color_uncontrolled;

    private Collider2D controlling_colider;
    public string layer_name_active_stun_balls = "Active Stun Balls";
    public string layer_name_balls = "Balls";
    private int layer_active_stun_balls, layer_balls;

    
    


    public void Start()
    {
        color_uncontrolled = sprite_renderer.color;
        layer_active_stun_balls = LayerMask.NameToLayer(layer_name_active_stun_balls);
        layer_balls = LayerMask.NameToLayer(layer_name_balls);

        cam_shake = Camera.main.GetComponent<CameraShake>();
        if (!cam_shake) Debug.LogError("main camera has no CameraShake component");

        // BUG FIX - bug in which the collider will not move with the ball when the ball is parented to an empty gameobject...
        collider2D.enabled = false;
        collider2D.enabled = true;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        // VS Racquet (physical)
        if (col.collider.CompareTag("Racquet"))
        {
            if (controlling_player != 0)
            {
                Racquet r = col.collider.GetComponent<Racquet>();
                if (r != null && r.player_number != controlling_player && controlling_player != 0)
                {
                    HitRacquet(r);
                }
            }
        }
        
        else if (col.collider.CompareTag("Wall"))
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
    public void TryTakeControlOfBall(Racquet racquet, Collider2D physical_collider)
    {
        if (controlling_player == 0)
        {
            // set controlling player
            controlling_player = racquet.player_number;
            sprite_renderer.color = Color.Lerp(color_uncontrolled, racquet.player_color, 0.35f);

            
            // don't collide with controlling physical racquet
            controlling_colider = physical_collider;
            Physics2D.IgnoreCollision(this.collider2D, controlling_colider); 

            // collide with opponent physical racquet
            gameObject.layer = layer_active_stun_balls;


            StartCoroutine("ResetControllingPlayer");

            // audio
            ball_audio.PlayPossessSound();
        } 
    }


    private void HitRacquet(Racquet r)
    {
        r.Stun(stun_duration);
        ball_audio.PlayStunSound();
        cam_shake.Shake(new CamShakeInstance(0.4f, 0.5f));
    }
    private IEnumerator ResetControllingPlayer()
    {
        yield return new WaitForSeconds(duration_controlled);
        
        controlling_player = 0;
        sprite_renderer.color = color_uncontrolled;


        // let old controlling player physical racquet collide with this ball again (after it is controlled by someone else)
        Physics2D.IgnoreCollision(this.collider2D, controlling_colider, false);

        // don't collide with physical racquets
        gameObject.layer = layer_balls;
    }
	
}
