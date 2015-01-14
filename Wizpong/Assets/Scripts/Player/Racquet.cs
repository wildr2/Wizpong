using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Racquet : MonoBehaviour 
{
    // General
    public int player_number;
    public Color player_color;

    // Rendering
    public SpriteRenderer ring_sprite, inside_sprite;
    public Transform control_zone; // object with the control zone trigger collider and sprite

    // Other
    private CameraShake cam_shake;
    public ControlZoneAudio czone_audio;
    public RacquetAudio audio;

    // Position and court bounds
    private const int bounds_width = 31, bounds_height = 17;
    private Vector2 initial_pos;

    // Finesse ability
    private bool using_finesse = false;
    private float finesse_heat = 0; // 0 to 1, 1 is overheated, increases as finesse is used

    // Ball control
    private Rigidbody2D controlled_ball = null;
    private const float max_czone_radius = 8f;
    private const float normal_czone_radius= 1.5f;
    private float czone_radius = 1.5f;

    // Movement
    private const float max_speed_attacking = 6, max_speed_defending = 3;
    private float max_speed;

    // Stun
    private bool stunned = false;
    private float stun_time_left = 0;
    private float stun_duration;
    private const float default_stun_duration = 1f;
    private const float finesse_overheat_stun_duration = 2f;
    
    // Lightning ability
    public Lightning lightning_prefab;
    public ShockBall[] shock_balls; 

    // Input
    private Vector2 input_direction = Vector2.zero;
    private bool input_finesse = false;

    // Events
    public event EventHandler<EventArgs<Rigidbody2D>> event_collide_ball;
    public event EventHandler<EventArgs<float>> event_stunned;


    // PUBLIC MODIFIERS

    public void Awake()
    {   
        // set colors
        player_color = GameSettings.GetPlayerColor(player_number);
        ring_sprite.color = player_color;
        inside_sprite.color = player_color;

        // save initial racquet pos
        initial_pos = transform.position;

        // pool lightning objects
        ObjectPool.Instance.RequestObjects(lightning_prefab, 3, false);
    }
    public void Start()
    {
        max_speed = max_speed_attacking;

        // get references
        cam_shake = Camera.main.GetComponent<CameraShake>();
        if (!cam_shake) Debug.LogError("main camera has no CameraShake component");
    }
    public void Update()
    {
        if (!Stunned()) UpdateMovement();
        UpdateBallControl();
        UpdateControlZoneChangingSize();
        UpdateFinesseAbility();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // VS Ball
        if (controlled_ball == null && collider.CompareTag("Ball") && collider.rigidbody2D != null)
        {
            controlled_ball = collider.rigidbody2D;
            OnBallEnterCZone();

            // VS Gameball
            GameBall gameball = controlled_ball.GetComponent<GameBall>();
            if (gameball != null)
            {
                gameball.RacquetTouch();
            }
            else
            {
                // VS Stunball
                StunBall stunball = controlled_ball.GetComponent<StunBall>();
                if (stunball != null)
                {
                    stunball.TryTakeControlOfBall(this, collider2D);
                }
                else
                {
                    // VS Shockball
                    ShockBall shockball = controlled_ball.GetComponent<ShockBall>();
                    if (shockball != null)
                    {
                        shockball.TakeControlOfBall(this);
                    }
                }
            }

            event_collide_ball(this, new EventArgs<Rigidbody2D>(controlled_ball));
        }
    }

    public void SetAttackingPlayer(bool attacking)
    {
        if (attacking)
        {
            max_speed = max_speed_attacking;
        }
        else
        {
            max_speed = max_speed_defending;
        }
    }
    public void ResetControlZone()
    {
        czone_radius = 0;
        control_zone.localScale = new Vector3(czone_radius, czone_radius, 1);
        control_zone.gameObject.SetActive(true);
        czone_audio.StopEffect();

        controlled_ball = null;
    }
    public void DisableControlZone()
    {
        czone_radius = 0;
        control_zone.gameObject.SetActive(false);
        czone_audio.StopEffect();

        controlled_ball = null;
    }
    public void Stun(float duration)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.isKinematic = true;

        if (duration > stun_time_left)
        {
            if (event_stunned != null) event_stunned(this, new EventArgs<float>(duration - stun_time_left));

            stun_duration = duration;
            stun_time_left = duration;

            
        }

        if (!stunned)
        {
            stunned = true;

            StartCoroutine("UpdateStun");
        }

        // audio
        audio.PlayStunSound();
    }
    public void Stun()
    {
        Stun(default_stun_duration);
    }

    // racquet control
    public void SetMoveDirection(Vector2 direction)
    {
        input_direction = direction;
    }
    public void FireLightning()
    {
        // get shock balls this racquet is in possession of
        List<ShockBall> owned_shockballs = new List<ShockBall>();
        for (int i = 0; i < shock_balls.Length; ++i)
        {
            if (shock_balls[i].ControllingPlayer() == player_number)
            {
                owned_shockballs.Add(shock_balls[i]);
            }
        }

        // strike all owned shock balls
        for (int i = 0; i < owned_shockballs.Count; ++i)
        {
            Lightning lightning = ObjectPool.Instance.GetObject(lightning_prefab, false);
            //Lightning lightning = ObjectPool.Instance.GetPooledObject(lightning_prefab, false).GetComponent<Lightning>();
            lightning.Initialize(this);
            lightning.Fire(transform, owned_shockballs[i].transform, owned_shockballs.Count);

            owned_shockballs[i].UseCharge();
        }
        
    }
    public void SetInputUseFinesse(bool use_finesse)
    {
        this.input_finesse = use_finesse;
    }
    
    public void Reset()
    {
        SetFinesseHeat(0);
        controlled_ball = null;
        ResetControlZone();

        max_speed = max_speed_attacking;
    }


    // PRIVATE MODIFIERS

    private void UpdateMovement()
    {
        if (stunned) return;

        // slow down when no input to move
        if (input_direction.magnitude <= 0.1f)
        {
            rigidbody2D.velocity = rigidbody2D.velocity / (1 + 5f * Time.deltaTime);
        }
        else
        {
            rigidbody2D.AddForce(input_direction * 20000f * Time.deltaTime);
            rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, max_speed);
        }


        // restrict movement to court
        if (Mathf.Abs(transform.position.x) > bounds_width / 2f)
        {
            //float x = transform.position.x / bounds_width / 2f;
            //rigidbody2D.AddForce(new Vector2(-x * Time.deltaTime * 60000f, 0));
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            transform.position = new Vector2(Mathf.Sign(transform.position.x) * bounds_width / 2f, transform.position.y);
        }
        if (Mathf.Abs(transform.position.y) > bounds_height / 2f)
        {
            //float y = transform.position.y / bounds_height / 2f;
            //rigidbody2D.AddForce(new Vector2(0, -y * Time.deltaTime * 60000f));
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
            transform.position = new Vector2(transform.position.x, Mathf.Sign(transform.position.y) * bounds_height / 2f);
        }
    }
    private IEnumerator UpdateStun()
    {
        while (stun_time_left > 0)
        {
            stun_time_left -= Time.deltaTime;
            yield return null;
        }

        // on stun end
        stun_time_left = 0;
        stunned = false;
        rigidbody2D.isKinematic = false;
    }
    private void UpdateBallControl()
    {
        // if no ball has entered the contorl zone trigger collider, there is no ball to control
        if (controlled_ball == null) return;

        // if the controlled ball is still in the control zone, move it
        Vector2 dist = transform.position - controlled_ball.transform.position;
        if (dist.magnitude < czone_radius)
        {
            Vector2 force = input_direction * rigidbody2D.velocity.magnitude * 200f;
            controlled_ball.rigidbody2D.AddForce(force * Time.deltaTime);
        }
        else
        {
            // ball exits contol zone
            OnBallExitCZone();
        } 
    }
    
    private void UpdateFinesseAbility()
    {
        if (input_finesse && !stunned)
        {
            // Using finesse

            // first frame using finesse
            if (!using_finesse)
            {
                using_finesse = true;
                TimeScaleManager.AddMultiplier("finesse", 0.35f, false);
            }

            // finesse heat
            finesse_heat += Time.deltaTime;
            SetFinesseHeat(finesse_heat);

            // overheat
            if (finesse_heat >= 1)
            {
                using_finesse = false;
                TimeScaleManager.RemoveMultiplier("finesse", true);
                Stun(finesse_overheat_stun_duration);
            }
        }

        // stopped using finesse this frame
        else if (using_finesse)
        {
            TimeScaleManager.RemoveMultiplier("finesse", true);
            using_finesse = false;
        }
        else // not using finesse
        {
            // decrease heat
            if (stunned)
            {
                SetFinesseHeat(stun_time_left / stun_duration);
            }
            else if (finesse_heat > 0)
            {
                finesse_heat -= Time.deltaTime / 2f;
                finesse_heat = Mathf.Max(0, finesse_heat);
                SetFinesseHeat(finesse_heat);
            }
        }
    }
    private void SetFinesseHeat(float heat)
    {
        finesse_heat = heat;
        inside_sprite.transform.localScale = new Vector3(heat, heat, heat);
    }

    private void UpdateControlZoneChangingSize()
    {
        if (ControllingBall())
        {
            // shrink the control zone while controlling a ball
            if (czone_radius > 0)
            {
                czone_radius -= Time.deltaTime * 2f;
                if (czone_radius < 0) czone_radius = 0;
            }
        }
        else
        {
            // expand the control zone while not controlling a ball
            if (czone_radius < normal_czone_radius)
            {
                czone_radius += Time.deltaTime * 3f;
                if (czone_radius > normal_czone_radius) czone_radius = normal_czone_radius;
            }
        }

        control_zone.localScale = new Vector3(czone_radius, czone_radius, 1);
    }
    private IEnumerator SetCZoneSizeToMax()
    {
        czone_radius = max_czone_radius;
        control_zone.localScale = new Vector3(czone_radius, czone_radius, 1);
        czone_audio.StartEffect();

        // only enable the collider at the end of the frame so as not to disrupt other collisions
        yield return new WaitForEndOfFrame();
        control_zone.gameObject.SetActive(true);
    }

    private void OnBallEnterCZone()
    {
        StartCoroutine("SetCZoneSizeToMax");
    }
    private void OnBallExitCZone()
    {
        controlled_ball = null;
        ResetControlZone();
    }


    // PUBLIC ACCESSORS 

    public bool Stunned()
    {
        return stunned;
    }
    public bool ControllingBall()
    {
        return controlled_ball != null;
    }
}
