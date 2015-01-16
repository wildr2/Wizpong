using UnityEngine;
using System.Collections;
using System;


public class MatchManager : MonoBehaviour
{
    // Other References
    public Racquet racquet1, racquet2;
    public GameBall gameball;
    public CourtUI ui;
    public CourtEffects court_fx;
    private CameraShake cam_shake;
    public GGPage gg_page;
    public MatchAudio match_audio;
    
    // Current point information
    private int last_possession = 0, possession = 0; // 0 is nobody, 1 is player 1...
    private Wall live_wall = null;
    private int live_wall_possession = 0; // possession when live_wall wall was made live
    private int last_wall_possession = 0;

    // Timing
    private float match_seconds_left, match_length_seconds = 5f * 60;
    private bool time_alert_playing = false;

    private const float seconds_between_points = 10;
    private const float seconds_to_first_point = 5;
    private float seconds_to_next_point; 

    // stats
    private int score_p1 = 0, score_p2 = 0;
    private int rewalls_p1 = 0, rewalls_p2 = 0;
    private float time_stunned_p1 = 0, time_stunned_p2 = 0;
    private float possession_time_p1 = 0, possession_time_p2 = 0;

    // Other
    private bool point_over = false;
    private bool game_over = false;


    // PUBLIC MODIFIERS

    public void Start()
    {
        // hook up to match events
        racquet1.event_collide_ball += new System.EventHandler<EventArgs<Rigidbody2D>>(OnRacquetVsBall);
        racquet2.event_collide_ball += new System.EventHandler<EventArgs<Rigidbody2D>>(OnRacquetVsBall);

        racquet1.event_stunned += new EventHandler<EventArgs<float>>(OnRacquetStunned);
        racquet2.event_stunned += new EventHandler<EventArgs<float>>(OnRacquetStunned); 

        gameball.event_collide_wall += new System.EventHandler<EventArgs<Wall>>(OnGameBallVsWall);
        gameball.event_on_death += new System.EventHandler<EventArgs>(OnGameBallDeath);      
        
  
        // cam shake reference
        cam_shake = Camera.main.GetComponent<CameraShake>();
        if (!cam_shake) Debug.LogError("main camera has no CameraShake component");

        // begin
        BeginMatch();
    }
    public void Update()
    {
        UpdateClocks();
        UpdatePossessionTimeStats();
    }


    // PRIVATE MODIFIERS

    private void UpdateClocks()
    {
        if (!point_over)
        {
            // possession clock
            float possession_seconds_left = gameball.GetLifeTimeRemaining();
            ui.UpdatePossessionClock(possession_seconds_left);

            if (!time_alert_playing && possession_seconds_left < 3)
            {
                match_audio.PlayAlertLoop();
                time_alert_playing = true;
            }
            else if (time_alert_playing && possession_seconds_left >= 3)
            {
                match_audio.StopAlertLoop();
                time_alert_playing = false;
            }
        }
        else if (!game_over)
        {
            // inter point clock
            seconds_to_next_point -= Time.deltaTime;
            ui.UpdateInterPointClock(seconds_to_next_point);
        }

        if (!game_over)
        {
            // match clock (uneffected by timescale except when paused)
            if (Time.timeScale != 0) match_seconds_left -= Time.unscaledDeltaTime;
            if (match_seconds_left <= 0)
            {
                match_seconds_left = 0;
                OnGameOver();
            }
            ui.UpdateMatchClock(match_seconds_left);
        }
    }
    private void UpdatePossessionTimeStats()
    {
        // uneffected by timescale except when paused
        if (!point_over && Time.timeScale != 0)
        {
            if (possession == 1)
                possession_time_p1 += Time.unscaledDeltaTime;
            else if (possession == 2)
                possession_time_p2 += Time.unscaledDeltaTime;
        }
    }

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
                last_possession = possession;
            }
        }
    }
    private void OnGameBallVsWall(object sender, EventArgs<Wall> e)
    {
        if (point_over) return;

        bool rewalled = false;
        if (live_wall != null)
        {
            // if the wall hit is the live wall
            if (e.Value.gameObject == live_wall.gameObject)
            {
                // rewall
                if (possession != live_wall_possession)
                {
                    rewalled = true;
                }
            }
            else
            {
                // return the live wall to normal
                live_wall.SetColorNeutral();
                live_wall = null;
            }
        }
        
        // reset visuals of last touched wall
        if (gameball.LastWall() != null)
            gameball.LastWall().SetColorNeutral();

        // if first wall bounce since racquet touch
        if (!gameball.HitWallSinceRacquetTouch())
        {
            // make wall live
            MakeWallLiveWall(e.Value);
        }
        else
        {
            // show that this wall was touched
            e.Value.SetColorTouched();
        }
         
        last_wall_possession = possession;


        // do rewall after all else
        if (rewalled)
            OnRewall();
    }
    private void OnGameBallDeath(object sender, EventArgs e)
    {
        if (point_over) return;
        OnPoint();
    }
    private void OnRacquetStunned(object sender, EventArgs<float> e)
    {
        Racquet r = sender as Racquet;
        if (r == null) return;

        if (r.player_number == 1) time_stunned_p1 += e.Value;
        else time_stunned_p2 += e.Value;
    }

    private void OnPosessionChange()
    {
        // racquet changes
        if (possession == 1)
        {
            racquet1.SetAttackingPlayer(true);
            racquet2.SetAttackingPlayer(false);
            gameball.SetTrailColor(racquet1.player_color);
        }
        else
        {
            racquet2.SetAttackingPlayer(true);
            racquet1.SetAttackingPlayer(false);
            gameball.SetTrailColor(racquet2.player_color);
        }

        // gameball lifetime reset
        if (last_wall_possession != possession)
            gameball.StartShrinking();

        // audio
        if (last_possession != 0)
            match_audio.PlayPosessionChange();
    }
    private void OnRewall()
    {
        match_audio.PlayRewall();
        ui.SetScoreTextRewall();

        if (possession == 1) rewalls_p1 += 1;
        else rewalls_p2 += 1;

        OnPoint();
    }
    private void MakeWallLiveWall(Wall wall)
    {
        live_wall_possession = possession;

        live_wall = wall;
        live_wall.SetColorLive(PossessorPlayerColor());

        // audio
        match_audio.PlayLiveWall();
    }
    private void OnPoint()
    {
        if (point_over) return; // insure this function is not called multiple times per point
        point_over = true;

        // seconds to next point
        // if there are not enough seconds left for the inter point time, the next point
        //   will begin immediately
        if (match_seconds_left > seconds_between_points)
            seconds_to_next_point = seconds_between_points;
        else
            seconds_to_next_point = 0;

        // visual
        cam_shake.Shake(new CamShakeInstance(0.8f, 1f));
        gameball.Hide();

        // audio
        match_audio.PlayPoint();
        match_audio.StopAlertLoop();
        time_alert_playing = false;

        // racquets
        racquet1.Reset();
        racquet2.Reset();

        // walls
        StartCoroutine("ResetWallsAfterDelay");

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

        // possession
        possession = 0;
        last_possession = 0;

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
        TimeScaleManager.AddMultiplier("match_event", 0.5f);
        gameball.Hide();
        
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

        // walls
        StartCoroutine("ResetWallsAfterDelay");

        // audio
        match_audio.PlayGameOver();
        match_audio.StopAlertLoop();
        time_alert_playing = false;

        // end page
        StartCoroutine("OpenGGPageAfterDelay");
    }
    private void ResetWalls()
    {
        // return the live wall to normal
        if (live_wall != null)
        {
            live_wall.FadeColortoNeutral();
            live_wall = null;
        }
        live_wall_possession = 0;

        // reset last touched wall
        if (gameball.LastWall())
        {
            gameball.LastWall().FadeColortoNeutral();
        }
    }

    private void BeginMatch()
    {
        point_over = true;
        game_over = false;

        // match clock
        match_length_seconds = (GameSettings.Instance.match_type == 0 ? 5 :
                                GameSettings.Instance.match_type == 1 ? 10 : 1) * 60;
        match_seconds_left = match_length_seconds;

        // inter point clock
        seconds_to_next_point = seconds_to_first_point;

        // gameball
        gameball.Hide();

        StartCoroutine("BeginNextPoint");
    }
    private IEnumerator BeginNextPoint()
    {
        yield return new WaitForSeconds(seconds_to_next_point);

        point_over = false;

        // reset possession
        possession = 0;
        last_possession = 0;

        // audio
        match_audio.PlayBeginPoint();

        // reset time scale
        TimeScaleManager.RemoveMultiplier("match_event");

        // prepare gameball
        gameball.Reset();

        // prepare racquets
        racquet1.Reset();
        racquet2.Reset();
    }
    private IEnumerator ResetWallsAfterDelay()
    {
        yield return new WaitForSeconds(seconds_to_next_point / 2f);
        ResetWalls();
    }
    private IEnumerator OpenGGPageAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        gg_page.TransitionIn();
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

    public int GetPointsP1()
    {
        return score_p1;
    }
    public int GetPointsP2()
    {
        return score_p2;
    }
    public int GetRewallsP1()
    {
        return rewalls_p1;
    }
    public int GetRewallsP2()
    {
        return rewalls_p2;
    }
    public float GetTimeStunnedP1()
    {
        return time_stunned_p1;
    }
    public float GetTimeStunnedP2()
    {
        return time_stunned_p2;
    }
    public float GetPosessionPercentageP1()
    {
        float total = possession_time_p1 + possession_time_p2;
        if (total == 0) return 0.5f;
        return possession_time_p1 / total;
    }

    public int GetWinningPlayer()
    {
        return score_p1 > score_p2 ? 1 : score_p1 < score_p2 ? 2 : 0; 
    }
    public bool GameOver()
    {
        return game_over;
    }
    public bool PointOver()
    {
        return point_over;
    }
}
