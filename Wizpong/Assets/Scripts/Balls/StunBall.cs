using UnityEngine;
using System.Collections;

public class StunBall : MonoBehaviour 
{
    private int controlling_player = 0; // 0 is noone, 1 is player 1...
    private const float duration_controlled = 2;
    private const float stun_duration = 2;

    public SpriteRenderer renderer;
    private Color color_uncontrolled;

    private Collider2D controlling_colider;
    public string layer_name_racquet_czone;
    private int layer_racquet_czone;



    public void Start()
    {
        color_uncontrolled = renderer.color;
        layer_racquet_czone = LayerMask.NameToLayer(layer_name_racquet_czone);
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
                    r.Stun(stun_duration);
                }
            }
        }
    }
    public void TryTakeControlOfBall(Racquet racquet, Collider2D physical_collider)
    {
        if (controlling_player == 0)
        {
            // set controlling player
            controlling_player = racquet.player_number;
            renderer.color = Color.Lerp(color_uncontrolled, racquet.player_color, 0.35f);

            // insure the ball only collides with the opponent (physical racquet)
            controlling_colider = physical_collider;
            Physics2D.IgnoreCollision(this.collider2D, controlling_colider);
            Physics2D.IgnoreLayerCollision(gameObject.layer, layer_racquet_czone, true);

            StartCoroutine("ResetControllingPlayer");
        } 
    }


    private IEnumerator ResetControllingPlayer()
    {
        yield return new WaitForSeconds(duration_controlled);
        
        controlling_player = 0;
        renderer.color = color_uncontrolled;

        // allow the ball to be controlled by racquets again
        Physics2D.IgnoreCollision(this.collider2D, controlling_colider, false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, layer_racquet_czone, false);
    }
	
}
