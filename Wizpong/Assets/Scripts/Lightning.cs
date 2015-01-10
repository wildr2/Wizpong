using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Lightning : MonoBehaviour 
{
    public Racquet racquet;

    // Visual
    private int max_redraws = 6;
    private int redraw_count = 0;

    private const int max_num_positions = 30;
    private const float zigzag_intensity = 0.5f;
    public LineRenderer line;

    private Transform bolt_start, bolt_end;
    private float fixed_distance = 0;
    private Vector2 direction;

    // stun
    private const float stun_duration = 1f;
    private int power = 1; // stun_duration is multiplied by power



    // References
    private CameraShake cam_shake;
    public LayerMask racquets_layer;



    // PUBLIC MODIFIERS

    public void Start()
    {
        // get references
        cam_shake = Camera.main.GetComponent<CameraShake>();
        if (!cam_shake) Debug.LogError("no CameraShake found");

        line.SetColors(Color.Lerp(racquet.player_color, Color.white, 0.5f), racquet.player_color);
    }
    public void Fire(Transform bolt_start, Transform bolt_end, int power)
    {
        this.bolt_start = bolt_start;
        this.bolt_end = bolt_end;

        // visual
        cam_shake.Shake(new CamShakeInstance(0.3f, 0.1f));
        line.enabled = true;
        redraw_count = 0;
        
        // audio
        SoundManager.PlayShock();

        // draw and collision (stun players)
        this.power = power;
        StartCoroutine(ReCreateBolt());
    }
    public void Fire(Transform bolt_start, Transform bolt_end, float fixed_distance, int power)
    {
        this.fixed_distance = fixed_distance;
        direction = (bolt_end.position - bolt_start.position).normalized;

        Fire(bolt_start, bolt_end, power);
    }


    // PRIVATE MODIFIERS

    private IEnumerator ReCreateBolt()
    {
        while (redraw_count < max_redraws)
        {
            // redraw and collide
            ReDrawLine();
            HandleCollision();

            ++redraw_count;
            yield return new WaitForSeconds(0.05f);
        }

        // finish bolt
        redraw_count = 0;
        line.enabled = false;
    }
    private void ReDrawLine()
    {
        Vector2 pos1 = bolt_start.position;
        Vector2 pos2 = bolt_end.position;
        if (fixed_distance > 0)
        {
            direction = (bolt_end.position - bolt_start.position).normalized;
            pos2 = pos1 + direction * fixed_distance;
        }

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
        if (fixed_distance > 0)
        {
            pos2 = pos1 + direction * fixed_distance;
        }

        // see if the player is hit (and hit them)
        RaycastHit2D[] hits = Physics2D.LinecastAll(pos1, pos2, racquets_layer.value);

        foreach (RaycastHit2D hit in hits)
        {
            // stun opponent racquet
            Racquet r = hit.collider.GetComponent<Racquet>();
            if (r != null && r.player_number != racquet.player_number) // affect only the opponent
            {
                Vector2 force_direction = ((Vector2)hit.collider.transform.position - (Vector2)bolt_start.position).normalized;
                r.Stun(stun_duration * power);
            }
        }
    }
}
