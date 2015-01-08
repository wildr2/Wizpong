using UnityEngine;
using System.Collections;

public class MenuBall : MonoBehaviour
{
    private ParticleSystem ps;
    private bool waiting_for_ps = false;


    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (!ps) Debug.LogError("menu ball particle system not found");
    }
    public void Update()
    {
        if (waiting_for_ps)
        {
            // let particles finish emitting before destroying the object
            if (ps.particleCount == 0) Destroy(gameObject);
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.name != "Ball Spawner") return;

        waiting_for_ps = true;
        ps.enableEmission = false;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.isKinematic = true;
    }
}
