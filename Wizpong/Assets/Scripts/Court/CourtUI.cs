using UnityEngine;
using System.Collections;

public class CourtUI : MonoBehaviour 
{
    // On court text
    public TextMesh score_text;
    public TextMesh match_clock;
    public TextMesh possession_clock;

    // Player color markers
    public SpriteRenderer color_marker1, color_marker2;

    // Other
    private int score_p1 = 0, score_p2;
    private bool showing_rewall_text = false;


    // PUBLIC MODIFIERS

    public void Awake()
    {
        // cursor
        //Screen.showCursor = false;
        //Screen.lockCursor = true;
    }
    public void Start()
    {
        // color markers
        color_marker1.color = GameSettings.Instance.GetPlayerColor(1);
        color_marker2.color = GameSettings.Instance.GetPlayerColor(2);
    }

    public void UpdateMatchClock(float time)
    {
        match_clock.text = FormatMinSecTimer(time);
    }
    public void UpdatePossessionClock(float time)
    {
        possession_clock.text = "score in " + FormatSecMsecTimer(time);
    }
    public void UpdateInterPointClock(float time)
    {
        possession_clock.text = "next point in " + FormatSecMsecTimer(time);
    }

    public void ResetScoreUI()
    {
        score_p1 = 0;
        score_p2 = 0;
        if (!showing_rewall_text) score_text.text = "0   0";
    }
    public void UpdateScoreUI(int score_p1, int score_p2)
    {
        this.score_p1 = score_p1;
        this.score_p2 = score_p2;
        if (!showing_rewall_text) score_text.text = score_p1.ToString() + "   " + score_p2.ToString();
    }
    public void SetScoreTextRewall()
    {
        score_text.text = "REWALL";
        StartCoroutine("ResetRewallTextToScore");
        showing_rewall_text = true;
    }

    
    // PRIVATE MODIFIERS

    private IEnumerator ResetRewallTextToScore()
    {
        yield return new WaitForSeconds(1.5f);
        score_text.text = score_p1.ToString() + "   " + score_p2.ToString();
        showing_rewall_text = false;
    }
    
    private string FormatSecMsecTimer(float seconds)
    {
        if (Mathf.Abs(seconds) < 0.1f) return "0:00";

        int second = Mathf.FloorToInt(seconds);
        int deci = 0;

        if (second > 0) deci = (int)((seconds % second) * 100f);
        else deci = (int)(seconds * 100f);

        return second + ":" + deci + (deci < 10 ? "0" : "");
    }
    private string FormatMinSecTimer(float seconds)
    {
        if (Mathf.Abs(seconds) < 0.1f) return "0:00";

        int min = (int)(seconds / 60);
        int sec = (int)(seconds % 60);

        return min + ":" + (sec < 10 ? "0" : "") + sec;
    }
}
