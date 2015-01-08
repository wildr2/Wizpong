using UnityEngine;
using System.Collections;

public class MenuBallSpawner : MonoBehaviour 
{
    public int spawn_width = 36;
    public Rigidbody2D ball_prefab;
    private Timer time_to_spawn = new Timer(2.5f);

    
    public void Start()
    {
        SpawnBall();
        time_to_spawn.on_time_up = SpawnBall;
    }

    public void Update()
    {
        time_to_spawn.UpdateIfTicking();
        if (!time_to_spawn.IsTicking()) time_to_spawn.StartTicking();
    }
    private void SpawnBall()
    {
        Vector2 pos = new Vector2(Random.Range(-spawn_width/2, spawn_width/2), 0) + (Vector2)transform.position;
        Rigidbody2D ball = (Rigidbody2D)Instantiate(ball_prefab, pos, transform.rotation);

        // set ball moving
        Vector2 direction = (-pos).normalized;
        //float angle = Random.Range(Mathf.Pi, Mathf.PI / 2f);
        float force = Random.Range(30, 55) * 0.15f;
        ball.rigidbody2D.velocity = direction * force;
    }

}
