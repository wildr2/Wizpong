using UnityEngine;
using System.Collections;
using System;


public class MatchManager : MonoBehaviour
{
    // Other References
    public Racquet racquet1, racquet2;
    public GameBall gameball;
    public UI ui;
    public CourtEffects court_fx;
    private CameraShake cam_shake;
    
    // Current point information
    private int possession = 0; // 0 is nobody, 1 is player 1...
    private Wall live_wall = null;
    private int live_wall_possession = 0; // possession when live_wall wall was made live
    private int last_wall_possession = 0;

    // Scoring and time
    private int score_p1 = 0, score_p2 = 0;
    private float match_seconds_left, match_seconds_left_initial = 5 * 60;

    // Other
    private float seconds_between_points = 3;
    private bool point_over = false;
    private bool game_over = false;


    // PUBLIC MODIFIERS

    public void Start()
    {
        // hook up to match events
        racquet1.event_collide_ball += new System.EventHandler<EventArgs<Rigidbody2D>>(OnRacquetVsBall);
        racquet2.event_collide_ball += new System.EventHandler<EventArgs<Rigidbody2D>>(OnRacquetVsBall);
        gameball.event_collide_wall += new System.EventHandler<EventArgs<Wall>>(OnGameBallVsWall);
        gameball.event_on_death += new System.EventHandler<EventArgs>(OnGameBallDeath);      
  
        // match clock
        match_seconds_left = match_seconds_left_initial;

        // cam shake reference
        cam_shake = Camera.main.GetComponent<CameraShake>();
        if (!cam_shake) Debug.LogError("main camera has no CameraShake component");
    }
    public void Update()
    {
        if (!point_over)
        {
            // possession clock
            ui.UpdatePossessionClock(gameball.GetLifeTimeRemaining());
        }

        // match clock
        if (!game_over)
        {
            match_seconds_left -= Time.unscaledDeltaTime;
            if (match_seconds_left <= 0)
            {
                match_seconds_left = 0;
                OnGameOver();
            }
            ui.UpdateMatchClock(match_seconds_left);
        }
    }


    // PRIVATE MODIFIERS

    private void OnRacquetVsBall(object sender, EventArgs<Rigidbody2D> e)
    {
        if (point_over) return;

        Racquet r = sender as Racquet;
        if (r == null) return;

        // vs gameball
        GameBall gb = e.Value.gameObject.GetComponent<GameBall>();
        if (gb != null)
        {
            if (possession != r.player_number)
            {
                possession = r.player_number;
                OnPosessionChange();
            }
            
            gameball.SetTrailColor(r.player_color);

            gameball.SetShrinking(true);
        }
    }
    private void OnGameBallVsWall(object sender, EventArgs<Wall> e)
    {
        if (point_over) return;

        if (live_wall != null)
        {
            // if the wall hit is the live wall
            if (e.Value.gameObject == live_wall.gameObject)
            {
                // rewall
                if (possession != live_wall_possession)
                {
                    OnRewall();
                }
            }
            else
            {
                // return the live wall to normal
                live_wall.SetColorNeutral();
                live_wall = null;
            }
        }
        
        // if first wall bounce since racquet touch
        if (!gameball.HitWallSinceRacquetTouch())
        {
            // make wall live
            MakeWallLiveWall(e.Value);
        }

        // shrink racquet
        if (possession == 1)
            racquet1.ShrinkControlZone();
        else if (possession == 2)
            racquet2.ShrinkControlZone();


        // gameball lifetime reset
        if (last_wall_possession != possession)
            gameball.ResetLifeTime();
        last_wall_possession = possession;
    }
    private void OnGameBallDeath(object sender, EventArgs e)
    {
        if (point_over) return;
        OnPoint();
    }

    private void OnPosessionChange()
    {
        if (possession == 1)
        {
            racquet1.SetAttackingPlayer(true);
            racquet2.SetAttackingPlayer(false);
        }
        else
        {
            racquet2.SetAttackingPlayer(true);
            racquet1.SetAttackingPlayer(false);
        }
    }
    private void OnRewall()
    {
        SoundManager.PlayRewall();
        ui.SetScoreTextRewall();
        OnPoint();
    }
    private void MakeWallLiveWall(Wall wall)
    {
        live_wall_possession = possession;

        live_wall = wall;
        live_wall.SetColorLive(PossessorPlayerColor());
    }
    private void OnPoint()
    {
        if (point_over) return; // insure this function is not called multiple times per point
        point_over = true;

        // visual
        cam_shake.Shake(new CamShakeInstance(0.8f, 1f));
        TimeScaleManager.AddMultiplier("match_event", 0.75f);
        gameball.SetTrailEnabled(false);

        // audio
        SoundManager.PlayPoint();

        // Score
        if (possession == 1)
        {
            score_p1 += 1;
            court_fx.FlashCourtColor(racquet1.player_color);
        }
        else
        {
            score_p2 += 1;
            court_fx.FlashCourtColor(racquet2.player_color);
        }

        // score text
        ui.UpdateScoreUI(score_p1, score_p2);


        StartCoroutine("BeginNextPoint");
    }
    private void OnGameOver()
    {
        point_over = true;
        game_over = true;

        // visual
        cam_shake.Shake(new CamShakeInstance(0.8f, 3f));
        TimeScaleManager.AddMultiplier("match_event", 0.75f);
        gameball.SetTrailEnabled(false);
        
        if (score_p1 > score_p2)
        {
            court_fx.EnableConfetti(racquet1.player_color);
            court_fx.FlashCourtColor(racquet1.player_color);
        }
        else if (score_p2 > score_p1)
        {
            court_fx.EnableConfetti(racquet2.player_color);
            court_fx.FlashCourtColor(racquet2.player_color);
        }

        // audio
        SoundManager.PlayGameOver();
    }

    private IEnumerator BeginNextPoint()
    {
        yield return new WaitForSeconds(seconds_between_points);

        point_over = false;

        // audio
        SoundManager.PlayBeginPoint();

        // reset time scale
        TimeScaleManager.RemoveMultiplier("match_event");
        
        // return the live wall to normal
        if (live_wall != null)
        {
            live_wall.SetColorNeutral();
            live_wall = null;
        }

        // reset wall info
        live_wall_possession = 0;
        if (gameball.LastWall())
        {
            gameball.LastWall().SetColorNeutral();
        }

        // prepare gameball
        gameball.Reset();

        // prepare racquets
        racquet1.Reset();
        racquet2.Reset();
    }


    // PRIVATE ACCESSORS

    private Color PossessorPlayerColor()
    {
        if (possession == 1)
            return racquet1.player_color;
        if (possession == 2)
            return racquet2.player_color;

        return Color.white;
    }


    // PUBLIC ACCESSORS



}
