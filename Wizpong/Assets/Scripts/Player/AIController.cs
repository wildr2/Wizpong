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
    public void Update()
    {
        
        // movement
        //    while (true)
        //    {
        //        if (game_ball.Possession() != player_number     || true)
        //        {
        //            // go for the ball
        //            input_direction = game_ball.transform.position - transform.position;
        //        }
        //        else if (!controlling_ball)
        //        {
        //            if (true)
        //            {
        //                // follow the opponent
        //                if (player_number == 1)
        //                    input_direction = game_ball.racquet2.transform.position - transform.position;
        //                else
        //                    input_direction = game_ball.racquet1.transform.position - transform.position;
        //            }
        //            else
        //            {
        //                //return to the middle
        //                input_direction = (-transform.position).normalized;
        //            }
        //        }

        //        // lightning
        //        if (Random.Range(0, 50) <= 1)
        //        {
        //            input_ligtning = true;
        //        }


        //        input_direction.Normalize();
        //        input_direction = new Vector2(Mathf.RoundToInt(input_direction.x), Mathf.RoundToInt(input_direction.y));

        //        yield return new WaitForSeconds(0.1f);
        //    }


        
    }

    private IEnumerator UpdateInput()
    {
        while (true)
        {
            UpdateMovement();
            UpdateLightning();

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
        if (racquet.ControllingBall()) return;

        Vector2 move_direction = new Vector2();

        // go for the game ball
        move_direction = (match.gameball.transform.position - racquet.transform.position).normalized;

        racquet.SetMoveDirection(move_direction);
    }
    private void UpdateLightning()
    {
        if (Random.Range(0, 100) < 3f)
        {
            racquet.FireLightning();
        }
    }
}
