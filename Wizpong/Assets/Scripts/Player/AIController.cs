using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour 
{
    public Racquet racquet;
    public MatchManager match;


    public void Start()
    {
        StartCoroutine("UpdateInput");
        racquet.event_collide_ball += new System.EventHandler<EventArgs<Rigidbody2D>>(OnRacquetVsBallCollision);
    }
    public void Instantiate(Racquet racquet, MatchManager match)
    {
        this.racquet = racquet;
        this.match = match;
    }

    private IEnumerator UpdateInput()
    {
        while (true)
        {
            UpdateMovement();
            //UpdateLightning();

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnRacquetVsBallCollision(object sender, EventArgs<Rigidbody2D> e)
    {

        // gameball collision
        GameBall gb = e.Value.gameObject.GetComponent<GameBall>();
        if (gb != null)
        {
            // choose a new direction to push the ball in

            Vector2 move_direction = new Vector2();          
            if (Random.Range(0f, 1f) < 0f)
            {
                Racquet r = racquet.player_number == 1 ? match.racquet2 : match.racquet1;
                move_direction = (racquet.transform.position - r.transform.position).normalized;
            }
            else
            {
                float angle = Random.Range(0, Mathf.PI * 2f);
                move_direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            }

            racquet.SetMoveDirection(move_direction);
        }
    }
    private void UpdateMovement()
    {
        if (racquet.ControllingBall() || match.PointOver()) return;

        Vector2 move_direction = new Vector2();

        // go for the game ball
        Vector2 v_to_gameball = match.gameball.transform.position - racquet.transform.position;
        move_direction = v_to_gameball.normalized;
        
        // enable control zone when close to ball
        if (v_to_gameball.magnitude < 2.5f && !racquet.ControlZoneEnabled())
        {
            racquet.EnableControlZone();
            racquet.SetInputUseFinesse(true);
            StartCoroutine("StopFinesseAfterWait");
        }


        racquet.SetMoveDirection(move_direction);
    }
    private void UpdateLightning()
    {
        if (Random.Range(0, 100) < 3f)
        {
            racquet.FireLightning();
        }
    }

    private IEnumerator StopFinesseAfterWait()
    {
        float wait = Random.Range(0.1f, 0.5f);
        yield return new WaitForSeconds(wait);

        if (!racquet.ControllingBall()) racquet.DisableControlZone();
        racquet.SetInputUseFinesse(false);
    }
}
